using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Util;
using ColorUtil;

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
        
        designControl = GetComponent<DesignControl>();
        designControl.Initialize();
        canvasElements = designControl.canvasElements;
        AssignRequirementsFromCommissionObject(commissionObject);

        requirementUI = GetComponent<RequirementUI>();
    }

    
    // Start is called before the first frame update
    void Start()
    {

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
            if (ColorInfo.ToneOf(color) == "warm")
            {
                temp++;
            }
            else if (ColorInfo.ToneOf(color) == "cool")
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

        monochromatic = ColorCombination.IsMonoChromatic(myColorsNamesDistinct);
        if (monochromatic)
        {
            colorScheme = ColorScheme.Monochromatic;
        }
        //check if analogous : 2-4 colors next to each other
        analogous = ColorCombination.IsAnalogous(myColorsNamesDistinct);
        if (analogous)
        {
            colorScheme = ColorScheme.Analogous;
        }
        complementary = ColorCombination.IsComplementary(myColorsNamesDistinct);
        if (complementary)
        {
            colorScheme = ColorScheme.Complementary;
        }
        if (!complementary && !analogous && !monochromatic)
        {
            colorScheme = ColorScheme.None;
        }

        bool isSplit = ColorCombination.IsSplitComplementary(myColorsNamesDistinct);
        if (isSplit)
        {
            colorScheme = ColorScheme.SplitComplementary;
        }


        bool isTriadic = ColorCombination.IsTriadic(myColorsNamesDistinct);
        if (isTriadic)
        {
            colorScheme = ColorScheme.Triadic;
        }

        //check if is tetradic
        if (nOfColors == 4 && !myColorsNamesDistinct.Contains("black") && !myColorsNamesDistinct.Contains("gray") && !myColorsNamesDistinct.Contains("white"))
        {
            bool triad = true;
            foreach (string color in myColorsNamesDistinct)
            {
                if (!myColorsNamesDistinct.Contains(ColorInfo.GetComplementaryHueString(color)))
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
            colorNames.Add(ColorInfo.GetHueString(color));
        }
        return colorNames;

    }


}
