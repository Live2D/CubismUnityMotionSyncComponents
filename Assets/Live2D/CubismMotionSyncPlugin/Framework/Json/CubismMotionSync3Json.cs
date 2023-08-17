/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using System;
using UnityEngine;
using Live2D.Cubism.Core;

namespace Live2D.CubismMotionSyncPlugin.Framework.Json
{
    /// <summary>
    /// Handles the motionSync3.json.
    /// </summary>
    [Serializable]
    public sealed class CubismMotionSync3Json
    {
        /// <summary>
        /// Loads a motionSync3.json asset.
        /// </summary>
        /// <param name="motionSync3Json">motionSync3.json to deserialize.</param>
        /// <returns>Deserialized physics3.json on success; <see langword="null"/> otherwise.</returns>
        public static CubismMotionSync3Json LoadFrom(string motionSync3Json)
        {
            return string.IsNullOrEmpty(motionSync3Json)
                ? null
                : JsonUtility.FromJson<CubismMotionSync3Json>(motionSync3Json);
        }

        /// <summary>
        /// Loads a motionSync3.json asset.
        /// </summary>
        /// <param name="motionSync3JsonAsset">motionSync3.json to deserialize.</param>
        /// <returns>Deserialized physics3.json on success; <see langword="null"/> otherwise.</returns>
        public static CubismMotionSync3Json LoadFrom(TextAsset motionSync3JsonAsset)
        {
            return motionSync3JsonAsset == null
                ? null
                : LoadFrom(motionSync3JsonAsset.text);
        }

        #region Json Data

        /// <summary>
        /// Json file format version.
        /// </summary>
        [SerializeField]
        public int Version;

        /// <summary>
        /// Meta.
        /// </summary>
        [SerializeField]
        public SerializableMeta Meta;

        /// <summary>
        /// Info about motion-sync settings.
        /// </summary>
        [SerializeField]
        public SerializableSetting[] Settings;

        #endregion

        #region JsonHelpers

        /// <summary>
        /// Settings name and id template.
        /// </summary>
        [Serializable]
        public struct SerializableDictionary
        {
            /// <summary>
            /// Id about <see cref="SerializableSetting"/>
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Name about <see cref="SerializableSetting"/>
            /// </summary>
            [SerializeField]
            public string Name;
        }

        /// <summary>
        /// Meta.
        /// </summary>
        [Serializable]
        public struct SerializableMeta
        {
            /// <summary>
            /// Number about <see cref="SerializableSetting"/>`.
            /// </summary>
            [SerializeField]
            public int SettingCount;

            /// <summary>
            /// Id-Name pairs in `Settings` array.
            /// </summary>
            [SerializeField]
            public SerializableDictionary[] Dictionary;
        }

        /// <summary>
        /// Configuration for motion sync in <see cref="CubismParameter"/>.
        /// </summary>
        [Serializable]
        public struct SerializableCubismParameter
        {
            /// <summary>
            /// <see cref="CubismParameter"/> Name.
            /// </summary>
            [SerializeField]
            public string Name;

            /// <summary>
            /// <see cref="CubismParameter"/> Id.
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Parameter minimum value.
            /// </summary>
            [SerializeField]
            public float Min;

            /// <summary>
            /// Parameter maximum value.
            /// </summary>
            [SerializeField]
            public float Max;

            /// <summary>
            /// Damping Value.
            /// </summary>
            [SerializeField]
            public float Damper;

            /// <summary>
            /// Smoothing Value.
            /// </summary>
            [SerializeField]
            public int Smooth;
        }

        /// <summary>
        /// Configuration for motion sync in audio parameter.
        /// </summary>
        [Serializable]
        public struct SerializableAudioParameter
        {
            // <summary>
            /// Name about audio parameters.
            /// NOTE: It is a string that can be used for any audio sync library.
            /// </summary>
            [SerializeField]
            public string Name;

            /// <summary>
            /// Id about audio parameters.
            /// NOTE: It is a string that can be used for any audio sync library.
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Parameter minimum value.
            /// </summary>
            [SerializeField]
            public float Min;

            /// <summary>
            /// Parameter maximum value.
            /// </summary>
            [SerializeField]
            public float Max;

            /// <summary>
            /// Scaling Value.
            /// </summary>
            [SerializeField]
            public float Scale;

            /// <summary>
            /// Synchronization enabled state.
            /// </summary>
            [SerializeField]
            public bool Enabled;
        }

        /// <summary>
        /// Template for associating Id and Value.
        /// </summary>
        [Serializable]
        public struct SerializableTarget
        {
            /// <summary>
            /// <see cref="CubismParameter"/> Id.
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// <see cref="CubismParameter"/> value.
            /// </summary>
            [SerializeField]
            public float Value;
        }

        /// <summary>
        /// Associating audio parameters to cubism parameters.
        /// </summary>
        [Serializable]
        public struct SerializableMapping
        {
            /// <summary>
            /// Mapping type.
            /// </summary>
            [SerializeField]
            public string Type;

            /// <summary>
            /// Id about audio parameter.
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Array of mapping parameter for converting audio parameter to cubism parameter.
            /// </summary>
            [SerializeField]
            public SerializableTarget[] Targets;
        }

        /// <summary>
        /// Info about motion-sync settings.
        /// </summary>
        [Serializable]
        public struct SerializableSetting
        {
            /// <summary>
            /// Id about setting.
            /// </summary>
            [SerializeField]
            public string Id;

            /// <summary>
            /// Types about audio analysis.
            /// </summary>
            [SerializeField]
            public string AnalysisType;

            /// <summary>
            /// Types about use case.
            /// </summary>
            [SerializeField]
            public string UseCase;

            /// <summary>
            /// Data about cubism parameters for motion sync.
            /// </summary>
            [SerializeField]
            public SerializableCubismParameter[] CubismParameters;

            /// <summary>
            /// Data about audio parameters for motion sync.
            /// </summary>
            [SerializeField]
            public SerializableAudioParameter[] AudioParameters;

            /// <summary>
            /// Data about mapping.
            /// </summary>
            [SerializeField]
            public SerializableMapping[] Mappings;

            /// <summary>
            /// Post-processing information.
            /// </summary>
            [SerializeField]
            public SerializablePostProcessing PostProcessing;
        }

        /// <summary>
        /// Post-processing information.
        /// </summary>
        [Serializable]
        public struct SerializablePostProcessing
        {
            /// <summary>
            /// Ratio about motion sync blending.
            /// </summary>
            [SerializeField]
            public float BlendRatio;

            /// <summary>
            /// Value about motion sync smoothing.
            /// </summary>
            [SerializeField]
            public int Smoothing;

            /// <summary>
            /// Settings about motion sync update speed.
            /// </summary>
            [SerializeField]
            public float SampleRate;
        }

        #endregion
    }
}
