/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Live2D.Cubism.Core;
using Live2D.Cubism.Editor.Importers;
using Live2D.CubismMotionSyncPlugin.Framework;
using Live2D.CubismMotionSyncPlugin.Framework.Json;

namespace Live2D.CubismMotionSyncPlugin.Editor.Importers
{
    /// <summary>
    /// Handles importing of .motionSync3.json.
    /// </summary>
    [Serializable]
    internal static class CubismMotionSync3JsonImporter
    {
        /// <summary>
        /// File references data.
        /// </summary>
        [Serializable]
        public struct SerializableFileReferences
        {
            /// <summary>
            /// Relative path to the motionsync3.json.
            /// </summary>
            [SerializeField]
            public string MotionSync;
        }

        [Serializable]
        private struct SerializableMotionSync
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
        /// Set expression list.
        /// </summary>
        /// <param name="importer">Event source.</param>
        /// <param name="model">Imported model.</param>
        private static void OnModelImport(CubismModel3JsonImporter importer, CubismModel model)
        {
            // Load Json asset.
            var modelJsonAsset = BuiltinLoadAssetAtPath(importer.AssetPath);
            // Deserialize Json.
            var modelJson = JsonUtility.FromJson<SerializableMotionSync>(modelJsonAsset);
            var filename = modelJson.FileReferences.MotionSync;

            // Return early if there is no references.
            if (string.IsNullOrEmpty(filename))
            {
                // Fail silently...
                return;
            }

            var modelDir = Path.GetDirectoryName(importer.AssetPath);
            var motionSync3JsonPath = Path.Combine(modelDir, filename);

            var motionSync3Json = CubismMotionSync3Json.LoadFrom(AssetDatabase.LoadAssetAtPath<TextAsset>(motionSync3JsonPath));

            var motionSyncController = model.GetComponent<CubismMotionSyncController>();
            if (motionSyncController == null)
            {
                motionSyncController = model.gameObject.AddComponent<CubismMotionSyncController>();
            }

            // Create motion sync data.
            var motionSyncData = CubismMotionSyncData.CreateInstance(motionSync3Json, model);
            motionSyncController.MotionSyncData = motionSyncData;
            motionSyncController.CreateProcessors();
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
