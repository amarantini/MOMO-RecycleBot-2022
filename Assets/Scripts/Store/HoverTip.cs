using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tipToShow;
    [SerializeField] float timeToWait = .5f;

    private void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTipManager.onMouseLoseFocus();
    }

    private void ShowMessage()
    {
        Vector2 mouseInput = Input.mousePosition; 
        HoverTipManager.onMouseHover(tipToShow, mouseInput);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);
        ShowMessage();
    } 
}
