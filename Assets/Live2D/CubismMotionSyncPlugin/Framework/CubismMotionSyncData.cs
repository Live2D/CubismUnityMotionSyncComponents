/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.CubismMotionSyncPlugin.Framework.Json;

namespace Live2D.CubismMotionSyncPlugin.Framework
{
    [Serializable]
    public class CubismMotionSyncData
    {
        #region Const Values

        /// <summary>
        /// Minimum blend ratio
        /// </summary>
        public const float BlendRatioMinValue = 0.0f;

        /// <summary>
        /// Maximum blend ratio
        /// </summary>
        public const float BlendRatioMaxValue = 1.0f;

        /// <summary>
        /// Minimum smoothing
        /// </summary>
        public const int SmoothingMinValue = 1;

        /// <summary>
        /// Maximum smoothing
        /// </summary>
        public const int SmoothingMaxValue = 100;

        /// <summary>
        /// Minimum calculation frame rate
        /// </summary>
        public const float SampleRateMinValue = 1.0f;

        /// <summary>
        /// Maximum calculation frame rate
        /// </summary>
        public const float SampleRateMaxValue = 120.0f;

        /// <summary>
        /// Minimum volume influence
        /// </summary>
        public const float EmphasisLevelMinValue = 0.0f; // Unused

        /// <summary>
        /// Maximum volume influence
        /// </summary>
        public const float EmphasisLevelDefaultValue = 0.0f; // Unused

        /// <summary>
        /// Default volume influence value
        /// </summary>
        public const float EmphasisLevelMaxValue = 1.0f; // Unused

        #endregion

        #region Helpers

        /// <summary>
        /// Enumerated types of analysis methods.
        /// </summary>
        [Serializable]
        public enum AnalysisType
        {
            CRI = 0, // CubismMotionSyncEngine_CRI
            Unknown = -1,
        }

        /// <summary>
        /// Enumerated Use Cases.
        /// </summary>
        [Serializable]
        public enum UseCase
        {
            Mouth = 0,
            Unknown = -1,
        }

        /// <summary>
        /// Enumerated mapping type.
        /// </summary>
        [Serializable]
        public enum MappingType
        {
            Shape = 0,
            Unknown = -1,
        }

        /// <summary>
        /// Settings name and id template.
        /// </summary>
        [Serializable]
        public struct SerializableDictionary
        {
            /// <summary>
            /// Id about <see cref="SerializableSetting"/>
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Name about <see cref="SerializableSetting"/>
            /// </summary>
            [SerializeField]
            public string Name;
        }

        /// <summary>
        /// Meta.
        /// </summary>
        [Serializable]
        public struct SerializableMeta
        {
            /// <summary>
            /// 
            /// </summary>
            [SerializeField]
            public int SettingCount;

            /// <summary>
            /// Id-Name pairs in `Settings` array.
            /// </summary>
            [SerializeField]
            public SerializableDictionary[] Dictionary;
        }

        /// <summary>
        /// Configuration for motion sync in <see cref="CubismParameter"/>.
        /// </summary>
        [Serializable]
        public struct SerializableCubismParameter
        {
            /// <summary>
            /// Single <see cref="CubismModel"/> parameter.
            /// </summary>
            [SerializeField]
            public CubismParameter Parameter;

            /// <summary>
            /// Parameter minimum value.
            /// </summary>
            [SerializeField]
            public float Min;

            /// <summary>
            /// Parameter maximum value.
            /// </summary>
            [SerializeField]
            public float Max;

            /// <summary>
            /// Damping Value.
            /// </summary>
            [SerializeField]
            public float Damper;

            /// <summary>
            /// Smoothing Value.
            /// </summary>
            [SerializeField]
            public int Smooth;
        }

        /// <summary>
        /// Configuration for motion sync in audio parameter.
        /// </summary>
        [Serializable]
        public struct SerializableAudioParameter
        {
            /// <summary>
            /// Name about audio parameters.
            /// NOTE: It is a string that can be used for any audio sync library.
            /// </summary>
            [SerializeField]
            public string Name;

