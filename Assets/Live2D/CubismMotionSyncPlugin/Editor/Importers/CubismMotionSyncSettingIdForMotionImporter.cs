/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Editor;
using Live2D.Cubism.Editor.Importers;
using Live2D.Cubism.Framework.Json;
using Live2D.CubismMotionSyncPlugin.Framework.Motion;

namespace Live2D.CubismMotionSyncPlugin.Editor.Importers
{
    /// <summary>
    /// Retrieve information associated with motion from .model3.json.
    /// </summary>
    [Serializable]
    internal static class CubismMotionSyncSettingIdForMotionImporter
    {
        /// <summary>
        /// Motion data.
        /// </summary>
        [Serializable]
        public struct SerializableMotions
        {
            /// <summary>
            /// Motion groups.
            /// </summary>
            [SerializeField]
            public SerializableMotion[][] Motions;
        }

        /// <summary>
        /// Motions references data.
        /// </summary>
        [Serializable]
        public struct SerializableMotion
        {
            /// <summary>
            ///The motion sync setting ID.
            /// </summary>
            [SerializeField]
            public string MotionSync;

            /// <summary>
            /// Relative path to the sound file.
            /// </summary>
            [SerializeField]
            public string Sound;
        }

        /// <summary>
        /// FileReference keys from .model3.json.
        /// </summary>
        [Serializable]
        private struct SerializableFileReferences
        {
            /// <summary>
            /// Motions data.
            /// </summary>
            [SerializeField]
            public SerializableMotions Motions;
        }

        /// <summary>
        /// JSON key from .model.json.
        /// </summary>
        [Serializable]
        private struct SerializableModel3Json
        {
            /// <summary>
            /// File references data.
            /// </summary>
            [SerializeField]
            public SerializableFileReferences FileReferences;
        }

        #region Unity Event Handling

        /// <summary>
        /// Registers importer.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void RegisterImporter()
        {
            CubismImporter.OnDidImportModel += OnModelImport;
        }

        #endregion

