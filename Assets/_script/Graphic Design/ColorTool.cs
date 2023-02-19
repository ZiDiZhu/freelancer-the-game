using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Tools for analyzing color
// most methods takes Color as a parameter and outputs a string (to give the color a term)

public class ColorTool : MonoBehaviour
{
    //public GameObject currentColorReference; //reference to "fill" under color picker

    //public Color myColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //space to test function
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    float H, S, V;
        //    Color.RGBToHSV(myColor, out H, out S, out V);
        //    Debug.Log("H: " + H + " S: " + S + " V: " + V) ;
        //    Debug.Log(ColorName(myColor));
        //}
    }


    //12 names
    public string ColorName(Color color)
    {
        string colorName, hName, sName, vName;

        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);

        float[] rgb = { color.r, color.g, color.b };


        //if color is closer to black/white/grays
        if (SaturationOf(color) <= 0.1||StandardDeviation(rgb)<=0.05) 
        {
            if (ValueOf(color) <= 0.2)
            {
                return "black";
            }else if (ValueOf(color) >= 0.8)
            {
                return "white";
            }
            return "gray";
        }

        //recognize hue by hsv - 12 colors 
        return Hue12(HueOf(color));
        //else
        return "Color is undefined";
    }

    //In: Color; Out: float
    public float HueOf(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return H;
    }
    public float SaturationOf(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return S;
    }
    public float ValueOf(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        return V;
    }

    //warm, cool, neutral
    public string ToneOf(string color)
    {
        if (color == "black" || color == "white" || color == "gray")
        {
            return "neutral";
        }if(color == "magenta"|| color == "rose" || color == "red" || color == "orange" || color == "yellow" || color == "lime")
        {
            return "warm";
        }else if (color == "green" || color == "mint" || color == "cyan" || color == "azure" || color == "blue" || color == "purple")
        {
            return "cool";
        }
        else
        {
            return "unknown tone";
        }
    }

    //In: color; Out: string or string[]
    public string ComplementaryOf(Color color)
    {
        //could be replaced by string switch case if this gives mistakes
        float H = HueOf(color);
        H += 0.5f;
        if (H > 1)
        {
            H -= 1;
        }
        return Hue12(H);
    }

    public List<string> AnalogousOf(Color color) 
    {
        List<string> analogousColors = new List<string>();
        float myHue =HueOf(color);
        string myName = Hue12(myHue);
        float leftHue = myHue - 0.1f;
        float rightHue = myHue + 0.1f;
        
        //Loop back if out of range
        if (leftHue < 0)
            leftHue += 1;
        if (rightHue > 1)
            rightHue -= 1;

        string leftAnalogous = Hue12(leftHue);
        string rightAnalogous = Hue12(rightHue);
        analogousColors.Add(leftAnalogous);
        analogousColors.Add(rightAnalogous);
        //if (!myName.Equals(leftAnalogous))
        //    analogousColors.Add(leftAnalogous);

        return analogousColors;
    }


    //In: float; Out: string
    //identify Hue
    public string Hue12(float H)
    {
        float offset = 0.05f; //adjusting the color wheel offset
        float noramlizedH = Mathf.Floor((H + offset) * 12);
        switch (noramlizedH)
        {
            case 0:
                return "red";
            case 1:
                return "orange";
            case 2:
                return "yellow";
            case 3:
                return "lime";
            case 4:
                return "green";
            case 5:
                return "mint";
            case 6:
                return "cyan";
            case 7:
                return "azure";
            case 8:
                return "blue";
            case 9:
                return "purple";
            case 10:
                return "magenta";
            case 11:
                return "rose";
            case 12:
                return "red";
        }
        return "Color is undefined";
    } 
    public string Hue6(float H)
    {
        float offset = 0.1f; //adjusting the color wheel offset
        float noramlizedH = Mathf.Floor((H + offset) * 6);
        switch (noramlizedH)
        {
            case 0:
                return "red";
            case 1:
                return "yellow";
            case 2:
                return "green";
            case 3:
                return "cyan";
            case 4:
                return "blue";
            case 5:
                return "magenta";
            case 6:
                return "red"; //loop back.
        }
        return "Color is undefined";
    }


    //In: float ; Out:float
    //Math Tools
    public float StandardDeviation(float[] x)
    {
        float pv = 0; //population variance
        float mu = MeanValue(x);
        for(int i=0; i < x.Length; i++)
        {
            pv += (x[i]-mu)*(x[i]-mu);
        }
        pv /= x.Length;
        return Mathf.Sqrt(pv);
    }
    public float Sum(float[] x)
    {
        float s = 0;
        for(int i= 0; i<x.Length;i++)
        {
            s += x[i];
        }
        return s;
    }
    public float MeanValue(float[] x) //average
    {
        return Sum(x) / (x.Length);
    }

}
