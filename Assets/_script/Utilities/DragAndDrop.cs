using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;


    private RectTransform rectTransform,parentRectTransform;
    [SerializeField] private float border_left, border_right, border_top, border_down;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        border_left = parentRectTransform.anchoredPosition.x;
        border_right = parentRectTransform.anchoredPosition.x + parentRectTransform.sizeDelta.x - rectTransform.sizeDelta.x;
        border_top = parentRectTransform.anchoredPosition.y+ parentRectTransform.sizeDelta.y/2-rectTransform.sizeDelta.y/2;
        border_down = parentRectTransform.anchoredPosition.y - parentRectTransform.sizeDelta.y / 2 + rectTransform.sizeDelta.y/2;
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
        bool readable = FindObjectOfType<DesignRequirement>().IsReadable(rectTransform.rect.width*0.5f,30f);
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
        if (rectTransform.anchoredPosition.x >= border_left
            && rectTransform.anchoredPosition.x <= border_right
            && rectTransform.anchoredPosition.y <= border_top
            && rectTransform.anchoredPosition.y >= border_down) //within border of parents
        {
            rectTransform.anchoredPosition += eventData.delta / transform.parent.localScale;

        }
        else //outside of border
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

}