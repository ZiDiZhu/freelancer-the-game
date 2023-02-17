using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//Action script for the design app. Replace old script "GraphicShape"
public class DesignControl : MonoBehaviour
{
    public List<GameObject> canvasElements; //things in your canvas




    public Button dragBtn, bucketBtn; //delete btn to be added

    //mode of interaction with shape
    enum InteractionMode
    {
        Drag, //drag and drop the shape on the canvas. Disable for template
        Bucket, //colors the shape
        Delete //deletes the shape. Disable for templates
    }


    


    // Start is called before the first frame update
    void Start()
    {
        InitializeButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //link buttons to callbacks
    private void InitializeButtons()
    {
        dragBtn.onClick.AddListener(() => DragBtnClicked());
        bucketBtn.onClick.AddListener(() => BucketBtnClicked());
    }

    //Link canvas elements to callback. (has to be clickable) 
    public void InitializeCanvasElements()
    {

    }

    //calls when drag (move) button gets clicked
    void DragBtnClicked()
    {

    }
    void BucketBtnClicked()
    {

    }

    void CanvasElementClicked()
    {

    }


}
