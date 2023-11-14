/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Live2D.CubismMotionSyncPlugin.Samples.WebGL.AudioData.Editor.Inspectors
{
    /// <summary>
    /// Cubism Motion Sync Audio Data List extension.
    /// </summary>
    [CustomEditor(typeof(CubismMotionSyncAudioDataList))]
    public class CubismMotionSyncAudioDataListInspector : UnityEditor.Editor
    {
        /// <summary>
        /// Draws inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var dataList = target as CubismMotionSyncAudioDataList;

            // Fail silently...
            if (serializedObject == null || dataList == null)
            {
                return;
            }

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            var audioDataInfos = serializedObject.FindProperty("MotionSyncAudioDataInfos");
            EditorGUILayout.PropertyField(audioDataInfos);

            // Save any changes.
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            #region Add CubismMotionSyncAudioData assets Drag & Drop Area

            var area = GUILayoutUtility.GetRect(0.0f, 100.0f, GUILayout.ExpandWidth(true));
            GUI.Box(area, "Drag & Drop [Add CubismMotionSyncAudioData assets]");

            // Get event;
            var eventType = Event.current.type;

            if (!(eventType == EventType.DragPerform || eventType == EventType.DragUpdated)
                 || !area.Contains(Event.current.mousePosition))
            {
                return;
            }

            DragAndDrop.visualMode = DragAndDropVisualMode.Link;

            // Early return if not dropped.
            if (eventType != EventType.DragPerform)
            {
                return;
            }

            // Accept Drag & Drop to get object reference.
            DragAndDrop.AcceptDrag();
            var droppedObjects = DragAndDrop.objectReferences;

            // Mark event as processed.
            Event.current.Use();

            if (droppedObjects == null)
            {
                return;
            }

            var droppedAudioDataList = new List<CubismMotionSyncAudioData>();
            foreach (var item in droppedObjects)
            {
                if (item.GetType() != typeof(CubismMotionSyncAudioData))
                {
                    continue;
                }

                var audioData = (CubismMotionSyncAudioData)item;
                droppedAudioDataList.Add(audioData);
            }

            if (droppedAudioDataList.Count < 1)
            {
                return;
            }

            var oldLength = 0;
            foreach (var audioData in droppedAudioDataList)
            {
                if (dataList.MotionSyncAudioDataInfos == null
                    || !Array.Exists(dataList.MotionSyncAudioDataInfos, element => element.AudioDataInstanceId == audioData.GetInstanceID()))
                {
                    var audioDataInfo = new CubismMotionSyncAudioDataList.MotionSyncAudioDataInfo
                    {
                        AudioDataInstanceId = audioData.GetInstanceID(),
                        AudioDataObject = audioData
                    };

                    if (dataList.MotionSyncAudioDataInfos != null)
                    {
                        oldLength = dataList.MotionSyncAudioDataInfos.Length;
                    }
                    Array.Resize(ref dataList.MotionSyncAudioDataInfos, oldLength + 1);
                    dataList.MotionSyncAudioDataInfos[oldLength] = audioDataInfo;
                }
            }

            #endregion

            // Reflecting changes.
            EditorUtility.SetDirty(dataList);
        }
    }
}
