using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool withinParentalBorder = false;//if true, confined to parent's recttransform

    private Canvas canvas;

    private DesignRequirement dR;

    private RectTransform rectTransform,parentRectTransform;
    private float border_left, border_right, border_top, border_down;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        border_left = parentRectTransform.anchoredPosition.x - rectTransform.sizeDelta.x/2; ;
        border_right = parentRectTransform.anchoredPosition.x + parentRectTransform.sizeDelta.x;
        border_top = parentRectTransform.anchoredPosition.y+ parentRectTransform.sizeDelta.y/2-rectTransform.sizeDelta.y/2;
        border_down = parentRectTransform.anchoredPosition.y - parentRectTransform.sizeDelta.y / 2 + rectTransform.sizeDelta.y/2;

        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveWithinParent(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (FindObjectOfType<DesignRequirement>().commissionObject.requiresReadability)
        {

            if (rectTransform.anchoredPosition.x >= border_left
            && rectTransform.anchoredPosition.x <= border_right
            && rectTransform.anchoredPosition.y <= border_top
            && rectTransform.anchoredPosition.y >= border_down) //within border of parents
            {
                withinParentalBorder = true;

            }
            else //outside of border
            {
                withinParentalBorder = false;
            }

            bool readable = FindObjectOfType<DesignRequirement>().IsReadable(rectTransform.rect.width * 0.3f, 80f);
            FindObjectOfType<RequirementUI>().Readability(readable);
            FindObjectOfType<RequirementUI>().CheckRequiredElementsInFrame();
        }
        
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BringToFront();
    }

    public void BringToFront()
    {
        transform.SetAsLastSibling();//bring to front
    }

    //move object within parent object's rect transform
    public void MoveWithinParent(PointerEventData eventData)
    {
        

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
     private void SnapToBorder()
    {
        //leftmost
        if (rectTransform.anchoredPosition.x < border_left)
        {
            rectTransform.anchoredPosition = new Vector2(border_left, rectTransform.anchoredPosition.y);
        }
        //rightmost
        if (rectTransform.anchoredPosition.x > border_right)
        {
            rectTransform.anchoredPosition = new Vector2(border_right, rectTransform.anchoredPosition.y);
        }
        //topmost
        if (rectTransform.anchoredPosition.y > border_top)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, border_top);
        }
        //downmost
        if (rectTransform.anchoredPosition.y < border_down)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, border_down);
        }
    }
}