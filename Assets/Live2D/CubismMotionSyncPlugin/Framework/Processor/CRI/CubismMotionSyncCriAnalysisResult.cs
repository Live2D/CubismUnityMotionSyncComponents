/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Live2D.Cubism.Core.Unmanaged;
using Live2D.CubismMotionSyncPlugin.Plugins;

namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    public class CubismMotionSyncCriAnalysisResult
    {
        /// <summary>
        /// Analysis result of CubismMotionSyncEngine.
        /// </summary>
        private CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisResult AnalysisResult { get; set; }

        /// <summary>
        /// Unmanaged address of <see cref="AnalysisResult"/>.
        /// </summary>
        public IntPtr Address { get; private set; }


        public float[] Values { get; private set; }


        /// <summary>
        /// Creating a structure to contain the analysis results
        /// </summary>
        public unsafe void Create(CubismMotionSyncData.SerializableSetting setting)
        {
            var analysisResult = new CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisResult();

            var resultValueCount = setting.CubismParameters.Length;
            var resultsSize = Marshal.SizeOf(typeof(float)) * resultValueCount;

            analysisResult.Values = (float*)CubismUnmanagedMemory.Allocate(resultsSize, CubismMotionSyncCriProcessor.DefaultAlign);
            analysisResult.ValuesCount = resultValueCount;
            analysisResult.ProcessedSampleCount = 0;

            var analysisResultPtrSize = Marshal.SizeOf(typeof(CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisResult));
            Address = CubismUnmanagedMemory.Allocate(analysisResultPtrSize, CubismMotionSyncCriProcessor.DefaultAlign);
            Marshal.StructureToPtr(analysisResult, Address, false);


            AnalysisResult = analysisResult;

            Values = new float[resultValueCount];
        }

        public unsafe void Delete()
        {
            CubismUnmanagedMemory.Deallocate((IntPtr)AnalysisResult.Values);
            

            CubismUnmanagedMemory.Deallocate(Address);


            // Cleaning up.
            Address = IntPtr.Zero;
            Values = null;
        }

        public unsafe void PullData()
        {
            Marshal.PtrToStructure<CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisResult>(Address);

            var resultStruct = Marshal.PtrToStructure<CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisResult>(Address);


            for (var targetIndex = 0; targetIndex < Values.Length; targetIndex++)
            {
                resultStruct.Values[targetIndex] = Mathf.Clamp(resultStruct.Values[targetIndex], -1.0f, 1.0f);
                Values[targetIndex] = resultStruct.Values[targetIndex];
            }
        }
    }
}
