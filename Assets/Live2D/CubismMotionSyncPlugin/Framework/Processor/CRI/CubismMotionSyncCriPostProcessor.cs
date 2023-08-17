/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using Live2D.Cubism.Core;

namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    public class CubismMotionSyncCriPostProcessor
    {
        /// <summary>
        /// Values after smoothing.
        /// </summary>
        public float[] LastSmoothedParameterValues { get; private set; }

        /// <summary>
        /// Values after dampening.
        /// </summary>
        public float[] LastDampedParameterValues { get; private set; }

        private float[] Smooths { get; set; }

        private float[] Dampers { get; set; }

        public void Create(CubismMotionSyncData.SerializableSetting setting)
        {
            // Initialize.
            LastSmoothedParameterValues = new float[setting.CubismParameters.Length];
            LastDampedParameterValues = new float[setting.CubismParameters.Length];
            Smooths = new float[setting.CubismParameters.Length];
            Dampers = new float[setting.CubismParameters.Length];


            for (var cubismParameterIndex = 0; cubismParameterIndex < setting.CubismParameters.Length; cubismParameterIndex++)
            {
                if (setting.CubismParameters[cubismParameterIndex].Parameter == null)
                {
                    LastSmoothedParameterValues[cubismParameterIndex] = float.NaN;
                    LastDampedParameterValues[cubismParameterIndex] = float.NaN;
                    continue;
                }

                LastSmoothedParameterValues[cubismParameterIndex] = setting.CubismParameters[cubismParameterIndex].Parameter.Value;
                LastDampedParameterValues[cubismParameterIndex] = setting.CubismParameters[cubismParameterIndex].Parameter.Value;


                Smooths[cubismParameterIndex] = setting.CubismParameters[cubismParameterIndex].Smooth;
                Dampers[cubismParameterIndex] = setting.CubismParameters[cubismParameterIndex].Damper;
            }
        }

        /// <summary>
        /// Cleans up the resources.
        /// </summary>
        public void Delete()
        {
            // Cleaning up.
            LastSmoothedParameterValues = null;
            LastDampedParameterValues = null;
            Smooths = null;
            Dampers = null;
        }

        /// <summary>
        /// Interpolation process after analysis.
        /// </summary>
        public void Process(CubismParameter[] targetModelParameters, CubismMotionSyncCriAnalysisResult analysisResult)
        {
            for (var targetIndex = 0; targetIndex < targetModelParameters.Length; targetIndex++)
            {
                if (targetModelParameters[targetIndex] == null
                    || float.IsNaN(analysisResult.Values[targetIndex]))
                {
                    continue;
                }

                var cacheValue = analysisResult.Values[targetIndex];
                var smooth = Smooths[targetIndex];

                // Smoothing.
                cacheValue = ((100.0f - smooth) * cacheValue + LastSmoothedParameterValues[targetIndex] * smooth) / 100.0f;

                // Assign value after smoothing.
                LastSmoothedParameterValues[targetIndex] = cacheValue;

                var damper = Dampers[targetIndex];

                // Dampening.
                if (Mathf.Abs(cacheValue - LastDampedParameterValues[targetIndex]) < damper)
                {
                    cacheValue = LastDampedParameterValues[targetIndex];
                }

                // Assign value after dampening.
                LastDampedParameterValues[targetIndex] = cacheValue;
            }
        }
    }
}
