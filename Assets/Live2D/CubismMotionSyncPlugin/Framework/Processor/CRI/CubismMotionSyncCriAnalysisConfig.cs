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
    /// Handles the analysis configuration for CRI.
    /// </summary>
    public class CubismMotionSyncCriAnalysisConfig
    {
        /// <summary>
        /// Analysis config of CubismMotionSyncEngine.
        /// </summary>
        private CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisConfig_CRI Config { get; set; }

        /// <summary>
        /// Unmanaged address of <see cref="Config"/>.
        /// </summary>
        public IntPtr Address { get; private set; }

        /// <summary>
        /// Blending ratio for vieseme.
        /// </summary>
        public float BlendRatio
        {
            get
            {
                return Config.BlendRatio;
            }
            set
            {
                var config = Config;

                config.BlendRatio = value;

                Config = config;
            }
        }

        /// <summary>
        /// Audio level effect ratio.
        /// </summary>
        public float AudioLevelEffectRatio
        {
            get
            {
                return Config.AudioLevelEffectRatio;
            }
            set
            {
                var config = Config;

                config.AudioLevelEffectRatio = value;

                Config = config;
            }
        }

        /// <summary>
        /// Smoothing value.
        /// </summary>
        public int Smoothing
        {
            get
            {
                return Config.Smoothing;
            }
            set
            {
                var config = Config;

                config.Smoothing = value;

                Config = config;
            }
        }

        /// <summary>
        /// Makes the analysis configuration from the motionSync3.json settings.
        /// </summary>
        /// <param name="setting">Settings from motionSync3.json</param>
        public void Create(CubismMotionSyncData.SerializableSetting setting)
        {

            Config = new CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisConfig_CRI
            {
                BlendRatio = setting.PostProcessing.BlendRatio,
                AudioLevelEffectRatio = setting.EmphasisLevel,
                Smoothing = setting.PostProcessing.Smoothing
            };


            var size = Marshal.SizeOf(typeof(CubismMotionSyncEngine_CRI.csmMotionSync_AnalysisConfig_CRI));

            Address = CubismUnmanagedMemory.Allocate(size, CubismMotionSyncCriProcessor.DefaultAlign);


            Marshal.StructureToPtr(Config, Address, false);
        }

        /// <summary>
        /// Commits the changes to unmanaged memory.
        /// </summary>
        public void CommitChanges()
        {
            Marshal.StructureToPtr(Config, Address, true);
        }

        /// <summary>
        /// Cleans up the resouces.
        /// </summary>
        public void Delete()
        {
            CubismUnmanagedMemory.Deallocate(Address);


            // Cleaning up.
            Address = IntPtr.Zero;
        }
    }
}
