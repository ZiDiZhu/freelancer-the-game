using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//temp, test for a "graphical template"
public class GraphicShape : MonoBehaviour
{
    public GameObject[] shape;//shapes in a template, which are buttons
    public GameObject selectedShape;
    public GameObject[] colorSwatch;
    public Button bucketTool;
    public Color currentColor;
    public GameObject currentColorReference; //to fix the issue where sometimes current color dont update. reference to "fill" under color picker

    private void Start()
    {
        for(int i = 0; i < shape.Length; i++)
        {
            int n = i;//to prevent variable capturing
            shape[n].GetComponent<Button>().onClick.AddListener(() => clickedShape(shape[n]));//so that clicking on button triggers a callback
            
        }

        for(int i = 0; i < colorSwatch.Length; i++)
        {
            int n = i;
            colorSwatch[n].GetComponent<Button>().onClick.AddListener(() => colorSwatchClicked(colorSwatch[n]));

        }

    }

    public void SetCurrentColor(Image image)
    {
        currentColor = image.color;
    }

    private void clickedShape(GameObject myShape)
    {
        SetCurrentColor(currentColorReference.GetComponent<Image>());
        //fill color
        myShape.GetComponent<Shape>().myColor = currentColor;
        myShape.GetComponent<Image>().color = currentColor;

        /*
        Debug.Log("Pressed button" + myShape.name);
        if (selectedShape == myShape)
        {
            DeselectShape();
        }

        else
        {
            selectedShape = myShape;
        }
        */
            
    }

    public void colorSwatchClicked(GameObject myColorSwatch)
    {
        if (selectedShape != null)
        {
            //selectedShape.GetComponent<Shape>().myColor = currentColor;
            currentColor = myColorSwatch.GetComponent<Image>().color;
        }
    }

    void DeselectShape()
    {
        selectedShape = null;
        EventSystem.current.SetSelectedGameObject(null);
    }


}
