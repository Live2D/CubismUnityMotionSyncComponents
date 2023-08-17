/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 再生する音声を切り替える
/// </summary>
public class AudioSelecter : MonoBehaviour
{
    /// <summary>
    /// ボタンの制御用コンポーネント
    /// </summary>
    [SerializeField]
    public ButtonController ButtonController;

    /// <summary>
    /// このモデルで利用するAudioSource
    /// </summary>
    [SerializeField]
    public AudioSource AudioSource;

    /// <summary>
    /// 利用するAudioClip
    /// </summary>
    [SerializeField]
    public AudioClip[] Clips;

    /// <summary>
    /// 音声切り替え用ドロップダウン
    /// </summary>
    [SerializeField]
    public Dropdown AudioSelectDropdown;

    private void OnEnable()
    {
        if (AudioSelectDropdown == null || AudioSource == null)
        {
            return;
        }

        AudioSelectDropdown.gameObject.SetActive(true);

        AudioSource.clip = Clips[AudioSelectDropdown.value];
        ButtonController.ActiveAudioSource = AudioSource;
    }

    private void Start()
    {
        if (Clips.Length > AudioSelectDropdown.options.Count)
        {
            AudioSelectDropdown.options.RemoveRange(0, AudioSelectDropdown.options.Count);
        }

        for (var i = 0; i < Clips.Length; i++)
        {
            AudioSelectDropdown.options.Add(new Dropdown.OptionData(Clips[i].name));
        }
    }

    /// <summary>
    /// 音声の選択
    /// </summary>
    public void AudioSelect()
    {
        if (AudioSelectDropdown == null || AudioSource == null)
        {
            return;
        }

        AudioSource.Stop();
        AudioSource.clip = Clips[AudioSelectDropdown.value];
    }

    private void OnDisable()
    {
        if (AudioSelectDropdown == null || AudioSource == null)
        {
            return;
        }

        AudioSource.Stop();
        AudioSelectDropdown.gameObject.SetActive(false);
    }
}
