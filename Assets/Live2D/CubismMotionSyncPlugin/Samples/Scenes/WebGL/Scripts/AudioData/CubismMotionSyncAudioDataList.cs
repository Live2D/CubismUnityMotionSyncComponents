/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */

using System;
using UnityEngine;


namespace Live2D.CubismMotionSyncPlugin.Samples.WebGL.AudioData
{
    [CreateAssetMenu(menuName = "Live2D Cubism/MotionSync/Audio Data List")]
    public class CubismMotionSyncAudioDataList : ScriptableObject
    {
        /// <summary>
        /// Information of <see cref="CubismMotionSyncAudioData"/> asset.
        /// </summary>
        [Serializable]
        public struct MotionSyncAudioDataInfo
        {
            /// <summary>
            /// <see cref="CubismMotionSyncAudioData"/> asset instance id.
            /// </summary>
            [SerializeField]
            public int AudioDataInstanceId;

            /// <summary>
            /// Reference to <see cref="CubismMotionSyncAudioData"/> asset object.
            /// </summary>
            [SerializeField]
            public CubismMotionSyncAudioData AudioDataObject;
        }

        /// <summary>
        /// Array of <see cref="MotionSyncAudioDataInfo"/>.
        /// </summary>
        [SerializeField]
        public MotionSyncAudioDataInfo[] MotionSyncAudioDataInfos;

        /// <summary>
        /// Get <see cref="CubismMotionSyncAudioData"/> instance <see cref="MotionSyncAudioDataInfos"/> used instance id.
        /// </summary>
        /// <param name="audioDataInstanceId">Instance id used for search.</param>
        /// <returns>Audio data instance.</returns>
        public CubismMotionSyncAudioData GetCubismMotionSyncAudioData(int audioDataInstanceId)
        {
            for (var i = 0; i < MotionSyncAudioDataInfos.Length; i++)
            {
                // If the id matches.
                if (MotionSyncAudioDataInfos[i].AudioDataInstanceId == audioDataInstanceId)
                {
                    return MotionSyncAudioDataInfos[i].AudioDataObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Get <see cref="CubismMotionSyncAudioData"/> instance <see cref="MotionSyncAudioDataInfos"/> used name.
        /// </summary>
        /// <param name="name">Name used for search.</param>
        /// <returns>Audio data instance.</returns>
        public CubismMotionSyncAudioData GetCubismMotionSyncAudioData(string name)
        {
            for (var i = 0; i < MotionSyncAudioDataInfos.Length; i++)
            {
                // If the name matches.
                if (MotionSyncAudioDataInfos[i].AudioDataObject.AudioName == name)
                {
                    return MotionSyncAudioDataInfos[i].AudioDataObject;
                }
            }

            return null;
        }
    }
}
