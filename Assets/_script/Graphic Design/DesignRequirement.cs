using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Util;

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
    public List<string> myColorsNamesDistinct;

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
        designControl = GetComponent<DesignControl>();
        colortool = new ColorTool();
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
        EvaluateColorRequirements();
        
        EvaluateColorScheme();

    }


    public void EvaluateColorRequirements()
    {
        myColors = GetColorsFromCanvas(canvasElements);
        myColorsNames = GetStringsFromColors(myColors);
        myColorsNamesDistinct = ListUtils.GetDistinctElems(myColorsNames);
        missingColors = ListUtils.GetMissingElements(requiredColors, myColorsNamesDistinct);
        wrongColors = ListUtils.GetSharedListElements(myColorsNamesDistinct, bannedColors);
        missingNumberOfColors = MathUtils.GetDistanceFromRange(minNumberofColors, maxNumberofColors, myColorsNamesDistinct.Count);
    }


    public string EvaluateTone()
    {
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


    public void EvaluateColorScheme()
    {
        int nOfColors = myColorsNamesDistinct.Count;

        //check if monochromatic
        if(nOfColors == 1)
        {
            monochromatic = true;
            colorScheme = ColorScheme.Monochromatic;
        }else if (nOfColors == 2&&(myColorsNamesDistinct.Contains("black")||myColorsNamesDistinct.Contains("white")))
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
        if(nOfColors >= 3 && !myColorsNamesDistinct.Contains("black") && !myColorsNamesDistinct.Contains("gray") && !myColorsNamesDistinct.Contains("white"))
        {
            bool isSplit = false;
            foreach(string color in myColorsNamesDistinct)
            {
                foreach (string c in colortool.AnalogousOf(colortool.ComplementaryOf(color)))
                {
                    isSplit = false;
                    if (myColorsNamesDistinct.Contains(c))
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
        if (nOfColors == 3&&!myColorsNamesDistinct.Contains("black")&&!myColorsNamesDistinct.Contains("gray")&&!myColorsNamesDistinct.Contains("white"))
        {
            bool triad = true;
            foreach(string color in myColorsNamesDistinct)
            {
                if (myColorsNamesDistinct.Contains(colortool.ComplementaryOf(color)))
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
        if (nOfColors == 4 && !myColorsNamesDistinct.Contains("black") && !myColorsNamesDistinct.Contains("gray") && !myColorsNamesDistinct.Contains("white"))
        {
            bool triad = true;
            foreach (string color in myColorsNamesDistinct)
            {
                if (!myColorsNamesDistinct.Contains(colortool.ComplementaryOf(color)))
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
        if(myColorsNamesDistinct.Count == 2 || (myColorsNamesDistinct.Count == 3 && (myColorsNamesDistinct.Contains("black") || myColorsNamesDistinct.Contains("white") || myColorsNamesDistinct.Contains("gray"))))
        {
            foreach(GameObject go in gameObjects)
            {
                //Analogous of complementary works too, for a more forgiving evaluation
                List<string> simlarColors = colortool.AnalogousOf(go.GetComponent<Image>().color);
                foreach(string color in simlarColors)
                {
                    if (myColorsNamesDistinct.Contains(colortool.ComplementaryOf(color)))
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }
    

    //to do: Map this to int 1-5 
    
    public float PercentageScore()
    {
        float totalPossibleScore = requiredColors.Count + bannedColors.Count;
        if (requiredcolorScheme != ColorScheme.None)
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


    public List <Color> GetColorsFromCanvas(List<GameObject> go)
    {
        List<Color> colors = new List<Color>();
        foreach (GameObject elem in go)
        {
            colors.Add(elem.GetComponent<Image>().color);
        }
        return colors;
    }


    public List<string> GetStringsFromColors(List<Color> colors)
    {
        List<string> colorNames = new List<string>();
        foreach(Color color in colors)
        {
            colorNames.Add(colortool.ColorName(color));
        }
        return colorNames;

    }


}
