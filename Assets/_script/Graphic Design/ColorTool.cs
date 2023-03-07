using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tools for analyzing color
// most methods takes Color as a parameter and outputs a string (to give the color a term)

public class ColorTool 
{
    //12 names
    public string ColorName(Color color)
    {

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

    
    public string ComplementaryOf(Color color)
    {
        return ComplementaryOf(ColorName(color));

        //could be replaced by string switch case if this gives mistakes
        //float H = HueOf(color);
        //H += 0.5f;
        //if (H > 1)
        //{
        //    H -= 1;
        //}
        //return Hue12(H);
    }

    //For Canvas evaluation, maybe The analog colors of the complementary also count
    public string ComplementaryOf(string color)
    {
        switch (color)
        {
            case "red":
                return "cyan";
            case "orange":
                return "azure";
            case "yellow":
                return "blue";
            case "lime":
                return "purple";
            case "green":
                return "magenta";
            case "mint":
                return "rose";
            case "cyan":
                return "red";
            case "azure":
                return "orange";
            case "blue":
                return "yellow";
            case "purple":
                return "lime";
            case "magenta":
                return "green";
            case "rose":
                return "mint";
            default:
                return "n/a";
        }
    }

    //CHANGED: This list now contains the color itself
    public List<string> AnalogousOf(Color color)
    {
        List<string> analogousColors = new List<string>();
        float myHue = HueOf(color);
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
        analogousColors.Add(Hue12(myHue));

        return analogousColors;
    }

    public List<string> AnalogousOf(string color)
    {
        switch (color)
        {
            case "red":
                return new List<string>{ "orange","rose"};
            case "orange":
                return new List<string> { "red", "yellow" };
            case "yellow":
                return new List<string> { "orange", "lime" };
            case "lime":
                return new List<string> { "yellow", "green" };
            case "green":
                return new List<string> { "mint", "lime" };
            case "mint":
                return new List<string> { "green", "cyan" };
            case "cyan":
                return new List<string> { "mint", "azure" };
            case "azure":
                return new List<string> { "cyan", "blue" };
            case "blue":
                return new List<string> { "azure", "purple" };
            case "purple":
                return new List<string> { "blue", "magenta" };
            case "magenta":
                return new List<string> { "purple", "rose" };
            case "rose":
                return new List<string> { "red", "magenta" };
            default:
                return null;
        }
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
