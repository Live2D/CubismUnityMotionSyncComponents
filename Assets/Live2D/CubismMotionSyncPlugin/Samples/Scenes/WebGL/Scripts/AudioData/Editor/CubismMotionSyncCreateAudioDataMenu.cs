/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Live2D.CubismMotionSyncPlugin.Samples.WebGL.AudioData.Editor
{
    /// <summary>
    /// Create <see cref="CubismMotionSyncAudioData"/> on unity editor menu.
    /// </summary>
    public class CubismMotionSyncCreateAudioDataMenu : EditorWindow
    {
#if UNITY_WEBGL
        /// <summary>
        /// Unity editor menu what create <see cref="CubismMotionSyncAudioData"/> asset from <see cref="AudioClip"/> asset.
        /// </summary>
        [MenuItem("Assets/Create/Live2D Cubism/MotionSync/Create Audio Data From AudioClip")]
        public static void CreateAudioData()
        {
            var activeObject = Selection.activeObject;
            var audioClip = (AudioClip)activeObject;

            var audioFilePath = AssetDatabase.GetAssetPath(activeObject.GetInstanceID());
            var directoryPath = Path.GetDirectoryName(audioFilePath) + "/";

            // Create asset.
            var audioData = CubismMotionSyncAudioData.CreateInstance(audioClip);

            if (!audioData)
            {
                Debug.LogError($"Failed to create {directoryPath + audioClip.name}.asset.");
                return;
            }

            var path = directoryPath + audioData.AudioName + ".asset";
            AssetDatabase.CreateAsset(audioData, path);

            // Reflecting changes.
            EditorUtility.SetDirty(audioData);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Check for conditions under which <see cref="CubismMotionSyncAudioData"/> can be created.
        /// </summary>
        [MenuItem("Assets/Create/Live2D Cubism/MotionSync/Create Audio Data From AudioClip", true)]
        public static bool CanCreateAudioData()
        {
            // Enable only when AudioClip is selected.
            return Selection.activeObject?.GetType() == typeof(AudioClip);
        }
#endif // UNITY_WEBGL
    }
}
