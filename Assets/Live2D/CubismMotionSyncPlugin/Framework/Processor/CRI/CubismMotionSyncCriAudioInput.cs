/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;

namespace Live2D.CubismMotionSyncPlugin.Framework.Processor.CRI
{
    /// <summary>
    /// Allocate data for buffering.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class CubismMotionSyncCriAudioInput : MonoBehaviour
    {
        /// <summary>
        /// Minimum AudioBuffer length per second.
        /// </summary>
        private const float MinimumBufferLengthPerSecond = 1.0f;

        /// <summary>
        /// AudioBuffer length per second.
        /// </summary>
        [SerializeField]
        public float BufferLengthPerSecond = MinimumBufferLengthPerSecond;

        /// <summary>
        /// Channel of the audio used for analysis.
        /// </summary>
        [SerializeField]
        public int ListeningChannel = 0;

        /// <summary>
        /// Buffer.
        /// </summary>
        public float[] AudioBuffer { get; private set; }

        /// <summary>
        /// Written Position.
        /// </summary>
        public long CurrentWritePosition { get; private set; }

        /// <summary>
        /// Loaded positions.
        /// </summary>
        public long CurrentReadPosition { get; private set; }

        /// <summary>
        /// Audio frequency.
        /// </summary>
        public int Frequency { get; private set; }

        /// <summary>
        /// Number of Audio channel.
        /// </summary>
        private int Channels { get; set; }

        /// <summary>
        /// Count of Buffer.
        /// </summary>
        private int BufferCount
        {
            get
            {
                return (int)(Frequency * Mathf.Max(MinimumBufferLengthPerSecond, BufferLengthPerSecond));
            }
        }

        private void Awake()
        {
            Frequency = AudioSettings.GetConfiguration().sampleRate;


            AudioBuffer = new float[BufferCount];
        }

        /// <summary>
        /// Raises the audio filter read event.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="channels">Channels.</param>
        private void OnAudioFilterRead(float[] data, int channels)
        {
            Channels = channels;


            var listeningChannel = Mathf.Max(0, ListeningChannel) % Channels;

            // If the setting has changed, the data is destroyed.
            if (AudioBuffer == null || AudioBuffer.Length != BufferCount)
            {
                var oldLength = AudioBuffer != null ? AudioBuffer.Length : 0;
                Debug.Log($"[CubismMotionSyncCriAudioInput.OnAudioFilterRead] Extend buffer length: {oldLength.ToString()} to {BufferCount.ToString()}");
                AudioBuffer = new float[BufferCount];
                CurrentWritePosition = 0;
                CurrentReadPosition = 0;
            }

            if (AudioBuffer.Length < 1)
            {
                return;
            }

            for (var index = 0; index < data.Length; index++)
            {
                if (index % Channels != listeningChannel)
                {
                    continue;
                }


                WriteSample(data[index]);
            }
        }

        /// <summary>
        /// Writes the sample to buffer.
        /// </summary>
        /// <param name="value">Sample value to write.</param>
        public void WriteSample(float value)
        {
            AudioBuffer[CurrentWritePosition % (uint)AudioBuffer.Length] = value;

            CurrentWritePosition++;
        }

        /// <summary>
        /// Reads the sample from buffer.
        /// </summary>
        /// <returns>Sample value</returns>
        public float ReadSample()
        {
            var ret = AudioBuffer[CurrentReadPosition % (uint)AudioBuffer.Length];

            CurrentReadPosition++;

            return ret;
        }
    }
}
