using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//attached to each element in the canvas.
public class CanvasElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] DesignControl designControl;
    // Start is called before the first frame update
    void Start()
    {
        designControl = FindObjectOfType<DesignControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
