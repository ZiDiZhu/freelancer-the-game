using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using HSVPicker;
using ColorUtil;

//More accurately, currently selected color info displayer
public class DesignAnalysisDisplay : MonoBehaviour
{
    [SerializeField] private ColorPicker colorPicker;
    public Color currentColor;

    public TMP_Text colorNameTMP, colorToneTMP,complementaryNameTMP, analogousText;

    private void Awake()
    {
        if (colorPicker == null)
        {
            colorPicker = FindObjectOfType<ColorPicker>();
        }
        UpdateColorInfo();
        
    }

    //Called on Color picker's on value change
    public void UpdateColorInfo()
    {
        Color currentColor = colorPicker.CurrentColor;
        string currentColorName = ColorInfo.GetHueString(currentColor);

        //display color information
        colorNameTMP.text = "Color: "+ ColorInfo.GetHueString(currentColor);
        
        if(currentColorName == "black"|| currentColorName == "white"|| currentColorName == "gray")
        {
            analogousText.text = "";
            complementaryNameTMP.text = "";
        }
        else
        {

            string analogous = "Analogous: ";
            foreach (string str in ColorInfo.GetAnalogousHueString(currentColorName))
            {
                analogous += " " + str;
            }
            analogousText.text = analogous;

            complementaryNameTMP.text = "Complementary: " + ColorInfo.GetComplementaryHueString(currentColorName);
        }
        colorToneTMP.text ="Tone: " + ColorInfo.ToneOf(currentColorName);

    }




}
