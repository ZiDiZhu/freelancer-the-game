using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//Utility Script that checks if Design fits the requirement
public class DesignRequirement : MonoBehaviour
{
    public CommissionObject commissionObject; //To Do: take requirements from this object

    public List<GameObject> canvasElements; //To DO: constant update from the source that contains canvas elements
    [SerializeField] DesignControl designControl;
    [SerializeField] RequirementUI requirementUI; //displayer script
    public List<string> requiredColors,missingColors,bannedColors,wrongColors; //check if contains or does not contain certain color
    public int minNumberofColors, maxNumberofColors, missingNumberOfColors;//number of colors requiremeny
    public float score;


    private void Awake()
    {
        requirementUI = GetComponent<RequirementUI>();
    }

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
        missingNumberOfColors = GetOutOfRangeNumber(minNumberofColors, maxNumberofColors);
        score = PercentageScore();
    }
    public float PercentageScore()
    {
        float totalPossibleScore = requiredColors.Count + bannedColors.Count;
        float myScore = totalPossibleScore - missingColors.Count - wrongColors.Count - missingNumberOfColors;
        return myScore / totalPossibleScore;
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
    public int GetOutOfRangeNumber(int min, int max)
    {
        int n = GetDistinctColorsFromCanvasElems(canvasElements).Count;
        Debug.Log(n);
        if (n > max)
        {
            return max -n;
        }else if (n < min)
        {
            return min - n;
        }
        return 0;
    }
    public List<string> GetColorStringsFromCanvasElemList(List<GameObject> go)
    {
        List<string> myColors = new List<string>();
        foreach(GameObject elem in go)
        {
            myColors.Add(colortool.ColorName(elem.GetComponent<Image>().color));
        }
        return myColors;
    }
    public List<string> GetDistinctColorsFromCanvasElems(List<GameObject> go)
    {
        List<string> myColors = new List<string>();
        foreach (GameObject elem in go)
        {
            if (!myColors.Contains(colortool.ColorName(elem.GetComponent<Image>().color)))
            {
                myColors.Add(colortool.ColorName(elem.GetComponent<Image>().color));
            }
        }
        return myColors;
    }


}
