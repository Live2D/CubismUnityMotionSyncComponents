/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI;
using Live2D.CubismMotionSyncPlugin.Samples.WebGL.AudioData;

namespace Live2D.CubismMotionSyncPlugin.Samples.WebGL
{
    /// <summary>
    /// Sample what allocate data for buffering for WebGL.
    /// </summary>
    public class CubismMotionSyncCriAudioInputWebGL : CubismMotionSyncCriAudioInput
    {

        /// <summary>
        ///  List of motionsync audio data to be used in WebGL.
        /// </summary>
        [SerializeField, HideInInspector]
        private CubismMotionSyncAudioDataList _dataList;

#if UNITY_WEBGL
        /// <summary>
        /// <see cref="AudioSource.timeSamples"/> of previous frame.
        /// </summary>
        private int _previousTimeSamples = 0;

        /// <summary>
        /// Data paired with the <see cref="AudioClip"/> currently playing.
        /// </summary>
        private CubismMotionSyncAudioData CurrentAudioData { get; set; }

        /// <summary>
        /// <see cref="AudioSource"/> to be used in WebGL.
        /// </summary>
        private AudioSource AudioSource { get; set; }

        protected override void Awake()
        {
            base.Awake();

            AudioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            // If the setting has changed, the data is destroyed.
            if (AudioBuffer == null || AudioBuffer.Length != BufferCount)
            {
                var oldLength = AudioBuffer != null ? AudioBuffer.Length : 0;
                Debug.Log($"[CubismMotionSyncCriAudioInput.OnAudioFilterRead] Extend buffer length: {oldLength.ToString()} to {BufferCount.ToString()}");
                AudioBuffer = new float[BufferCount];
                CurrentWritePosition = 0;
                CurrentReadPosition = 0;
            }

            // Fail silently...
            if (AudioBuffer.Length < 1 || !AudioSource || !AudioSource.isPlaying)
            {
                return;
            }

            SetAudioData();
        }

        /// <summary>
        /// Retrieve data from the asset and write to buffer.
        /// </summary>
        private void SetAudioData()
        {
            // If the name doesn't matches.
            if (!CurrentAudioData || AudioSource.clip.name != CurrentAudioData.name)
            {
                CurrentAudioData = _dataList.GetCubismMotionSyncAudioData(AudioSource.clip.name);
            }

            if (!CurrentAudioData)
            {
                Debug.LogError("Unable to retrieve `_currentAudioData`.");
                return;
            }

            Channels = AudioSource.clip.channels;
            var listeningChannel = Mathf.Max(0, ListeningChannel) % Channels;
            var currentTimeSamples = AudioSource.timeSamples;

            // Adjustment to prevent negative values from entering.
            if (currentTimeSamples < _previousTimeSamples)
            {
                _previousTimeSamples = 0;
            }
            var differenceTime = currentTimeSamples - _previousTimeSamples;

            // AudioSource.timeSamples is actual number of "_currentAudioData.Data.Length / AudioSource.clip.channels", so for _currentAudioData.Data prepare an index.
            var dataIndex = 0;
            for (var index = dataIndex; index < differenceTime; index++)
            {
                var previousDataSampleTime = _previousTimeSamples * Channels;

                // Abort process if index out of range.
                if (CurrentAudioData.Data.Length <= (dataIndex + previousDataSampleTime))
                {
                    break;
                }

                // If it points to a different index than the channel you want to retrieve
                if ((dataIndex + previousDataSampleTime) % Channels != listeningChannel)
                {
                    index--;
                    dataIndex++;
                    continue;
                }

                // Retrieved from data already stored in the asset.
                WriteSample(CurrentAudioData.Data[dataIndex + previousDataSampleTime]);
                dataIndex++;
            }

            // Prepare for the next frame.
            _previousTimeSamples = currentTimeSamples;
        }
#endif // UNITY_WEBGL
    }
}
