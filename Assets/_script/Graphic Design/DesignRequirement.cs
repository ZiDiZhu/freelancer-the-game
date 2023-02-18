using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//Utility Script that checks if Design fits the requirement
public class DesignRequirement : MonoBehaviour
{
    public List<GameObject> canvasElements; //Needs constant update from the source that contains canvas elements
    [SerializeField] DesignControl designControl;

    public List<string> requiredColors,missingColors,bannedColors,wrongColors; //check if contains or does not contain certain color

    public int minNumberofColors, maxNumberofColors, missingNumberOfColors;//number of colors requiremeny


    ColorTool colortool; //I/O for color info
    // Start is called before the first frame update
    void Start()
    {
        colortool = new ColorTool();
        designControl = GetComponent<DesignControl>();
        canvasElements = designControl.canvasElements;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Evaluate()
    {
        //will give unity an invalid operation exception
        missingColors = GetMissingColorsFrom(requiredColors);
        wrongColors = GetWrongColorsFrom(bannedColors);
    }

    

    //In: required colors; Out: missing colors
    //if returned null, its a success.
    public List<string> GetMissingColorsFrom(List<string> colors)
    {
        List<string> missingList = new List<string>(requiredColors);//deep clone

        foreach (string color in colors)//for each required color
        {
            foreach (GameObject elem in canvasElements) //for each canvas element
            {
                if (colortool.ColorName(elem.GetComponent<Image>().color) == color)
                {
                    missingList.Remove(color); //cross color off the list if found 
                }
            }
        }
        return missingList;
    }

    public List<string> GetWrongColorsFrom(List<string> colors)
    {
        List<string> wrongList = new List<string>();

        foreach (string color in colors)//for each required color
        {
            foreach (GameObject elem in canvasElements) //for each canvas element
            {
                if (colortool.ColorName(elem.GetComponent<Image>().color) == color)
                {
                    wrongList.Add(color); //cross color off the list if found 
                }
            }
        }
        return wrongList;
    }

}
