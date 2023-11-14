/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */

using UnityEngine;


namespace Live2D.CubismMotionSyncPlugin.Samples.WebGL.AudioData
{
    /// <summary>
    ///  Audio data from <see cref="AudioClip"/>.
    /// </summary>
    public class CubismMotionSyncAudioData : ScriptableObject
    {
        /// <summary>
        /// Name of audio file.
        /// </summary>
        [SerializeField]
        public string AudioName;

        /// <summary>
        /// Floating decimal data for audio.
        /// </summary>
        [SerializeField]
        public float[] Data;

#if UNITY_EDITOR
        /// <summary>
        /// Create instance from <see cref="AudioClip"/>.
        /// </summary>
        /// <param name="audioClip">Source <see cref="AudioClip"/>.</param>
        /// <returns>Floating decimal data created based on <see cref="AudioClip"/>.</returns>
        public static CubismMotionSyncAudioData CreateInstance(AudioClip audioClip)
        {
            var data = new float[audioClip.samples * audioClip.channels];
            var result = audioClip.GetData(data, 0);

            if (!result)
            {
                return null;
            }

            var instance = new CubismMotionSyncAudioData
            {
                AudioName = audioClip.name,
                Data = data
            };

            return instance;
        }
#endif // UNITY_EDITOR
    }
}
