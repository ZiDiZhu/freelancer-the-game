using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//displays UI from DesignRequirement, should attach to same GO
public class RequirementUI : MonoBehaviour
{
    public DesignRequirement designRequirement;

    public GameObject containColorUI, banColorUI, numberOfColorsUI;

    public TMP_Text missingColorsText, wrongColorsText, numberDifferenceText;

    public TMP_Text scoreText,SubmitButtonText;

    
    // Start is called before the first frame update
    void Start()
    {
        designRequirement = GetComponent<DesignRequirement>();
        InitializeRequirementList();
    }

    public void UpdateRequirement()
    {
        designRequirement.Evaluate();
        if (designRequirement.missingColors.Count == 0)
        {
            missingColorsText.text = "OK";
        }
        else
        {
            missingColorsText.text = "Missing: \n";
            foreach (string color in designRequirement.missingColors)
            {
                missingColorsText.text += " " + color;
            }
        }
        if (designRequirement.wrongColors.Count == 0)
        {
            wrongColorsText.text = "OK";
        }
        else
        {
            wrongColorsText.text = "Currently contains: \n";
            foreach (string color in designRequirement.wrongColors)
            {
                wrongColorsText.text += " " + color;
            }
        }
        
    }

    public void InitializeRequirementList()
    {
        
        // "Must include"
        if (designRequirement.requiredColors.Count != 0) //has contain color requirement
        {
            containColorUI.SetActive(true);
            string txt = "Must Include: \n";
            foreach (string color in designRequirement.requiredColors)
            {
                txt += color + " ";
            }
            containColorUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;
            missingColorsText = containColorUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }
        else
        {
            containColorUI.SetActive(false);
        }

        // "Do Not Include"
        if (designRequirement.requiredColors.Count != 0) //has contain color requirement
        {
            banColorUI.SetActive(true);
            string txt = "Do Not Include: \n";
            foreach (string color in designRequirement.bannedColors)
            {
                txt += color + " ";
            }
            banColorUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;
            wrongColorsText = banColorUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }
        else
        {
            containColorUI.SetActive(false);
        }
        if (designRequirement.minNumberofColors == -1 && designRequirement.maxNumberofColors == -1)
        {
            numberOfColorsUI.SetActive(false);
        }
        else
        {
            string txt = "";
            if(designRequirement.minNumberofColors == designRequirement.maxNumberofColors)
            {
                txt += "Must contain " + designRequirement.minNumberofColors + " distinct colors";
            }else if (designRequirement.minNumberofColors != -1)
            {
                txt += "At least " + designRequirement.minNumberofColors+ " distinct colors ";
            }
            else if (designRequirement.maxNumberofColors != -1)
            {
                txt += "At most " + designRequirement.maxNumberofColors + " distinct colors ";
            }
            banColorUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;

            numberDifferenceText = numberOfColorsUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }

        UpdateRequirement();
    }

}
