using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImprovedButton: Button
{
    public Action OnPointerEnterEvent;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        OnPointerEnterEvent?.Invoke();
    }

}