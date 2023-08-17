/**
 * Copyright(c) Live2D Inc. All rights reserved.
 *
 * Use of this source code is governed by the Live2D Open Software license
 * that can be found at https://www.live2d.com/eula/live2d-open-software-license-agreement_en.html.
 */


using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 表示するモデルを切り替える
/// </summary>
public class ModelSelecter : MonoBehaviour
{
    /// <summary>
    /// Models.
    /// </summary>
    [SerializeField]
    public GameObject[] Models;

    /// <summary>
    /// Drop-down for model switching.
    /// </summary>
    [SerializeField]
    public Dropdown ModelSelectDropdown;

    private void Start()
    {
        if (Models == null)
        {
            return;
        }

        for (var i = 0; i < Models.Length; i++)
        {
            ModelSelectDropdown.options[i].text = Models[i].gameObject.name;
            Models[i]?.SetActive(false);
        }

        Models[0].SetActive(true);
    }

    /// <summary>
    /// モデルの選択
    /// </summary>
    public void SelectModel()
    {
        var selectIndex = ModelSelectDropdown.value;
        if (Models == null || Models.Length <= selectIndex
            || Models[selectIndex] == null)
        {
            return;
        }

        for (var i = 0; i < Models.Length; i++)
        {
            Models[i]?.SetActive(false);
        }

        Models[selectIndex].SetActive(true);
    }
}
