using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class DesignAnalysisDisplay : MonoBehaviour
{

    public Color currentColor;
    public GameObject currentColorReference;

    public TMP_Text colorNameTMP, complementaryNameTMP, analogousText;
    ColorTool colortool;
    // Start is called before the first frame update
    void Start()
    {
        colortool = new ColorTool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called on Color picker's on value change
    public void UpdateColorInfo()
    {
        Color currentColor = currentColorReference.GetComponent<Image>().color;

        //display color information
        colorNameTMP.text = "Color: "+colortool.ColorName(currentColor);
        
        if(colortool.ColorName(currentColor) =="black"|| colortool.ColorName(currentColor)=="white"|| colortool.ColorName(currentColor) == "gray")
        {
            analogousText.text = "";
            complementaryNameTMP.text = "";
        }
        else
        {

            string analogous = "Analogous: ";
            foreach (string str in colortool.AnalogousOf(currentColor))
            {
                analogous += " " + str;
            }
            analogousText.text = analogous;

            complementaryNameTMP.text = "Complementary: " + colortool.ComplementaryOf(currentColor);
        }
        

    }
}
