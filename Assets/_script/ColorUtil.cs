using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace ColorUtil
{
    //Evaluate Single color
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

        //return string
        public static string GetHueString(Color color)
        {
            float[] rgb = { color.r, color.g, color.b };


            float H, S, V;//hue, saturation,color
            Color.RGBToHSV(color, out H, out S, out V);

            //White Black Grays
            if (S<=0.1 || MathUtils.StandardDeviation(rgb) <= 0.05)
            {
                if (V <= 0.2)
                {
                    return "black";
                }
                else if (V >= 0.8)
                {
                    return "white";
                }
                return "gray";
            }

            //Other Color
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

        //return string or strings
        public static string GetComplementaryHueString(string color)
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

        public static List<string> GetAnalogousHueString(string color)
        {
            switch (color)
            {
                case "red":
                    return new List<string> { "orange", "rose" };
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
                    return new List<string> { "cyan", "blue"};
                case "blue":
                    return new List<string> { "azure", "purple" };
                case "purple":
                    return new List<string> { "blue", "magenta" };
                case "magenta":
                    return new List<string> { "purple", "rose"};
                case "rose":
                    return new List<string> { "red", "magenta"};
                default:
                    return null;
            }
        }
        public static string ToneOf(string color)
        {
            if (color == "black" || color == "white" || color == "gray")
            {
                return "neutral";
            }
            if (color == "magenta" || color == "rose" || color == "red" || color == "orange" || color == "yellow" || color == "lime")
            {
                return "warm";
            }
            else if (color == "green" || color == "mint" || color == "cyan" || color == "azure" || color == "blue" || color == "purple")
            {
                return "cool";
            }
            else
            {
                return "unknown tone";
            }
        }
    }

    //Evaluate List of Colors
    public class ColorCombination
    {
        //In: distinct hue strings; Out: if analogous
        public static bool IsAnalogous(List<string> colors)
        {
            if(colors.Count >= 2)
            {
                foreach(string color in colors)
                {
                    bool found = false;
                    List<string> analogousOfColor = ColorInfo.GetAnalogousHueString(color);
                    foreach(string item in analogousOfColor)
                    {
                        if (colors.Contains(item)) //if any analogous that is not the color itself can be found 
                        {
                            found = true;
                        }
                    }
                    if (!found) //if any color on canvas doesn't have any analogous
                    {
                        return false;
                    }
                }
                return true; //if all colors has an analogous
            }
            else//if less than 2 colors
            {
                return false;
            }
        }

        public static bool IsComplementary(List<string> colors)
        {
            if (colors.Count == 2)
            {
                string exactComplementary = ColorInfo.GetComplementaryHueString(colors[0]);
                if (colors[1]== exactComplementary){
                    return true;
                }
                List<string> validComplementary = ColorInfo.GetAnalogousHueString(exactComplementary);
                if(colors[1]==validComplementary[0]|| colors[1] == validComplementary[1])
                {
                    return true;
                }
                return false;
            }
            else //if not exatly 2 colors
            {
                return false;
            }
        }

    }
}