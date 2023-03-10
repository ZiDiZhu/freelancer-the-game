using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//Utility Script that checks if Design fits the requirement
public class DesignRequirement : MonoBehaviour
{
    //Other Scripts attached to this gameObject
    private DesignControl designControl;
    private RequirementUI requirementUI; //displayer script

    [Header("Dependency - Scene")]
    public List<GameObject> canvasElements; //To DO: constant update from the source that contains canvas elements

    [Header("Dependency - Scriptable Object")]
    public CommissionObject commissionObject; //The scriptable commission Object that contains level requirements

    [Header("Current Commission Information")]
    public List<string> requiredColors;
    public List<string> bannedColors;
    public int minNumberofColors;
    public int maxNumberofColors;

    [Header("Current Canvas Information")]
    public List<Color> myColors;
    public List<string> myColorsNames;

    [Header("Current Canvas Information In Relation to Commission")]
    public List<string> wrongColors;
    public List<string> missingColors;
    public int missingNumberOfColors;


    public float score;

    //current design "combo"s
    private bool monochromatic, analogous, complementary, splitcomplementary;

    public ColorScheme colorScheme,requiredcolorScheme;
    public enum ColorScheme
    {
        None,
        Monochromatic,
        Analogous,
        Complementary,
        SplitComplementary,
        Triadic,
        Tetradic
    }
    public enum Tone
    {
        Neutral, //no leaning
        Warm, //warm leaning
        Hot, //all warm
        Cool,//cool leaning
        Cold//all cool
    }

    private void Awake()
    {
        requirementUI = GetComponent<RequirementUI>();
        colortool = new ColorTool();
        designControl = GetComponent<DesignControl>();
    }

    ColorTool colortool; //I/O for color info
    // Start is called before the first frame update
    void Start()
    {
        canvasElements = designControl.canvasElements;
        AssignRequirementsFromCommissionObject(commissionObject);
        
        requirementUI.InitializeRequirementList();
        requirementUI.UpdateRequirement();
    }

    //assigns the commission object data in this gameobject
    public void AssignRequirementsFromCommissionObject(CommissionObject co)
    {
        requiredColors = co.mustIncludeColors;
        bannedColors = co.doNotIncludeColors;
        maxNumberofColors = co.maxNumberColors;
        minNumberofColors = co.minNumberColors;
        requiredcolorScheme = co.requiredColorScheme;
    }

    public void Evaluate()
    {
        myColors = GetColorsFromCanvas(canvasElements);
        myColorsNames = GetColorStringsFromCanvasElemList(canvasElements);
        missingColors = GetMissingColorsFrom(requiredColors);
        wrongColors = GetWrongColorsFrom(bannedColors);
        missingNumberOfColors = GetOutOfRangeNumber(minNumberofColors, maxNumberofColors);

        EvaluateCombos();

        score = PercentageScore();
    }

    public string EvaluateTone()
    {
        myColorsNames = GetColorStringsFromCanvasElemList(canvasElements);
        int nOfColors = myColors.Count;
        int temp = 0;
        foreach(string color in myColorsNames)
        {
            if (colortool.ToneOf(color) == "warm")
            {
                temp++;
            }
            else if (colortool.ToneOf(color) == "cool")
            {
                temp--;
            }
        }

        if(temp == nOfColors)
        {
            return "warm";
        }else if(temp == nOfColors * -1)
        {
            return "cool";
        }else if(temp > 0)
        {
            return "warm-ish";
        }else if (temp < 0)
        {
            return "cool-ish";
        }else if(temp == 0)
        {
            return "neutral";
        }
        else
        {
            return "error in tone evaluation";
        }
    }


