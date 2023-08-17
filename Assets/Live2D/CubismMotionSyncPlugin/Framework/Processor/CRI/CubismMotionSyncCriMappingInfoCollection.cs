/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Live2D.Cubism.Core.Unmanaged;
using Live2D.CubismMotionSyncPlugin.Plugins;



namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    public class CubismMotionSyncCriMappingInfoCollection
    {
        /// <summary>
        /// Unmanaged address to the MappingInfo array.
        /// </summary>
        public IntPtr Address { get; private set; }

        /// <summary>
        /// Length of MappingInfo array.
        /// </summary>
        public int Length => MappingInfoArray != null ? MappingInfoArray.Length : 0;


        private CubismMotionSyncEngine_CRI.csmMotionSync_MappingInfo[] MappingInfoArray { get; set; }

        /// <summary>
        /// Create Array of <see cref="CubismMotionSyncEngine_CRI.csmMotionSync_MappingInfo"/>.
        /// </summary>
        public unsafe void Create(CubismMotionSyncData.SerializableSetting setting)
        {
            var serializableMappings = setting.Mappings;

            MappingInfoArray = new CubismMotionSyncEngine_CRI.csmMotionSync_MappingInfo[serializableMappings.Length];


            var listSize = MappingInfoArray.Length * Marshal.SizeOf(typeof(CubismMotionSyncEngine_CRI.csmMotionSync_MappingInfo));

            // Match json encoding.
            var encoding = Encoding.GetEncoding("UTF-8");


            for (var i = 0; i < MappingInfoArray.Length; i++)
            {
                MappingInfoArray[i] = new CubismMotionSyncEngine_CRI.csmMotionSync_MappingInfo();


                var audioParameterIdPtr = Marshal.StringToHGlobalAnsi(serializableMappings[i].AudioParameterId);
                MappingInfoArray[i].AudioParameterId = (char*)audioParameterIdPtr;


                var targetsIdSize = 0;
                for (var targetIndex = 0; targetIndex < serializableMappings[i].Targets.Length; targetIndex++)
                {
                    targetsIdSize += encoding.GetByteCount(serializableMappings[i].Targets[targetIndex].Parameter.Id);
                }


                MappingInfoArray[i].ModelParameterIds = (char**)CubismUnmanagedMemory.Allocate(targetsIdSize, CubismMotionSyncCriProcessor.DefaultAlign);
                for (var targetIndex = 0; targetIndex < serializableMappings[i].Targets.Length; targetIndex++)
                {
                    var cubismParametersPtr = Marshal.StringToHGlobalAnsi(serializableMappings[i].Targets[targetIndex].Parameter.Id);
                    MappingInfoArray[i].ModelParameterIds[targetIndex] = (char*)cubismParametersPtr;
                }


                var targetsValueSize = Marshal.SizeOf(typeof(float)) * serializableMappings[i].Targets.Length;
                MappingInfoArray[i].ModelParameterValues = (float*)CubismUnmanagedMemory.Allocate(targetsValueSize, CubismMotionSyncCriProcessor.DefaultAlign);
                for (var j = 0; j < serializableMappings[i].Targets.Length; j++)
                {
                    MappingInfoArray[i].ModelParameterValues[j] = serializableMappings[i].Targets[j].Value;
                }


                MappingInfoArray[i].ModelParameterCount = (uint)serializableMappings[i].Targets.Length;
                MappingInfoArray[i].Scale = setting.AudioParameters[i].Scale;
                MappingInfoArray[i].Enabled = Convert.ToInt32(setting.AudioParameters[i].Enabled);
            }


            var listPtr = CubismUnmanagedMemory.Allocate(listSize, CubismMotionSyncCriProcessor.DefaultAlign);
            var firstPtr = listPtr;


            for (var i = 0; i < MappingInfoArray.Length; i++)
            {
                // Copy data to allocated memory.
                Marshal.StructureToPtr(MappingInfoArray[i], listPtr, false);
                listPtr = (IntPtr)((long)listPtr + Marshal.SizeOf(MappingInfoArray[i]));
            }

            // Holds the first pointer to the array.
            Address = firstPtr;
        }

        /// <summary>
        /// Cleans up the resources.
        /// </summary>
        public unsafe void Delete()
        {
            // MappingInfoArray.
            for (var i = 0; i < MappingInfoArray.Length; i++)
            {
                CubismUnmanagedMemory.Deallocate((IntPtr)MappingInfoArray[i].AudioParameterId);
                CubismUnmanagedMemory.Deallocate((IntPtr)MappingInfoArray[i].ModelParameterIds);
                CubismUnmanagedMemory.Deallocate((IntPtr)MappingInfoArray[i].ModelParameterValues);


                // Cleaning up.
                MappingInfoArray[i].AudioParameterId = null;
                MappingInfoArray[i].ModelParameterIds = null;
                MappingInfoArray[i].ModelParameterValues = null;
            }


            CubismUnmanagedMemory.Deallocate(Address);


            // Cleaning up.
            Address = IntPtr.Zero;
            MappingInfoArray = null;
        }

    }
}