        /// <summary>
        /// After importing model.
        /// </summary>
        /// <param name="importer">Event source.</param>
        /// <param name="model">Imported model.</param>
        private static void OnModelImport(CubismModel3JsonImporter importer, CubismModel model)
        {
            // Load Json asset.
            var modelJsonAsset = BuiltinLoadAssetAtPath(importer.AssetPath);

            if (string.IsNullOrEmpty(modelJsonAsset))
            {
                // return silently...
                return;
            }

            // Deserialize Json.
            var modelJson = JsonUtility.FromJson<SerializableModel3Json>(modelJsonAsset);
            var model3JsonDirectoryPath = Path.GetDirectoryName(importer.AssetPath)?.Replace("\\", "/");

            // Get motion file settings from .model3.json data.
            var motionGroupNames = importer.Model3Json.FileReferences.Motions.GroupNames;
            var motionsFromImporter = importer.Model3Json.FileReferences.Motions.Motions;

            if (string.IsNullOrEmpty(model3JsonDirectoryPath) || motionGroupNames == null || motionsFromImporter == null)
            {
                // return silently...
                return;
            }

            // Create a temporary list to store motionSyncLinkData
            var motionSyncLinkDataTempList = new List<CubismMotionSyncLinkData>();

            #region Create .motionSyncLink.asset files
            for (var motionGroupIndex = 0; motionGroupIndex < motionGroupNames.Length; motionGroupIndex++)
            {
                // Set motion references.
                var value = CubismJsonParser.ParseFromString(modelJsonAsset);

                // Return early if there is no references.
                if (!value.Get("FileReferences").GetMap(null).ContainsKey("Motions"))
                {
                    return;
                }

                modelJson.FileReferences.Motions.Motions = new SerializableMotion[motionGroupNames.Length][];
                var motions = modelJson.FileReferences.Motions.Motions;
                var motionGroupArrayCount = importer.Model3Json.FileReferences.Motions.Motions[motionGroupIndex].Length;

                // Get motion data from Json.
                var motionGroup = value.Get("FileReferences").Get("Motions").Get(motionGroupNames[motionGroupIndex]);
                var motionCount = motionGroup.GetVector(null).ToArray().Length;
                motions[motionGroupIndex] = new SerializableMotion[motionCount];

                for (var motionsIndex = 0; motionsIndex < motionGroupArrayCount; motionsIndex++)
                {
                    // Get sound file path and motion sync setting ID.
                    var soundRelativePath = string.IsNullOrEmpty(motionsFromImporter[motionGroupIndex][motionsIndex].Sound)
                        ? string.Empty
                        : motionsFromImporter[motionGroupIndex][motionsIndex].Sound;

                    // Get motion sync setting ID from Json.
                    if (motionGroup.Get(motionsIndex).GetMap(null).ContainsKey("MotionSync"))
                    {
                        motions[motionGroupIndex][motionsIndex].MotionSync = motionGroup.Get(motionsIndex).Get("MotionSync").toString();
                    }

                    // Set motion sync setting ID.
                    var motionSyncSettingId = string.IsNullOrEmpty(motions[motionGroupIndex][motionsIndex].MotionSync)
                        ? string.Empty
                        : motions[motionGroupIndex][motionsIndex].MotionSync;

                    // Skip create .motionSyncLink.asset file if sound file path or motion sync setting ID is empty.
                    if (string.IsNullOrEmpty(soundRelativePath) || string.IsNullOrEmpty(motionSyncSettingId))
                    {
                        motionSyncLinkDataTempList.Add(null);
                        continue;
                    }

                    // Get AudioClip asset from sound file path.
                    var soundFilePath = string.Format("{0}/{1}", model3JsonDirectoryPath, soundRelativePath);
                    var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(soundFilePath);

                    if (audioClip == null)
                    {
                        Debug.LogWarning("AudioClip not found at path: " + soundFilePath);
                    }

                    var shouldImportAsOriginalWorkflow = CubismUnityEditorMenu.ShouldImportAsOriginalWorkflow;
                    var shouldClearAnimationCurves = CubismUnityEditorMenu.ShouldClearAnimationCurves;

                    var motionPath = string.Format("{0}/{1}", model3JsonDirectoryPath, motionsFromImporter[motionGroupIndex][motionsIndex].File);
                    var jsonString = string.IsNullOrEmpty(motionPath)
                                        ? null
                                        : File.ReadAllText(motionPath);

                    if (jsonString == null)
                    {
                        continue;
                    }

                    var motion3Json = CubismMotion3Json.LoadFrom(jsonString);

                    var animationClipPath = string.Format("{0}/{1}", model3JsonDirectoryPath, motionsFromImporter[motionGroupIndex][motionsIndex].File.Replace(".motion3.json", ".anim"));
                    animationClipPath = animationClipPath.Replace("\\", "/");

                    var animationName = Path.GetFileNameWithoutExtension(motionsFromImporter[motionGroupIndex][motionsIndex].File.Replace(".motion3.json", ".anim"));
                    var assetList = CubismCreatedAssetList.GetInstance();
                    var assetListIndex = assetList.AssetPaths.Contains(animationClipPath)
                        ? assetList.AssetPaths.IndexOf(animationClipPath)
                        : -1;

                    var oldAnimationClip = (shouldImportAsOriginalWorkflow)
                        ? (assetListIndex >= 0)
                            ? (AnimationClip)assetList.Assets[assetListIndex]
                            : AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath)
                        : null;

                    var newAnimationClip = (oldAnimationClip == null)
                        ? motion3Json.ToAnimationClip(shouldImportAsOriginalWorkflow, shouldClearAnimationCurves)
                        : motion3Json.ToAnimationClip(oldAnimationClip, shouldImportAsOriginalWorkflow, shouldClearAnimationCurves);
                    newAnimationClip.name = animationName;

                    if (assetListIndex < 0)
                    {
                        // Create animation clip.
                        if (oldAnimationClip == null)
                        {
                            AssetDatabase.CreateAsset(newAnimationClip, animationClipPath);
                        }

                        assetList.Assets.Add(newAnimationClip);
                        assetList.AssetPaths.Add(animationClipPath);
                        assetList.IsImporterDirties.Add(false);
                    }
                    // Update animation clip.
                    else
                    {
                        EditorUtility.CopySerialized(newAnimationClip, oldAnimationClip);
                        EditorUtility.SetDirty(oldAnimationClip);
                        assetList.Assets[assetListIndex] = oldAnimationClip;
                    }

                    var motionInstanceId = -1;
                    // Add animation event
                    {
                        motionInstanceId = newAnimationClip.GetInstanceID();

                        var sourceAnimationEvents = AnimationUtility.GetAnimationEvents(newAnimationClip);
                        var index = -1;

                        for (var sourceAnimationEventIndex = 0; sourceAnimationEventIndex < sourceAnimationEvents.Length; ++sourceAnimationEventIndex)
                        {
                            if (sourceAnimationEvents[sourceAnimationEventIndex].functionName != "InstanceId")
                            {
                                continue;
                            }

                            index = sourceAnimationEventIndex;
                            break;
                        }

                        if (index == -1)
                        {
                            index = sourceAnimationEvents.Length;
                            Array.Resize(ref sourceAnimationEvents, sourceAnimationEvents.Length + 1);
                            sourceAnimationEvents[sourceAnimationEvents.Length - 1] = new AnimationEvent();
                        }

                        sourceAnimationEvents[index].time = 0;
                        sourceAnimationEvents[index].functionName = "InstanceId";
                        sourceAnimationEvents[index].intParameter = motionInstanceId;
                        sourceAnimationEvents[index].messageOptions = SendMessageOptions.DontRequireReceiver;

                        AnimationUtility.SetAnimationEvents(newAnimationClip, sourceAnimationEvents);
                    }

                    // Create .motionSyncLink.asset file.
                    var motionSyncLinkData = CubismMotionSyncLinkData.CreateInstance(motionSyncSettingId, audioClip, motionsFromImporter[motionGroupIndex][motionsIndex].File, motionInstanceId);

                    // Regist motion sync link data.
                    motionSyncLinkDataTempList.Add(motionSyncLinkData);

                    var motionSyncLinkDataPath = motionPath.Replace(".motion3.json", ".motionSyncLink.asset");
                    if (!motionSyncLinkData)
                    {
                        Debug.LogError($"Failed to create {motionSyncLinkDataPath}.");
                        continue;
                    }

                    AssetDatabase.CreateAsset(motionSyncLinkData, motionSyncLinkDataPath);

                    // Reflecting changes.
                    EditorUtility.SetDirty(motionSyncLinkData);
                    AssetDatabase.Refresh();
                }
            }
            #endregion

