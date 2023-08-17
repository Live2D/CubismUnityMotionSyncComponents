/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Core.Unmanaged;

namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    /// <summary>
    /// Handled when using CRI.
    /// </summary>
    public class CubismMotionSyncCriProcessor : MonoBehaviour, ICubismMotionSyncProcessor
    {
        public const string VisemeName_SIL = "Silence";
        public const string VisemeName_A = "A";
        public const string VisemeName_I = "I";
        public const string VisemeName_U = "U";
        public const string VisemeName_E = "E";
        public const string VisemeName_O = "O";

        public const int UseBitDepth = 32;

        /// <summary>
        /// Default alignment for memory allocation.
        /// </summary>
        public const int DefaultAlign = CubismCoreDll.AlignofModel;

        /// <summary>
        /// Enum of shape of the mouth.
        /// </summary>
        public enum Viseme
        {
            SIL = 0,
            A = 1,
            I = 2,
            U = 3,
            E = 4,
            O = 5,
        }

        /// <summary>
        /// Names of viseme.
        /// </summary>
        public readonly string[] VisemeNames =
        {
            VisemeName_SIL,
            VisemeName_A,
            VisemeName_I,
            VisemeName_U,
            VisemeName_E,
            VisemeName_O
        };

        /// <summary>
        /// Viseme list.
        /// </summary>
        public readonly Viseme[] Visemes = { Viseme.SIL, Viseme.A, Viseme.I, Viseme.U, Viseme.E, Viseme.O };

        /// <summary>
        /// Connecting <see cref="CubismParameter"/> s to <see cref="Viseme"/>.
        /// </summary>
        public struct CubismCriParameterPair
        {
            /// <summary>
            /// OVR Parameter.
            /// </summary>
            public int VisemeIndex;

            /// <summary>
            /// Array of <see cref="CubismParameter"/>s to be used.
            /// </summary>
            public CubismParameter[] BindCubismParameters;
        }

        /// <summary>
        /// <see cref="TargetModelParameters"/> s backing field.
        /// </summary>
        [SerializeField, HideInInspector]
        private CubismParameter[] _targetModelParameters;

        /// <summary>
        /// Parameters of the Cubism model used in lip sync
        /// </summary>
        public CubismParameter[] TargetModelParameters
        {
            get { return _targetModelParameters; }
            private set { _targetModelParameters = value; }
        }

        /// <summary>
        /// <see cref="CubismCriParameterPairs"/> s backing field.
        /// </summary>
        [SerializeField, HideInInspector]
        private CubismCriParameterPair[] _cubismCriParameterPairs;

        /// <summary>
        /// Connecting <see cref="CubismParameter"/> s to <see cref="Viseme"/>.
        /// </summary>
        public CubismCriParameterPair[] CubismCriParameterPairs
        {
            get { return _cubismCriParameterPairs; }
            private set { _cubismCriParameterPairs = value; }
        }

        /// <summary>
        /// Index in <see cref="CubismMotionSyncData.Settings"/>.
        /// </summary>
        public int SettingIndex { get; set; }

        /// <summary>
        ///　Audio Listener
        /// </summary>
        [SerializeField, HideInInspector]
        private CubismMotionSyncCriAudioInput _motionSyncAudioInput;

        /// <summary>
        /// MotionSync Context;
        /// </summary>
        public CubismMotionSyncCriAudioInput MotionSyncAudioInput
        {
            get { return _motionSyncAudioInput; }
            set { _motionSyncAudioInput = value; }
        }

        /// <summary>
        /// Cache <see cref="CubismMotionSyncController"/>.
        /// </summary>
        private CubismMotionSyncController MotionSyncController { get; set; }

        /// <summary>
        /// Unmanaged address of <see cref="AnalysisContextHandle"/>.
        /// </summary>
        private CubismMotionSyncCriContextConfig ContextConfig { get; set; }

        private CubismMotionSyncCriContext Context { get; set; }

        private CubismMotionSyncCriAnalysisConfig AnalysisConfig { get; set; }

        private CubismMotionSyncCriAnalysisResult AnalysisResult { get; set; }

        private CubismMotionSyncCriPostProcessor PostProcessor { get; set; }

        /// <summary>
        /// Mapping Information of <see cref="Viseme"/>.
        /// </summary>
        private CubismMotionSyncCriMappingInfoCollection MappingInfoCollection { get; set; }

        /// <summary>
        /// Time not processed by motionSync.
        /// </summary>
        private float CurrentRemainTime { get; set; }

        private void Start()
        {
            CubismMotionSyncCriEngine.InitializeEngine();


            if (!CubismMotionSyncCriEngine.IsInitialized)
            {
                enabled = false;
                return;
            }


            if (MotionSyncAudioInput == null)
            {
                Debug.LogError("[CubismMotionSyncCriProcessor.Start]: Missing reference to CubismMotionSyncCriAudioInput component.\nPlease add CubismMotionSyncCriAudioInput component to GameObject and set a reference to CubismMotionSyncCriProcessor component.");
                return;
            }


            // Initialize.
            InitializeProcessor();
        }

        private void OnDestroy()
        {
            if (Context != null)
            {
                ReleaseProcessor();
            }


            CubismMotionSyncCriEngine.DisposeEngine();
        }

        /// <summary>
        /// Initialize Processor.
        /// </summary>
        private void InitializeProcessor()
        {
            // Get CubismMotionSyncController.
            MotionSyncController = GetComponent<CubismMotionSyncController>();

            if (MotionSyncController == null)
            {
                // Fail silently...
                return;
            }

            var setting = MotionSyncController.MotionSyncData.Settings[SettingIndex];

            // Receive parameter list data from MotionSyncData.
            TargetModelParameters = new CubismParameter[setting.CubismParameters.Length];

            for (var parameterIndex = 0; parameterIndex < setting.CubismParameters.Length; parameterIndex++)
            {
                // Get CubismParameter reference.
                TargetModelParameters[parameterIndex] = setting.CubismParameters[parameterIndex].Parameter;
            }

            CubismCriParameterPairs = new CubismCriParameterPair[Visemes.Length];

            for (var mappingIndex = 0; mappingIndex < setting.Mappings.Length; mappingIndex++)
            {
                var isContainsViseme = false;
                for (var visemeIndex = 0; visemeIndex < Visemes.Length; visemeIndex++)
                {
                    if (string.Equals(setting.Mappings[mappingIndex].AudioParameterId ,VisemeNames[visemeIndex]))
                    {
                        continue;
                    }

                    CubismCriParameterPairs[mappingIndex].VisemeIndex = visemeIndex;
                    isContainsViseme = true;
                    break;
                }

                if (!isContainsViseme)
                {
                    Debug.LogError("[CubismMotionSyncCriProcessor.InitializeProcessor]: Invalid Viseme detected.");
                    continue;
                }

                CubismCriParameterPairs[mappingIndex].BindCubismParameters
                    = new CubismParameter[setting.Mappings[mappingIndex].Targets.Length];

                // Assign CubismParameters.
                for (var targetIndex = 0; targetIndex < setting.Mappings[mappingIndex].Targets.Length; targetIndex++)
                {
                    CubismCriParameterPairs[mappingIndex].BindCubismParameters[targetIndex]
                        = setting.Mappings[mappingIndex].Targets[targetIndex].Parameter;
                }
            }

            // Sort by Viseme Order.
            Array.Sort(CubismCriParameterPairs, (a, b) => a.VisemeIndex - b.VisemeIndex);

            // Create contents.
            CreateProcessorContents(setting);
        }

        /// <summary>
        /// Creation of contents for use in Processor.
        /// </summary>
        /// <param name="setting">Motion sync setting to be used.</param>
        private void CreateProcessorContents(CubismMotionSyncData.SerializableSetting setting)
        {
            PostProcessor = new CubismMotionSyncCriPostProcessor();
            PostProcessor.Create(setting);


            AnalysisConfig = new CubismMotionSyncCriAnalysisConfig();
            AnalysisConfig.Create(setting);


            // Creating a structure to contain the analysis results.
            AnalysisResult = new CubismMotionSyncCriAnalysisResult();
            AnalysisResult.Create(setting);


            ContextConfig = new CubismMotionSyncCriContextConfig();
            ContextConfig.Create(MotionSyncAudioInput.Frequency, UseBitDepth);


            MappingInfoCollection = new CubismMotionSyncCriMappingInfoCollection();
            MappingInfoCollection.Create(setting);


            Context = new CubismMotionSyncCriContext();
            Context.Create(ContextConfig, MappingInfoCollection);
        }

        /// <summary>
        /// Delete context and Freeing Memory.
        /// </summary>
        private void ReleaseProcessor()
        {
            // Context.
            Context.Delete();

            MappingInfoCollection.Delete();

            // Config.
            ContextConfig.Delete();

            // AnalysisResult.
            AnalysisResult.Delete();

            // AnalysisConfig.
            AnalysisConfig.Delete();

            PostProcessor.Delete();
        }

        /// <summary>
        /// Update motion sync status.
        /// </summary>
        /// <param name="motionSyncData">motion sync data</param>
        public void UpdateCubismMotionSync(CubismMotionSyncData motionSyncData)
        {
            if (!enabled || MotionSyncAudioInput == null || TargetModelParameters == null)
            {
                // Fail silently...
                return;
            }

            // Calculate based on the analysis information of current frame.
            UpdateParameterToProcessor();
        }

        /// <summary>
        /// Update CubismParameter to Processor.
        /// </summary>
        /// <param name="frame"></param>
        private void UpdateParameterToProcessor()
        {
            var deltaTime = Time.deltaTime;
            CurrentRemainTime += deltaTime;

            // Check each time assuming it may have been updated.
            var fps = MotionSyncController.MotionSyncData.Settings[SettingIndex].PostProcessing.SampleRate;

            MotionSyncController.MotionSyncData.Settings[SettingIndex].PostProcessing.SampleRate = Mathf.Clamp(fps, 1.0f, 120.0f);
            fps = MotionSyncController.MotionSyncData.Settings[SettingIndex].PostProcessing.SampleRate;
            var processorDeltaTime = 1.0f / fps;

            // If the specified frame time is not reached, no analysis is performed.
            if (CurrentRemainTime < processorDeltaTime)
            {
                for (var targetIndex = 0; targetIndex < TargetModelParameters.Length; targetIndex++)
                {
                    if (TargetModelParameters[targetIndex] == null
                            || float.IsNaN(AnalysisResult.Values[targetIndex]))
                    {
                        continue;
                    }

                    // Overwrite parameter values every frame to prevent data from replacing itself.
                    TargetModelParameters[targetIndex].Value = PostProcessor.LastDampedParameterValues[targetIndex];
                }
                return;
            }

            Analyze();

            // Reset counter.
            CurrentRemainTime = CurrentRemainTime % processorDeltaTime;

            for (var targetIndex = 0; targetIndex < TargetModelParameters.Length; targetIndex++)
            {
                if (TargetModelParameters[targetIndex] == null
                        || float.IsNaN(AnalysisResult.Values[targetIndex]))
                {
                    continue;
                }

                // Overwrite parameter values every frame to prevent data from replacing itself.
                TargetModelParameters[targetIndex].Value = PostProcessor.LastDampedParameterValues[targetIndex];
            }
        }

        /// <summary>
        /// Calculate <see cref="TargetModelParameters"/> values from audio.
        /// </summary>
        private void Analyze()
        {
            if (MotionSyncAudioInput.AudioBuffer.Length < 1)
            {
                return;
            }
            var setting = MotionSyncController.MotionSyncData.Settings[SettingIndex];

            var currentPosition = MotionSyncAudioInput.CurrentWritePosition;
            var lastReadPosition = MotionSyncAudioInput.CurrentReadPosition;

            var requireSampleCount = Context.RequireSampleCount;

            if (requireSampleCount == 0)
            {
                Debug.LogWarning("requireSampleCount return 0. Make sure that the context and other data you have created is correct.");
                return;
            }


            // Update AnalysisConfig.
            AnalysisConfig.BlendRatio = setting.PostProcessing.BlendRatio;
            AnalysisConfig.AudioLevelEffectRatio = setting.EmphasisLevel;
            AnalysisConfig.Smoothing = setting.PostProcessing.Smoothing;

            // Overwrites older data blocks since the size is fixed.
            AnalysisConfig.CommitChanges();


            for (var currentReadPosition = lastReadPosition; currentReadPosition + requireSampleCount < currentPosition; currentReadPosition += requireSampleCount)
            {
                var result = Context.Analyze(requireSampleCount, AnalysisResult, AnalysisConfig, MotionSyncAudioInput);

                if (!result)
                {
                    Debug.LogError("[CubismMotionSyncCriProcessor.Analyze]: Failed Analyze...");
                    break;
                }

                AnalysisResult.PullData();

                requireSampleCount = Context.RequireSampleCount;
                PostProcessor.Process(TargetModelParameters, AnalysisResult);
            }
        }
    }
}