            /// <summary>
            /// Id about audio parameters.
            /// NOTE: It is a string that can be used for any audio sync library.
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Parameter minimum value.
            /// </summary>
            [SerializeField]
            public float Min;

            /// <summary>
            /// Parameter maximum value.
            /// </summary>
            [SerializeField]
            public float Max;

            /// <summary>
            /// Scaling Value.
            /// </summary>
            [SerializeField]
            public float Scale;

            /// <summary>
            /// Synchronization enabled state.
            /// </summary>
            [SerializeField]
            public bool Enabled;
        }

        /// <summary>
        /// Template for associating Id and Value.
        /// </summary>
        [Serializable]
        public struct SerializableTarget
        {
            /// <summary>
            /// Single <see cref="CubismModel"/> parameter.
            /// </summary>
            [SerializeField]
            public CubismParameter Parameter;

            /// <summary>
            /// <see cref="CubismParameter"/> value.
            /// </summary>
            [SerializeField]
            public float Value;
        }

        /// <summary>
        /// Associating audio parameters to cubism parameters.
        /// </summary>
        [Serializable]
        public struct SerializableMapping
        {
            /// <summary>
            /// Mapping type.
            /// </summary>
            [SerializeField]
            public MappingType Type;

            /// <summary>
            /// Id about audio parameter.
            /// </summary>
            [SerializeField]
            public string AudioParameterId;

            /// <summary>
            /// Array of mapping parameter for converting audio parameter to cubism parameter.
            /// </summary>
            [SerializeField]
            public SerializableTarget[] Targets;
        }

        /// <summary>
        /// Mapping Information.
        /// </summary>
        [Serializable]
        public struct SerializableSetting
        {
            /// <summary>
            /// Id about setting.
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Types about audio analysis.
            /// </summary>
            [SerializeField]
            public AnalysisType AnalysisType;

            /// <summary>
            /// Types about use case.
            /// </summary>
            [SerializeField]
            public UseCase UseCase;

            /// <summary>
            /// Data about cubism parameters for motion sync.
            /// </summary>
            [SerializeField]
            public SerializableCubismParameter[] CubismParameters;

            /// <summary>
            /// Data about audio parameters for motion sync.
            /// </summary>
            [SerializeField]
            public SerializableAudioParameter[] AudioParameters;

            /// <summary>
            /// Data about mapping.
            /// </summary>
            [SerializeField]
            public SerializableMapping[] Mappings;

            /// <summary>
            /// Post-processing information.
            /// </summary>
            [SerializeField]
            public SerializablePostProcessing PostProcessing;

            /// <summary>
            /// Percentage of audio emphasis.
            /// </summary>
            [SerializeField]
            public float EmphasisLevel; // Unused
        }

        /// <summary>
        /// Post-processing information.
        /// </summary>
        [Serializable]
        public struct SerializablePostProcessing
        {
            /// <summary>
            /// Ratio about motion sync blending.
            /// </summary>
            [SerializeField]
            public float BlendRatio;

            /// <summary>
            /// Value about motion sync smoothing.
            /// </summary>
            [SerializeField]
            public int Smoothing;

            /// <summary>
            /// Settings about motion sync update speed.
            /// </summary>
            [SerializeField]
            public float SampleRate;
        }

        #endregion

        #region Variables

        /// <summary>
        /// Json file format version.
        /// </summary>
        [SerializeField]
        public int Version;

        /// <summary>
        /// Meta.
        /// </summary>
        [SerializeField]
        public SerializableMeta Meta;

        /// <summary>
        /// Info about motion-sync settings.
        /// </summary>
        [SerializeField]
        public SerializableSetting[] Settings;

        #endregion

        public static CubismMotionSyncData CreateInstance(CubismMotionSync3Json json, CubismModel model)
        {
            var motionSyncData = new CubismMotionSyncData();
            return CreateInstance(motionSyncData, json, model);
        }

