/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using UnityEditor;
using Live2D.Cubism.Framework;
using Live2D.CubismMotionSyncPlugin.Framework;

namespace Live2D.CubismMotionSyncPlugin.Editor.Inspectors
{
    /// <summary>
    /// Cubism MotionSync Controller Inspector extension.
    /// </summary>
    [CustomEditor(typeof(CubismMotionSyncController))]
    internal sealed class CubismMotionSyncControllerInspector : UnityEditor.Editor
    {
        private bool _presetDictionaryFoldout = false;
        private bool _settingsFoldout = false;
        private bool _settingsCubismParametersFoldout = false;
        private bool _settingsAudioParametersFoldout = false;
        private bool _mappingsFoldout = false;

        #region Editor

        /// <summary>
        /// Draws inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var motionSyncController = target as CubismMotionSyncController;

            // Fail silently...
            if (motionSyncController == null || motionSyncController.MotionSyncData == null)
            {
                return;
            }

            // Show MotionSyncData.
            EditorGUI.BeginChangeCheck();

            // Version.
            EditorGUILayout.LabelField("Json Version: " + motionSyncController.MotionSyncData.Version);

            _presetDictionaryFoldout = EditorGUILayout.Foldout(_presetDictionaryFoldout, "PresetDictionary (Read-only)");

            if (_presetDictionaryFoldout)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                // Make it practically ReadOnly.
                GUI.enabled = false;


                // Label.
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Id");
                    EditorGUILayout.LabelField("Name");
                }
                EditorGUILayout.EndHorizontal();

                for (int dictionaryIndex = 0; dictionaryIndex < motionSyncController.MotionSyncData.Meta.Dictionary.Length; dictionaryIndex++)
                {
                    // Element.
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        EditorGUILayout.TextField(motionSyncController.MotionSyncData.Meta.Dictionary[dictionaryIndex].Id);
                        EditorGUILayout.TextField(motionSyncController.MotionSyncData.Meta.Dictionary[dictionaryIndex].Name);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUI.enabled = true;
                EditorGUILayout.EndVertical();
            }

            #region Settings

            _settingsFoldout = EditorGUILayout.Foldout(_settingsFoldout, "Settings (Read-only)");

            if (_settingsFoldout)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                // Make it practically ReadOnly.
                GUI.enabled = false;

