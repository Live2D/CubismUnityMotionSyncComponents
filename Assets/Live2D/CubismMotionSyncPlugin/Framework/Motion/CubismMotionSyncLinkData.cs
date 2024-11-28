/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using UnityEditor;

namespace Live2D.CubismMotionSyncPlugin.Framework.Motion
{
    /// <summary>
    /// Scriptable object that stores motion sync related data associated with a motion.
    /// </summary>
    public class CubismMotionSyncLinkData : ScriptableObject
    {
        /// <summary>
        /// Motion file path.
        /// </summary>
        [SerializeField]
        public string MotionPath;

        /// <summary>
        /// Instance ID obtained from the motion's event.
        /// </summary>
        [SerializeField]
        public int MotionInstanceId;

        /// <summary>
        /// Motion sync setting ID associated with the motion.
        /// </summary>
        [SerializeField]
        public string MotionSyncSettingId;

        /// <summary>
        /// AudioClip associated with the motion.
        /// </summary>
        [SerializeField]
        public AudioClip audioClip;

#if UNITY_EDITOR

        /// <summary>
        /// Create asset data.
        /// </summary>
        /// <param name="motionSyncSettingId">MotionSync setting ID strings.</param>
        /// <param name="audioClip">Audio files linked to the motion.</param>
        /// <param name="motionFilePath">Motion  files linked to the audio.</param>
        /// <param name="motionInstanceId">AnimationClip instance Ids what creates from motion.</param>
        /// <returns>Instance.</returns>
        public static CubismMotionSyncLinkData CreateInstance(string motionSyncSettingId, AudioClip audioClip, string motionFilePath, int motionInstanceId)
        {
            var motionSyncLinkData = AssetDatabase.LoadAssetAtPath<CubismMotionSyncLinkData>(motionFilePath.Replace(".motion3.json", ".motionSyncLink.asset"));

            // if not found, create new instance
            if (motionSyncLinkData == null)
            {
                motionSyncLinkData = CreateInstance<CubismMotionSyncLinkData>();
            }

            // Update information other than MotionInstanceId.
            motionSyncLinkData.audioClip = audioClip;
            motionSyncLinkData.MotionPath = motionFilePath;
            motionSyncLinkData.MotionSyncSettingId = motionSyncSettingId;
            motionSyncLinkData.MotionInstanceId = motionInstanceId;

            return motionSyncLinkData;
        }

#endif // UNITY_EDITOR
    }
}