        public static CubismMotionSyncData CreateInstance(CubismMotionSyncData motionSyncData, CubismMotionSync3Json json, CubismModel model)
        {
            // Version.
            motionSyncData.Version = json.Version;

            // Meta.
            motionSyncData.Meta.SettingCount = json.Meta.SettingCount;
            motionSyncData.Meta.Dictionary = new SerializableDictionary[json.Meta.Dictionary.Length];
            for (var metaIndex = 0; metaIndex < motionSyncData.Meta.SettingCount; metaIndex++)
            {
                motionSyncData.Meta.Dictionary[metaIndex].Id = json.Meta.Dictionary[metaIndex].Id;
                motionSyncData.Meta.Dictionary[metaIndex].Name = json.Meta.Dictionary[metaIndex].Name;
            }

            motionSyncData.Settings = new SerializableSetting[motionSyncData.Meta.SettingCount];
            // Settings
            for (var settingIndex = 0; settingIndex < motionSyncData.Meta.SettingCount; settingIndex++)
            {
                motionSyncData.Settings[settingIndex] = new SerializableSetting();

                // Id.
                motionSyncData.Settings[settingIndex].Id = json.Settings[settingIndex].Id;

                // Analysis type.
                // NOTE: This is done later because of the processing required for each analysis method.
                switch (json.Settings[settingIndex].AnalysisType)
                {
                    case "CRI":
                        // Set analysis type.
                        motionSyncData.Settings[settingIndex].AnalysisType = AnalysisType.CRI;
                        break;
                    default:
                        motionSyncData.Settings[settingIndex].AnalysisType = AnalysisType.Unknown;
                        Debug.LogError($"MotionSyncData.Settings[{settingIndex}].AnalysisType is Unkown.");
                        break;
                }

                // Set use case.
                switch (json.Settings[settingIndex].UseCase)
                {
                    case "Mouth":
                        motionSyncData.Settings[settingIndex].UseCase = UseCase.Mouth;
                        break;
                    default:
                        motionSyncData.Settings[settingIndex].UseCase = UseCase.Unknown;
                        Debug.LogError($"MotionSyncData.Settings[{settingIndex}].UseCase is Unkown.");
                        break;
                }

                motionSyncData.Settings[settingIndex].CubismParameters = new SerializableCubismParameter[json.Settings[settingIndex].CubismParameters.Length];
                // Set CubismParameter.
                for (var cubismParameterIndex = 0; cubismParameterIndex < json.Settings[settingIndex].CubismParameters.Length; cubismParameterIndex++)
                {
                    motionSyncData.Settings[settingIndex].CubismParameters[cubismParameterIndex].Parameter = model.Parameters.FindById(json.Settings[settingIndex].CubismParameters[cubismParameterIndex].Id);
                    motionSyncData.Settings[settingIndex].CubismParameters[cubismParameterIndex].Min = json.Settings[settingIndex].CubismParameters[cubismParameterIndex].Min;
                    motionSyncData.Settings[settingIndex].CubismParameters[cubismParameterIndex].Max = json.Settings[settingIndex].CubismParameters[cubismParameterIndex].Max;
                    motionSyncData.Settings[settingIndex].CubismParameters[cubismParameterIndex].Damper = json.Settings[settingIndex].CubismParameters[cubismParameterIndex].Damper;
                    motionSyncData.Settings[settingIndex].CubismParameters[cubismParameterIndex].Smooth = json.Settings[settingIndex].CubismParameters[cubismParameterIndex].Smooth;
                }

                motionSyncData.Settings[settingIndex].AudioParameters = new SerializableAudioParameter[json.Settings[settingIndex].AudioParameters.Length];
                // Set audio parameters.
                for (var audioParameterIndex = 0; audioParameterIndex < json.Settings[settingIndex].AudioParameters.Length; audioParameterIndex++)
                {
                    motionSyncData.Settings[settingIndex].AudioParameters[audioParameterIndex].Name = json.Settings[settingIndex].AudioParameters[audioParameterIndex].Name;
                    motionSyncData.Settings[settingIndex].AudioParameters[audioParameterIndex].Id = json.Settings[settingIndex].AudioParameters[audioParameterIndex].Id;
                    motionSyncData.Settings[settingIndex].AudioParameters[audioParameterIndex].Min = json.Settings[settingIndex].AudioParameters[audioParameterIndex].Min;
                    motionSyncData.Settings[settingIndex].AudioParameters[audioParameterIndex].Max = json.Settings[settingIndex].AudioParameters[audioParameterIndex].Max;
                    motionSyncData.Settings[settingIndex].AudioParameters[audioParameterIndex].Scale = json.Settings[settingIndex].AudioParameters[audioParameterIndex].Scale;
                    motionSyncData.Settings[settingIndex].AudioParameters[audioParameterIndex].Enabled = json.Settings[settingIndex].AudioParameters[audioParameterIndex].Enabled;
                }

                motionSyncData.Settings[settingIndex].Mappings = new SerializableMapping[json.Settings[settingIndex].AudioParameters.Length];
                // Set mappings.
                for (var mappingIndex = 0; mappingIndex < json.Settings[settingIndex].AudioParameters.Length; mappingIndex++)
                {
                    motionSyncData.Settings[settingIndex].Mappings[mappingIndex] = new SerializableMapping();

                    // Set type.
                    switch (json.Settings[settingIndex].Mappings[mappingIndex].Type )
                    {
                        case "Shape":
                            motionSyncData.Settings[settingIndex].Mappings[mappingIndex].Type = MappingType.Shape;
                            break;
                        default:
                            motionSyncData.Settings[settingIndex].Mappings[mappingIndex].Type = MappingType.Unknown;
                            Debug.LogError($"MotionSyncData.Settings[{settingIndex}].Mappings[{mappingIndex}].Type is Unkown.");
                            break;
                    }

                    // Set audio parameter Id.
                    motionSyncData.Settings[settingIndex].Mappings[mappingIndex].AudioParameterId = json.Settings[settingIndex].Mappings[mappingIndex].Id;

                    motionSyncData.Settings[settingIndex].Mappings[mappingIndex].Targets = new SerializableTarget[json.Settings[settingIndex].Mappings[mappingIndex].Targets.Length];
                    // Set taegets.
                    for (var targetIndex = 0; targetIndex < json.Settings[settingIndex].Mappings[mappingIndex].Targets.Length; targetIndex++)
                    {
                        motionSyncData.Settings[settingIndex].Mappings[mappingIndex].Targets[targetIndex].Parameter = model.Parameters.FindById(json.Settings[settingIndex].Mappings[mappingIndex].Targets[targetIndex].Id);
                        motionSyncData.Settings[settingIndex].Mappings[mappingIndex].Targets[targetIndex].Value = json.Settings[settingIndex].Mappings[mappingIndex].Targets[targetIndex].Value;
                    }
                }

                // Set post processing.
                motionSyncData.Settings[settingIndex].PostProcessing.BlendRatio = Mathf.Clamp01(json.Settings[settingIndex].PostProcessing.BlendRatio);
                motionSyncData.Settings[settingIndex].PostProcessing.Smoothing = Mathf.Clamp(json.Settings[settingIndex].PostProcessing.Smoothing, SmoothingMinValue, SmoothingMaxValue);
                motionSyncData.Settings[settingIndex].PostProcessing.SampleRate = Mathf.Clamp(json.Settings[settingIndex].PostProcessing.SampleRate, SampleRateMinValue, SampleRateMaxValue);

                // Set bake settings.
                motionSyncData.Settings[settingIndex].EmphasisLevel = EmphasisLevelDefaultValue;
            }

            return motionSyncData;
        }
    }
}
