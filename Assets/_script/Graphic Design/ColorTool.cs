using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTool : MonoBehaviour
{
    public GameObject currentColorReference; //reference to "fill" under color picker

    public Color myColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //space to test function
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float H, S, V;
            Color.RGBToHSV(myColor, out H, out S, out V);
            Debug.Log("H: " + H + " S: " + S + " V: " + V) ;
            Debug.Log(ColorName(myColor));
        }
    }


    //12 names
    public string ColorName(Color color)
    {
        string colorName, hName, sName, vName;

        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);

        float[] rgb = { color.r, color.g, color.b };


        //if color is closer to black/white/grays
        if (S <= 0.1||StandardDeviation(rgb)<=0.05) 
        {
            if (V <= 0.2)
            {
                return "black";
            }else if (V >= 0.8)
            {
                return "white";
            }
            return "gray";
        }

        //recognize hue by hsv - 12 colors 
        float offset = 0.05f; //adjusting the color wheel offset
        float noramlizedH = Mathf.Floor((H + offset) *12);
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
                return "spring green";
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

    //map hue to one of the 6 colors
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
