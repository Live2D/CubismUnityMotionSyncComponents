/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.CubismMotionSyncPlugin.Framework.Processor;
using Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI;

namespace Live2D.CubismMotionSyncPlugin.Framework
{
    public class CubismMotionSyncController : MonoBehaviour, ICubismUpdatable
    {
        /// <summary>
        /// CubismModel cache.
        /// </summary>
        private CubismModel _model { get; set; }

        /// <summary>
        /// <see cref="MotionSyncData"/> s backing field.
        /// </summary>
        [SerializeField, HideInInspector]
        private CubismMotionSyncData _motionSyncData;

        /// <summary>
        /// Motion sync data.
        /// </summary>
        [SerializeField]
        public CubismMotionSyncData MotionSyncData
        {
            get
            {
                return _motionSyncData;
            }
            set
            {
                _motionSyncData = value;
            }
        }

        /// <summary>
        /// <see cref="Processors"/> s backing field.
        /// </summary>
        [SerializeField, HideInInspector]
        private ICubismMotionSyncProcessor[] _processors;

        /// <summary>
        /// Interface of Processors.
        /// </summary>
        public ICubismMotionSyncProcessor[] Processors
        {
            get
            {
                return _processors;
            }
            set
            {
                _processors = value;
            }
        }

        /// <summary>
        /// Model has update controller component.
        /// </summary>
        [HideInInspector]
        public bool HasUpdateController { get; set; }

        /// <summary>
        /// Called by cubism update controller. Order to invoke OnLateUpdate.
        /// </summary>
        public int ExecutionOrder
        {
            get { return CubismUpdateExecutionOrder.CubismMouthController + 1; }
        }

        /// <summary>
        /// Called by cubism update controller. Needs to invoke OnLateUpdate on Editing.
        /// </summary>
        public bool NeedsUpdateOnEditing
        {
            get { return false; }
        }

        /// <summary>
        /// Creates <see cref="ICubismMotionSyncProcessor"/>.
        /// </summary>
        public void CreateProcessors()
        {
            // Remove old processors.
            var oldProcessors = gameObject.GetComponents<ICubismMotionSyncProcessor>();

            if (MotionSyncData.Settings.Length <= oldProcessors.Length)
            {
                return;
            }

            for (var i = 0; i < oldProcessors.Length; i++)
            {
                DestroyImmediate((UnityEngine.Object)oldProcessors[i]);
            }

            // Create the Processors.
            Processors = new ICubismMotionSyncProcessor[MotionSyncData.Settings.Length];
            for (var index = 0; index < MotionSyncData.Settings.Length; index++)
            {
                switch (MotionSyncData.Settings[index].AnalysisType)
                {
                    // CRI Processor
                    case CubismMotionSyncData.AnalysisType.CRI:
                        var criProcessor = gameObject.AddComponent<CubismMotionSyncCriProcessor>();
                        Processors[index] = criProcessor;
                        Processors[index].SettingIndex = index;
                        break;

                    // Not supported.
                    case CubismMotionSyncData.AnalysisType.Unknown:
                    default:
                        Processors[index] = null;
                        Debug.LogError($"[CubismMotionSyncController.CreateProcessors]: index{index} AnalysisType is incorrect!\n Processors[{index}] will be null.");
                        break;
                }
            }
        }

        /// <summary>
        /// Called by cubism update manager.
        /// </summary>
        public void OnLateUpdate()
        {
            // Fail silently...
            if (!enabled || _model == null || MotionSyncData == null)
            {
                return;
            }

            for (var index = 0; index < MotionSyncData.Settings.Length; index++)
            {
                // Update model lipsync.
                Processors[index]?.UpdateCubismMotionSync(MotionSyncData);
            }
        }

        #region Unity Event Handling

        /// <summary>
        /// Called by Unity.
        /// </summary>
        private void OnEnable()
        {
            _model = this.FindCubismModel();

            // Get cubism update controller.
            HasUpdateController = (GetComponent<CubismUpdateController>() != null);

            if (Processors != null)
            {
                return;
            }

            Processors = new ICubismMotionSyncProcessor[MotionSyncData.Settings.Length];

            // Get all attached ICubismMotionSyncProcessor components
            var processors = GetComponents<ICubismMotionSyncProcessor>();

            for (var index = 0; index < MotionSyncData.Settings.Length; index++)
            {
                switch (MotionSyncData.Settings[index].AnalysisType)
                {
                    case CubismMotionSyncData.AnalysisType.CRI:
                        if (processors[index].GetType() != typeof(CubismMotionSyncCriProcessor))
                        {
                            Debug.LogError($"[CubismMotionSyncController.OnEnable]: index{index} AnalysisType is {CubismMotionSyncData.AnalysisType.CRI} .\n but Processors[{index}] is null.");
                            break;
                        }

                        Processors[index] = processors[index];
                        Processors[index].SettingIndex = index;
                        break;
                    // Not supported.
                    case CubismMotionSyncData.AnalysisType.Unknown:
                    default:
                        Processors[index] = null;
                        Debug.LogError($"[CubismMotionSyncController.OnEnable]: index{index} Invalid AnalysisType. Processors[{index}] will be null.");
                        break;
                }
            }
        }

        /// <summary>
        /// Called by Unity.
        /// </summary>
        private void LateUpdate()
        {
            if (!HasUpdateController)
            {
                OnLateUpdate();
            }
        }

        #endregion
    }
}
