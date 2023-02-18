using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Utility Script that checks if Design fits the requirement
public class DesignRequirement : MonoBehaviour
{
    public List<GameObject> canvasElements; //Needs constant update from the source that contains canvas elements



    ColorTool colortool; //I/O for color info
    // Start is called before the first frame update
    void Start()
    {
        colortool = new ColorTool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    public bool HasColor(string color)
    {
        bool hasColor = false;
        foreach (GameObject elem in canvasElements)
        {
            if (colortool.ColorName(elem.GetComponent<Image>().color) == color)
            {
                return true;
            }
        }

        return hasColor;
    }

    //returns the list of colors that are missing. if returned null, its a success.
    public List<string> HasColors(List<string> colors)
    {
        List<string> missingColors = colors;

        foreach (string color in colors)//for each required color
        {
            foreach (GameObject elem in canvasElements) //for each canvas element
            {
                if (colortool.ColorName(elem.GetComponent<Image>().color) == color)
                {
                    missingColors.Remove(color); //cross color off the list if found 
                }
            }
        }

        return missingColors;
    }



}
