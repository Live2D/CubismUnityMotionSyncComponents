/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.Runtime.InteropServices;

namespace Live2D.CubismMotionSyncPlugin.Plugins
{
    public static class CubismMotionSyncEngine_CRI
    {
        /// <summary>
        /// Name of native DLL file.
        /// </summary>
#if (UNITY_IOS || UNITY_WEBGL || UNITY_SWITCH) && !UNITY_EDITOR
        public const string DllName = "__Internal";
#else
        public const string DllName = "Live2DCubismMotionSyncEngine_CRI";
#endif

        /// <summary>
        /// Decralation of allocator.
        /// </summary>
        /// <param name="size">Allocate size.</param>
        /// <returns>Address.</returns>
        public delegate IntPtr csmMotionSync_AllocFunc(uint size);

        /// <summary>
        /// Decralation of aligned allocator.
        /// </summary>
        /// <param name="size">Size of Allocate.</param>
        /// <param name="align">Size of alignment.</param>
        /// <returns>Address.</returns>
        public delegate IntPtr csmMotionSync_AlignedAllocFunc(uint size, uint align);

        /// <summary>
        /// Decralation of deallocator.
        /// </summary>
        /// <param name="ptr">Address.</param>
        public delegate void csmMotionSync_DeallocFunc(IntPtr ptr);

        /// <summary>
        /// Decralation of aligned deallocator.
        /// </summary>
        /// <param name="ptr">Address.</param>
        public delegate void csmMotionSync_AlignedDeallocFunc(IntPtr ptr);

        /// <summary>
        /// LogHandler
        /// </summary>
        /// <param name="message">Null-terminated string message to log.</param>
        public delegate void csmMotionSyncLogFunction(string message);

        /// <summary>
        /// Engine configuration for CRI.
        /// </summary>
        public struct csmMotionSync_EngineConfig_CRI
        {
            /// <summary>
            /// Pointer to Allocator.
            /// </summary>
            public csmMotionSync_AllocFunc Allocator;

            /// <summary>
            /// Pointer to Deallocator.
            /// </summary>
            public csmMotionSync_DeallocFunc Deallocator;
        }

        /// <summary>
        /// Context cofiguration for CRI.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct csmMotionSync_ContextConfig_CRI
        {
            /// <summary>
            /// Audio sample rate.
            /// </summary>
            public int SampleRato;

            /// <summary>
            /// Audio bit depth.
            /// </summary>
            public int BitDepth;
        }

        /// <summary>
        /// Context cofiguration for CRI.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct csmMotionSync_AnalysisConfig_CRI
        {
            /// <summary>
            /// Blending ratio for vieseme.
            /// </summary>
            public float BlendRatio;

            /// <summary>
            /// Smoothing value.
            /// </summary>
            public int Smoothing;

            /// <summary>
            /// Audio level effect ratio.
            /// </summary>
            public float AudioLevelEffectRatio;
        }

        /// <summary>
        /// Result of analysis.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct csmMotionSync_AnalysisResult
        {
            /// <summary>
            /// Value Array of CubismParameter.
            /// </summary>
            public unsafe float* Values;

            /// <summary>
            /// Number of values used in the analysis.
            /// </summary>
            public int ValuesCount;

            /// <summary>
            /// Number of samples used in the analysis.
            /// </summary>
            public uint ProcessedSampleCount;
        }

        /// <summary>
        /// Mapping Information of AudioParameter.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        unsafe public struct csmMotionSync_MappingInfo
        {
            /// <summary>
            /// ID of AudioParameter.
            /// </summary>
            public char* AudioParameterId;

            /// <summary>
            /// Array of ID of CubismParameter linked to the AudioParameter.
            /// </summary>
            public char** ModelParameterIds;

            /// <summary>
            /// Array of CubismParameter Value.
            /// </summary>
            public float* ModelParameterValues;

            /// <summary>
            /// Number of CubismParameter.
            /// </summary>
            public uint ModelParameterCount;

            /// <summary>
            /// Scaling value for CubismParameter.
            /// </summary>
            public float Scale;

            /// <summary>
            /// Enable / Disable for applying.
            /// `0` is False. `1` is True.
            /// </summary>
            public int Enabled;
        }

        /// <summary>
        /// Queries Engine version.
        /// </summary>
        [DllImport(DllName, EntryPoint = "csmMotionSync_GetEngineVersion")]
        public static extern uint GetEngineVersion();

        /// <summary>
        /// Queries the name of Engine.
        /// </summary>
        /// <returns>Engine Name.</returns>
        [DllImport(DllName, EntryPoint = "csmMotionSync_GetEngineName")]
        public static extern unsafe char* GetEngineName();

        /// <summary>
        /// Sets log handler.
        /// </summary>
        [DllImport(DllName, EntryPoint = "csmMotionSync_SetLogFunction")]
        public static extern void SetLogFunction(csmMotionSyncLogFunction handler);

        /// <summary>
        /// Initializes the Engine.
        /// </summary>
        /// <param name="engineConfiguration">Congifuration parameter for engine initialization. Set to `IntPtr.Zero` to use default parameter.</param>
        /// <returns>`1` if Engine is available.</returns>
        [DllImport(DllName, EntryPoint = "csmMotionSync_InitializeEngine")]
        public static extern int InitializeEngine(IntPtr engineConfiguration);

        /// <summary>
        /// Disposes the Engine.
        /// </summary>
        [DllImport(DllName, EntryPoint = "csmMotionSync_DisposeEngine")]
        public static extern void DisposeEngine();

        /// <summary>
        /// Makes the context of Analysis.
        /// </summary>
        /// <param name="contextConfiguration">Congifuration parameter for engine initialization. Set to `IntPtr.Zero` to use default parameter.</param>
        /// <param name="mappingInformations">First address of Array of of Mapping Information of AudioParameter.</param>
        /// <param name="mappingInformationCount">Length of `mappingInformations`.</param>
        /// <returns></returns>
        [DllImport(DllName, EntryPoint = "csmMotionSync_CreateContext")]
        public static extern IntPtr CreateContext(IntPtr contextConfiguration, IntPtr mappingInformations, int mappingInformationCount);

        /// <summary>
        /// Destroys the context of Analysis.
        /// </summary>
        /// <param name="context">@param context Context to destroy.</param>
        [DllImport(DllName, EntryPoint = "csmMotionSync_DeleteContext")]
        public static extern void DeleteContext(IntPtr context);

        [DllImport(DllName, EntryPoint = "csmMotionSync_GetRequireSampleCount")]
        public static extern uint GetRequireSampleCount(IntPtr context);

        [DllImport(DllName, EntryPoint = "csmMotionSync_Analyze")]
        unsafe public static extern int Analyze(IntPtr context, float* samples, uint sampleCount, IntPtr analysisResult, IntPtr engineConfiguration);
    }
}
