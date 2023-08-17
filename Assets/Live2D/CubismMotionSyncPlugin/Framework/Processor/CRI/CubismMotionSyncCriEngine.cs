/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.Runtime.InteropServices;
using Live2D.CubismMotionSyncPlugin.Plugins;
using UnityEngine;

namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    public static class CubismMotionSyncCriEngine
    {
        public static bool IsInitialized => ReferenceCount > 0;

        private static int ReferenceCount { get; set; }

        /// <summary>
        /// Get the name of engine.
        /// </summary>
        /// <returns>Name of engine.</returns>
        public static unsafe string GetEngineName()
        {
            var engineName = (IntPtr)CubismMotionSyncEngine_CRI.GetEngineName();
            return Marshal.PtrToStringAnsi(engineName);
        }

        /// <summary>
        /// Get the version of engine.
        /// </summary>
        /// <returns>Version of engine.</returns>
        public static string GetEngineVersion()
        {
            var rawVersion = CubismMotionSyncEngine_CRI.GetEngineVersion();

            // Conversion.
            var major = ((rawVersion & 0xFF000000) >> 24);
            var minor = ((rawVersion & 0x00FF0000) >> 16);
            var patch = (rawVersion & 0x0000FFFF);

            var version = $"{major:00}.{minor:00}.{patch:0000}";
            return version;
        }

        /// <summary>
        /// Initialize engine.
        /// </summary>
        public static void InitializeEngine()
        {
            if (IsInitialized)
            {
                ReferenceCount++;
                return;
            }


            var engineName = GetEngineName();
            var engineVersion = GetEngineVersion();

            Debug.Log($"[{engineName}] Version {engineVersion}");

            // Initialize MotionSyncEngine.
            var result = CubismMotionSyncEngine_CRI.InitializeEngine(IntPtr.Zero);

            if (result != 0)
            {
                Debug.Log("[CubismMotionSyncCriProcessor.InitializeEngine] Successfully initialized Engine");
                ReferenceCount++;
            }
            else
            {
                Debug.LogError("[CubismMotionSyncCriProcessor.InitializeEngine] Failed to initialize Engine.");
            }
        }

        /// <summary>
        /// Engine termination.
        /// </summary>
        public static void DisposeEngine()
        {
            if (!IsInitialized)
            {
                return;
            }


            if (ReferenceCount == 1)
            {
                CubismMotionSyncEngine_CRI.DisposeEngine();
                Debug.Log("[CubismMotionSyncCriProcessor.DisposeEngine] Successfully disposed Engine");
            }


            --ReferenceCount;
        }
    }
}
