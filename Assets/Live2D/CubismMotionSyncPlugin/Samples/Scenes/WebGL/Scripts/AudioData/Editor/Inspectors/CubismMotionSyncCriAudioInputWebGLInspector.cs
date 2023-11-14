/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEditor;
using UnityEngine;
using Live2D.CubismMotionSyncPlugin.Samples.WebGL;

namespace Live2D.CubismMotionSyncPlugin.Editor.Inspectors
{
    /// <summary>
    /// Cubism MotionSync CRI Audio Input Inspector extension.
    /// </summary>
    [CustomEditor(typeof(CubismMotionSyncCriAudioInputWebGL))]
    public class CubismMotionSyncCriAudioInputWebGLInspector : UnityEditor.Editor
    {
        /// <summary>
        /// Draws inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var audioInput = target as CubismMotionSyncCriAudioInputWebGL;
            
            // Fail silently...
            if (serializedObject == null || audioInput == null)
            {
                return;
            }

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            audioInput.BufferLengthPerSecond = EditorGUILayout.FloatField("BufferLengthPerSecond", audioInput.BufferLengthPerSecond);

            if (audioInput.BufferLengthPerSecond < CubismMotionSyncCriAudioInputWebGL.MinimumBufferLengthPerSecond)
            {
                Debug.LogWarning($"Specified value is too small. The value is automatically set to {CubismMotionSyncCriAudioInputWebGL.MinimumBufferLengthPerSecond}.");
                audioInput.BufferLengthPerSecond = CubismMotionSyncCriAudioInputWebGL.MinimumBufferLengthPerSecond;
            }

            audioInput.ListeningChannel = EditorGUILayout.IntField("ListeningChannel", audioInput.ListeningChannel);
            if (audioInput.ListeningChannel < 0)
            {
                audioInput.ListeningChannel = 0;
            }

#if !UNITY_WEBGL
            EditorGUI.BeginDisabledGroup(true);
#endif
            var dataList = serializedObject.FindProperty("_dataList");
            EditorGUILayout.PropertyField(dataList, new GUIContent("Data List (WebGL only)"));
#if !UNITY_WEBGL
            EditorGUI.EndDisabledGroup();
#endif

            // Reflecting changes.
            serializedObject.ApplyModifiedProperties();

            // Save any changes.
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(audioInput);
            }
        }
    }
}
