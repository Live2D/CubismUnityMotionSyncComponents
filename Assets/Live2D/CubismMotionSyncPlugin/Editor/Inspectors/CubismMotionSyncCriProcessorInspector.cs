/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */

using UnityEditor;
using Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI;

namespace Live2D.CubismMotionSyncPlugin.Editor.Inspectors
{
    /// <summary>
    /// Cubism MotionSync CRI Processor Inspector extension.
    /// </summary>
    [CustomEditor(typeof(CubismMotionSyncCriProcessor))]
    public class CubismMotionSyncCriProcessorInspector : UnityEditor.Editor
    {
        #region Editor

        /// <summary>
        /// Draws inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Fail silently.
            if (serializedObject == null)
            {
                return;
            }

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            var audioListener = serializedObject.FindProperty("_motionSyncAudioInput");
            EditorGUILayout.PropertyField(audioListener);

            // Save any changes.
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }
}
