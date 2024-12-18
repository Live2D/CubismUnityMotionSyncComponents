﻿/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */

#if !UNITY_WEBGL
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Live2D.CubismMotionSyncPlugin.Samples.Microphone
{
    /// <summary>
    /// Motion sync microphone input.
    /// </summary>
    public class MotionSyncMicInput : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        public AudioSource AudioSource;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        public Dropdown MicSelectDropdown;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        public int RecordingLengthSec = 1;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        public string _selectedDeviceName;

        private const float WaitMilliSec = 1000.0f;
        private const int WaitThredMilliSec = 50;

        private int _minFrequency;
        private int _maxFrequency;

        // Start is called before the first frame update
        private void Start()
        {
            if (UnityEngine.Microphone.devices.Length == 0)
            {
                return;
            }

            if (AudioSource == null)
            {
                AudioSource = GetComponent<AudioSource>();
            }

            if (AudioSource == null)
            {
                return;
            }

            AudioSource.loop = true;
            AudioSource.mute = false;

            MicSelectDropdown.ClearOptions();
            var options = new System.Collections.Generic.List<Dropdown.OptionData>();
            for (var i = 0; i < UnityEngine.Microphone.devices.Length; i++)
            {
                options.Add(new Dropdown.OptionData(UnityEngine.Microphone.devices[i]));
            }
            MicSelectDropdown.AddOptions(options);

            StartMicrohphone(0);

            if (AudioSource.clip == null)
            {
                UnityEngine.Debug.LogError("AudioSource.clip is null.");
                return;
            }

            var timer = Stopwatch.StartNew();

            // Wait until the recording has started
            while (UnityEngine.Microphone.GetPosition(_selectedDeviceName) <= 0 && timer.Elapsed.TotalMilliseconds < RecordingLengthSec * WaitMilliSec)
            {
                Thread.Sleep(WaitThredMilliSec);
            }

            if (UnityEngine.Microphone.GetPosition(_selectedDeviceName) <= 0)
            {
                UnityEngine.Debug.LogError("Timeout initializing: " + _selectedDeviceName);
                return;
            }

            AudioSource.Play();
        }

        private void OnDestroy()
        {
            UnityEngine.Microphone.End(_selectedDeviceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceIndex"></param>
        private void StartMicrohphone(int deviceIndex)
        {
            _selectedDeviceName = UnityEngine.Microphone.devices[deviceIndex];
            // Get device capacity.
            UnityEngine.Microphone.GetDeviceCaps(_selectedDeviceName, out _minFrequency, out _maxFrequency);

            if (_minFrequency == 0 && _maxFrequency == 0)
            {
                _minFrequency = AudioSettings.outputSampleRate;
                _maxFrequency = AudioSettings.outputSampleRate;
            }

            AudioSource.Stop();
            AudioSource.clip = UnityEngine.Microphone.Start(_selectedDeviceName, true, RecordingLengthSec, AudioSettings.outputSampleRate);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SelectMicrophoneDevice()
        {
            // Now device process ending.
            UnityEngine.Microphone.End(_selectedDeviceName);
            var selectIndex = MicSelectDropdown.value;

            // Change device.
            StartMicrohphone(selectIndex);

            if (AudioSource.clip == null)
            {
                UnityEngine.Debug.LogError("AudioSource.clip is null.");
                return;
            }

            AudioSource.Play();
        }
    }
}
#endif