    public void EvaluateCombos()
    {
        List<string> myColors = GetDistinctColorsFromCanvasElems(canvasElements);
        int nOfColors = myColors.Count;

        //check if monochromatic
        if(nOfColors == 1)
        {
            monochromatic = true;
            colorScheme = ColorScheme.Monochromatic;
        }else if (myColors.Count == 2&&(myColors.Contains("black")||myColors.Contains("white")))
        {
            monochromatic = true;
            colorScheme = ColorScheme.Monochromatic;
        }
        else
        {
            monochromatic = false;
        }

        //check if analogous : 2-4 colors next to each other
        analogous = CheckIfAnalogous(canvasElements);
        if (analogous)
        {
            colorScheme = ColorScheme.Analogous;
        }
        complementary = CheckIfComplementary(canvasElements);
        if (complementary)
        {
            colorScheme = ColorScheme.Complementary;
        }
        if (!complementary && !analogous && !monochromatic)
        {
            colorScheme = ColorScheme.None;
        }

        //check if complementary
        if(nOfColors >= 3 && !myColors.Contains("black") && !myColors.Contains("gray") && !myColors.Contains("white"))
        {
            bool isSplit = false;
            foreach(string color in myColors)
            {
                foreach (string c in colortool.AnalogousOf(colortool.ComplementaryOf(color)))
                {
                    isSplit = false;
                    if (myColors.Contains(c))
                    {
                        isSplit = true;
                    }
                }
            }
            if (isSplit)
            {
                colorScheme = ColorScheme.SplitComplementary;

            }
        }

        //check if is triad
        if (nOfColors == 3&&!myColors.Contains("black")&&!myColors.Contains("gray")&&!myColors.Contains("white"))
        {
            bool triad = true;
            foreach(string color in myColors)
            {
                if (myColors.Contains(colortool.ComplementaryOf(color)))
                {
                    triad = false;
                }
            }
            if (triad)
            {
                colorScheme = ColorScheme.Triadic;
            }
        }

        //check if is tetradic
        if (nOfColors == 4 && !myColors.Contains("black") && !myColors.Contains("gray") && !myColors.Contains("white"))
        {
            bool triad = true;
            foreach (string color in myColors)
            {
                if (!myColors.Contains(colortool.ComplementaryOf(color)))
                {
                    triad = false;
                }
            }
            if (triad)
            {
                colorScheme = ColorScheme.Tetradic;
            }
        }

    }

    public bool CheckIfAnalogous(List<GameObject> gameObjects)
    {
        //List<string> myColors = GetDistinctColorsFromCanvasElems(gameObjects);
        if (myColors.Count >= 2) //needs at least 2 
        {
            foreach (GameObject go in canvasElements)
            {
                List<string> a = colortool.AnalogousOf(go.GetComponent<Image>().color);
                bool found = false;
                foreach (string ac in a)
                {
                    if (myColorsNames.Contains(ac)&&ac!=colortool.ColorName(go.GetComponent<Image>().color))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool CheckIfComplementary(List<GameObject> gameObjects)
    {
        List<string> myColors = GetDistinctColorsFromCanvasElems(gameObjects);
        if(myColors.Count == 2 || (myColors.Count == 3 && (myColors.Contains("black") || myColors.Contains("white") || myColors.Contains("gray"))))
        {
            foreach(GameObject go in gameObjects)
            {
                //Analogous of complementary works too, for a more forgiving evaluation
                List<string> simlarColors = colortool.AnalogousOf(go.GetComponent<Image>().color);
                foreach(string color in simlarColors)
                {
                    if (myColors.Contains(colortool.ComplementaryOf(color)))
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }
    


    public float PercentageScore()
    {
        float totalPossibleScore = requiredColors.Count + bannedColors.Count;
        if (requiredcolorScheme != null)
        {
            totalPossibleScore += 3;
        }
        
        float myScore = totalPossibleScore - missingColors.Count - wrongColors.Count - missingNumberOfColors;
        if (requiredcolorScheme !=ColorScheme.None&&requiredcolorScheme != colorScheme)
        {
            myScore -= 3;
        }
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
            foreach (string colorName in myColorsNames) //for each color in current canvas
            {
                if (colorName == color && !wrongList.Contains(color))
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
        if (n > max)
        {
            return max -n;
        }else if (n < min)
        {
            return min - n;
        }
        return 0;
    }

    public List <Color> GetColorsFromCanvas(List<GameObject> go)
    {
        List<Color> colors = new List<Color>();
        foreach (GameObject elem in go)
        {
            colors.Add(elem.GetComponent<Image>().color);
        }
        return colors;
    }

    public List<string> GetColorStringsFromCanvasElemList(List<GameObject> go)
    {
        List<string> colors = new List<string>();
        foreach(GameObject elem in go)
        {
            colors.Add(colortool.ColorName(elem.GetComponent<Image>().color));
        }
        return colors;
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
