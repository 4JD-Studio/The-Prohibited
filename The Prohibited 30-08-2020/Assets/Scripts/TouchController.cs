using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private float difference;
    public float percentageThreshold = 0.2f;

    public void OnDrag(PointerEventData eventData)
    {
        difference = eventData.pressPosition.x - eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentageThreshold)
        {
            if(percentage > 0)
            {
                UIController.Inistance.OnHowToPlayNextClick(true);
            }
            else if(percentage < 0)
            {
                UIController.Inistance.OnHowToPlayNextClick(false);
            }
        }
    }
}
