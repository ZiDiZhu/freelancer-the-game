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

    private void Start()
    {
        for(int i = 0; i < shape.Length; i++)
        {
            int n = i;//to prevent variable capturing
            shape[n].GetComponent<Button>().onClick.AddListener(() => buttonCallBack(shape[n]));//so that clicking on button triggers a callback
        }

        for(int i = 0; i < colorSwatch.Length; i++)
        {
            int n = i;
            colorSwatch[n].GetComponent<Button>().onClick.AddListener(() => colorSwatchClicked(colorSwatch[n]));

        }

    }


    private void buttonCallBack(GameObject myShape)
    {
        Debug.Log("Pressed button" + myShape.name);
        if (selectedShape == myShape)
        {
            DeselectShape();
        }

        else
        {
            selectedShape = myShape;
            selectedShape.GetComponent<Outline>().enabled = true;
        }
            
    }

    private void colorSwatchClicked(GameObject myColorSwatch)
    {
        if (selectedShape != null)
        {
            selectedShape.GetComponent<Image>().color = myColorSwatch.GetComponent<Image>().color;
        }
    }

    void DeselectShape()
    {
        selectedShape.GetComponent<Outline>().enabled = false;
        selectedShape = null;
        EventSystem.current.SetSelectedGameObject(null);
    }


}
