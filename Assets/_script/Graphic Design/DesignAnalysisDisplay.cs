using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//More accurately, currently selected color info displayer
public class DesignAnalysisDisplay : MonoBehaviour
{

    public Color currentColor;
    public GameObject currentColorReference;

    public TMP_Text colorNameTMP, colorToneTMP,complementaryNameTMP, analogousText;
    ColorTool colortool;

    private void Awake()
    {
        colortool = new ColorTool();
    }

    //Called on Color picker's on value change
    public void UpdateColorInfo()
    {
        Color currentColor = currentColorReference.GetComponent<Image>().color;
        string currentColorName = colortool.ColorName(currentColor);

        //display color information
        colorNameTMP.text = "Color: "+colortool.ColorName(currentColor);
        
        if(currentColorName == "black"|| currentColorName == "white"|| currentColorName == "gray")
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
        colorToneTMP.text ="Tone: " + colortool.ToneOf(currentColorName);

    }




}
