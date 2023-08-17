/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.Runtime.InteropServices;
using Live2D.Cubism.Core.Unmanaged;
using UnityEngine;
using Live2D.CubismMotionSyncPlugin.Plugins;

namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    public class CubismMotionSyncCriContext
    {
        /// <summary>
        /// Unmanaged address of CubismMotionSyncEngine.
        /// </summary>
        public IntPtr Address { get; private set; }

        /// <summary>
        /// Number of samples required by the engine.
        /// </summary>
        public uint RequireSampleCount => CubismMotionSyncEngine_CRI.GetRequireSampleCount(Address);

        private IntPtr UnmanagedSampleArrayAddress { get; set; }

        private CubismUnmanagedFloatArrayView SamplesArray { get; set; }


        public void Create(CubismMotionSyncCriContextConfig contextConfig, CubismMotionSyncCriMappingInfoCollection mappingInfoCollection)
        {
            Address = CubismMotionSyncEngine_CRI.CreateContext(contextConfig.Address, mappingInfoCollection.Address, mappingInfoCollection.Length);


            if (Address == IntPtr.Zero)
            {
                Debug.LogError("[CubismMotionSyncCriContext.Create]: Failed to create context. Please verify .motionsync3.json data.");
            }


            SamplesArray = new CubismUnmanagedFloatArrayView(IntPtr.Zero, 0);
        }

        /// <summary>
        /// Cleans up the resources.
        /// </summary>
        public void Delete()
        {
            CubismMotionSyncEngine_CRI.DeleteContext(Address);

            CubismUnmanagedMemory.Deallocate(UnmanagedSampleArrayAddress);


            // Cleaning up.
            Address = IntPtr.Zero;
            UnmanagedSampleArrayAddress = IntPtr.Zero;
        }

        public unsafe bool Analyze(uint requireSampleCount, CubismMotionSyncCriAnalysisResult analysisResult, CubismMotionSyncCriAnalysisConfig analysisConfig, CubismMotionSyncCriAudioInput motionSyncAudioInput)
        {
            if (SamplesArray.Length < requireSampleCount)
            {
                if (SamplesArray.IsValid)
                {
                    // Re-allocation.
                    // Deallocate it when already exists.
                    CubismUnmanagedMemory.Deallocate(UnmanagedSampleArrayAddress);
                }


                var requireSampleSize = (int)(Marshal.SizeOf(typeof(float)) * requireSampleCount);

                UnmanagedSampleArrayAddress = CubismUnmanagedMemory.Allocate(requireSampleSize, CubismMotionSyncCriProcessor.DefaultAlign);
                SamplesArray = new CubismUnmanagedFloatArrayView(UnmanagedSampleArrayAddress, (int)requireSampleCount);
            }


            for (var i = 0; i < requireSampleCount; i++)
            {
                SamplesArray[i] = motionSyncAudioInput.ReadSample();
            }


            var result = CubismMotionSyncEngine_CRI.Analyze(Address, (float*)UnmanagedSampleArrayAddress.ToPointer(), requireSampleCount, analysisResult.Address, analysisConfig.Address);

            return (result != 0);
        }
    }
}
