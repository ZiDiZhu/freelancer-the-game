using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//A Shape is an element inside the design canvas
public class Shape : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    
    public GraphicShape graphicShape;
    public Color myColor;
    public Texture2D bucketCursorTexture;
    
    // Start is called before the first frame update
    void Start()
    {
        myColor = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //GetComponent<Image>().color = graphicShape.currentColor;
        Cursor.SetCursor(bucketCursorTexture,Vector2.zero,CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //GetComponent<Image>().color = myColor;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
