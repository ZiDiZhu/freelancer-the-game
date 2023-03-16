using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorUtil
{
    public class ColorInfo
    {
        //return h,s,v mapped to a float 0-1
        public static float GetHueFloat(Color color)
        {
            float H, S, V;
            Color.RGBToHSV(color, out H, out S, out V);
            return H;
        }
        public static float GetSaturationFloat(Color color)
        {
            float H, S, V;
            Color.RGBToHSV(color, out H, out S, out V);
            return S;
        }
        public static float GetValueFloat(Color color)
        {
            float H, S, V;
            Color.RGBToHSV(color, out H, out S, out V);
            return V;
        }

        
        public static string GetHueString(Color color)
        {
            float[] rgb = { color.r, color.g, color.b };


            float H = GetHueFloat(color);
            return "";
        }


    }
}