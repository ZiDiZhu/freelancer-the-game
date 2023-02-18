using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//this is a test script for the design app
public class GraphicShape : MonoBehaviour
{
    //public GameObject[] shape;//shapes in a template, which are buttons
    //public GameObject selectedShape;
    //public GameObject[] colorSwatch;
    //public Button bucketTool;
    //public Color currentColor;
    //public GameObject currentColorReference; //to fix the issue where sometimes current color dont update. reference to "fill" under color picker

    //public TMP_Text colorNameTMP, complementaryNameTMP, analogousText;
    //ColorTool colortool;


    ////requirements mechanic
    //public Toggle[] requirements;//TEMP

    //private void Start()
    //{
    //    colortool = new ColorTool();

    //    for(int i = 0; i < shape.Length; i++)
    //    {
    //        int n = i;//to prevent variable capturing
    //        shape[n].GetComponent<Button>().onClick.AddListener(() => clickedShape(shape[n]));//so that clicking on button triggers a callback
            
    //    }

    //    for(int i = 0; i < colorSwatch.Length; i++)
    //    {
    //        int n = i;
    //        colorSwatch[n].GetComponent<Button>().onClick.AddListener(() => colorSwatchClicked(colorSwatch[n]));
    //    }

    //}

    //public void SetCurrentColor(Image image)
    //{
    //    currentColor = image.color;

    //    //display color information
    //    colorNameTMP.text = colortool.ColorName(currentColor);
    //    complementaryNameTMP.text = colortool.ComplementaryOf(currentColor);
    //    string analogous = "";
    //    foreach (string str in colortool.AnalogousOf(currentColor))
    //    {
    //        analogous += " " + str;
    //    }
    //    analogousText.text = analogous;


    //    Debug.Log(currentColor);
    //}

    //private void clickedShape(GameObject myShape)
    //{
    //    SetCurrentColor(currentColorReference.GetComponent<Image>());
    //    //fill color
    //    myShape.GetComponent<Shape>().myColor = currentColor;
    //    myShape.GetComponent<Image>().color = currentColor;


    //    //TEMP
    //    TestLevel();
            
    //}

    //private void TestLevel()
    //{
    //    if (HasColor("cyan"))
    //    {
    //        requirements[0].isOn = true;
    //    }
    //    if (HasColor("yellow"))
    //    {
    //        requirements[1].isOn = true;
    //    }
    //    if (HasColor("red") || HasColor("blue"))
    //    {
    //        requirements[2].isOn = true;
    //    }
    //}

    ////TEMP
    //private bool HasColor(string color)
    //{
    //    bool hasColor = false;
    //    foreach(GameObject sh in shape)
    //    {
    //        if (colortool.ColorName(sh.GetComponent<Image>().color) == color)
    //        {
    //            return true;
    //        }
    //    }

    //    return hasColor;
    //}


    //public void colorSwatchClicked(GameObject myColorSwatch)
    //{
    //    if (selectedShape != null)
    //    {
    //        //selectedShape.GetComponent<Shape>().myColor = currentColor;
    //        currentColor = myColorSwatch.GetComponent<Image>().color;
    //    }
    //}

    //void DeselectShape()
    //{
    //    selectedShape = null;
    //    EventSystem.current.SetSelectedGameObject(null);
    //}


}