            // Create .motionSyncLinkList.asset.
            var motionSyncLinkList = ScriptableObject.CreateInstance<CubismMotionSyncLinkList>();

            if (!motionSyncLinkList)
            {
                Debug.LogError("Failed to create CubismMotionSyncLinkList.");
                return;
            }

            motionSyncLinkList.CubismMotionSyncLinkObjects = new CubismMotionSyncLinkData[motionSyncLinkDataTempList.Count];

            // Copy motion sync link data.
            Array.Copy(motionSyncLinkDataTempList.ToArray(), motionSyncLinkList.CubismMotionSyncLinkObjects, motionSyncLinkDataTempList.Count);

            AssetDatabase.CreateAsset(motionSyncLinkList, string.Format("{0}/{1}", model3JsonDirectoryPath, model.name + ".motionSyncLinkList.asset"));

            // Reflecting changes.
            EditorUtility.SetDirty(motionSyncLinkList);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Builtin method for loading assets.
        /// </summary>
        /// <param name="assetPath">Path to asset.</param>
        /// <returns>The asset on success; <see langword="null"/> otherwise.</returns>
        private static string BuiltinLoadAssetAtPath(string assetPath)
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath).ToString();
#else
            var textAsset = Resources.Load(assetPath, typeof(TextAsset)) as TextAsset;

            return (textAsset != null)
                ? textAsset.text
                : null;
#endif
        }
    }
}
