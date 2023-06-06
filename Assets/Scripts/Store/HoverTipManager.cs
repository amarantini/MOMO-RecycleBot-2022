using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    public static Action<string,Vector2> onMouseHover;
    public static Action onMouseLoseFocus;

    private void OnEnable()
    {
        onMouseHover += showTip;
        onMouseLoseFocus += hideTip;
    }

    private void OnDisable()
    {
        onMouseHover -= showTip;
        onMouseLoseFocus -= hideTip;
    }

    private void Awake()
    {
        hideTip();
    }

    private void showTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth>100?100: tipText.preferredWidth, tipText.preferredHeight);
        tipText.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x * 2, mousePos.y);
    }

    private void hideTip()
    {
        tipWindow.sizeDelta = new Vector2(0, 0);
        tipText.text = default;
        tipText.gameObject.SetActive(false);
    }

}