                // Label.
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Id");
                    EditorGUILayout.LabelField("Name");
                }
                EditorGUILayout.EndHorizontal();

                for (int settingIndex = 0; settingIndex < motionSyncController.MotionSyncData.Settings.Length; settingIndex++)
                {
                    // Make it practically ReadOnly.
                    GUI.enabled = false;
                    var setting = motionSyncController.MotionSyncData.Settings[settingIndex];

                    // Label.
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.TextField(setting.Id);
                        EditorGUILayout.TextField(setting.AnalysisType.ToString());
                        EditorGUILayout.TextField(setting.UseCase.ToString());

                        _settingsCubismParametersFoldout = EditorGUILayout.Foldout(_settingsCubismParametersFoldout, "CubismParameters (Read-only)");

                        if (_settingsCubismParametersFoldout)
                        {
                            for (int cubismParameterIndex = 0; cubismParameterIndex < setting.CubismParameters.Length; cubismParameterIndex++)
                            {
                                EditorGUILayout.BeginVertical(GUI.skin.box);
                                {
                                    // Element.
                                    // Name.
                                    EditorGUILayout.TextField("Name: ", setting.CubismParameters[cubismParameterIndex].Parameter?.GetComponent<CubismDisplayInfoParameterName>().Name);
                                    // Id.
                                    EditorGUILayout.TextField("Id: ", setting.CubismParameters[cubismParameterIndex].Parameter?.Id);
                                    // Minimum value.
                                    EditorGUILayout.FloatField("Min: ", setting.CubismParameters[cubismParameterIndex].Min);
                                    // Maximum value.
                                    EditorGUILayout.FloatField("Max: ", setting.CubismParameters[cubismParameterIndex].Max);
                                    // Damper.
                                    EditorGUILayout.FloatField("Damper: ", setting.CubismParameters[cubismParameterIndex].Damper);
                                    // Smooth.
                                    EditorGUILayout.IntField("Smooth: ", setting.CubismParameters[cubismParameterIndex].Smooth);
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }

                        _settingsAudioParametersFoldout = EditorGUILayout.Foldout(_settingsAudioParametersFoldout, "AudioParameters (Read-only)");

                        if (_settingsAudioParametersFoldout)
                        {
                            for (int audioParameterIndex = 0; audioParameterIndex < setting.AudioParameters.Length; audioParameterIndex++)
                            {
                                EditorGUILayout.BeginVertical(GUI.skin.box);
                                {
                                    // Element.
                                    // Name.
                                    EditorGUILayout.TextField("Name: ", setting.AudioParameters[audioParameterIndex].Name);
                                    // Id.
                                    EditorGUILayout.TextField("Id: ", setting.AudioParameters[audioParameterIndex].Id);
                                    // Minimum value.
                                    EditorGUILayout.FloatField("Min: ", setting.AudioParameters[audioParameterIndex].Min);
                                    // Maximum value.
                                    EditorGUILayout.FloatField("Max: ", setting.AudioParameters[audioParameterIndex].Max);
                                    // Scale.
                                    EditorGUILayout.FloatField("Scale: ", setting.AudioParameters[audioParameterIndex].Scale);
                                    // Enabled.
                                    EditorGUILayout.Toggle("Enabled: ", setting.AudioParameters[audioParameterIndex].Enabled);
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }

                        _mappingsFoldout = EditorGUILayout.Foldout(_mappingsFoldout, "Mappings (Read-only)");

                        if (_mappingsFoldout)
                        {
                            EditorGUILayout.BeginVertical(GUI.skin.box);

                            // Make it practically ReadOnly.
                            GUI.enabled = false;

                            for (int mappingIndex = 0; mappingIndex < motionSyncController.MotionSyncData.Settings[settingIndex].Mappings.Length; mappingIndex++)
                            {
                                var mapping = motionSyncController.MotionSyncData.Settings[settingIndex].Mappings[mappingIndex];

                                // Element.
                                // Type.
                                EditorGUILayout.TextField("Type: ", mapping.Type.ToString());
                                // Id.
                                EditorGUILayout.TextField("AudioParameterId: ", mapping.AudioParameterId);

                                // Targets.
                                EditorGUILayout.LabelField("Targets");
                                EditorGUILayout.BeginVertical(GUI.skin.box);
                                EditorGUI.indentLevel++;
                                for (int targetIndex = 0; targetIndex < mapping.Targets.Length; targetIndex++)
                                {
                                    // Id.
                                    EditorGUILayout.TextField("ModelParameterId: ", mapping.Targets[targetIndex].Parameter?.Id);
                                    // Value.
                                    EditorGUILayout.FloatField("Value: ", mapping.Targets[targetIndex].Value);
                                }
                                EditorGUI.indentLevel--;
                                EditorGUILayout.EndVertical();
                            }
                            GUI.enabled = true;
                            EditorGUILayout.EndVertical();

                            EditorGUILayout.Space();
                        }
                    }
                    EditorGUILayout.EndVertical();

                    GUI.enabled = true;

                    #region PostProcess

                    motionSyncController.MotionSyncData.Settings[settingIndex].PostProcessing.BlendRatio = EditorGUILayout.Slider("BlendRatio: ", setting.PostProcessing.BlendRatio, CubismMotionSyncData.BlendRatioMinValue, CubismMotionSyncData.BlendRatioMaxValue);
                    motionSyncController.MotionSyncData.Settings[settingIndex].PostProcessing.SampleRate = EditorGUILayout.Slider("SampleRate: ", setting.PostProcessing.SampleRate, CubismMotionSyncData.SampleRateMinValue, CubismMotionSyncData.SampleRateMaxValue);
                    motionSyncController.MotionSyncData.Settings[settingIndex].PostProcessing.Smoothing = EditorGUILayout.IntSlider("Smoothing: ", setting.PostProcessing.Smoothing, CubismMotionSyncData.SmoothingMinValue, CubismMotionSyncData.SmoothingMaxValue);
                    motionSyncController.MotionSyncData.Settings[settingIndex].EmphasisLevel = EditorGUILayout.Slider("EmphasisLevel: ", motionSyncController.MotionSyncData.Settings[settingIndex].EmphasisLevel, CubismMotionSyncData.EmphasisLevelMinValue, CubismMotionSyncData.EmphasisLevelMaxValue);
                    #endregion
                }
                EditorGUILayout.EndVertical();
            }

            #endregion

            // Save any changes.
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(motionSyncController);
            }
        }

        #endregion
    }
}
