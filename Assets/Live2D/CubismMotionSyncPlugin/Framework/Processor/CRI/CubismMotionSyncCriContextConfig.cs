/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.Runtime.InteropServices;
using Live2D.Cubism.Core.Unmanaged;
using Live2D.CubismMotionSyncPlugin.Plugins;

namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    /// <summary>
    ///
    /// </summary>
    public class CubismMotionSyncCriContextConfig
    {
        public IntPtr Address { get; private set; }

        /// <summary>
        /// Create <see cref="CubismMotionSyncEngine_CRI.csmMotionSync_ContextConfig_CRI"/>.
        /// </summary>
        public void Create(int sampleRate, int bitDepth)
        {
            var config = new CubismMotionSyncEngine_CRI.csmMotionSync_ContextConfig_CRI
            {
                SampleRato = sampleRate,
                BitDepth = bitDepth
            };


            var size = Marshal.SizeOf(config);

            Address = CubismUnmanagedMemory.Allocate(size, CubismMotionSyncCriProcessor.DefaultAlign);
            Marshal.StructureToPtr(config, Address, false);
        }

        /// <summary>
        /// Cleans up the resources.
        /// </summary>
        public void Delete()
        {
            CubismUnmanagedMemory.Deallocate(Address);


            // Cleaning up.
            Address = IntPtr.Zero;
        }
    }
}
