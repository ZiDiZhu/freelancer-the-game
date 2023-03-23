using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//attached to each element in the canvas.
public class CanvasElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] DesignControl designControl;
    public bool movable;//can be drag and dropped
    public bool colorable;//can be colored
    // Start is called before the first frame update
    void Start()
    {
        designControl = FindObjectOfType<DesignControl>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        designControl.changeCursor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        designControl.changeCursor(false);
    }
}
