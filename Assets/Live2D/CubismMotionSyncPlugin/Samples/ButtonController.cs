/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;


/// <summary>
/// 音声再生及び停止用のボタンを制御する
/// </summary>
public class ButtonController : MonoBehaviour
{
    [SerializeField, HideInInspector]
    public AudioSource ActiveAudioSource;

    public void Play()
    {
        ActiveAudioSource.Play();
    }

    public void Stop()
    {
        ActiveAudioSource.Stop();
    }
}
