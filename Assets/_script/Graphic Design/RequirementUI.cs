using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

//displays UI from DesignRequirement, should attach to same GO
public class RequirementUI : MonoBehaviour
{
    public DesignRequirement designRequirement;

    public GameObject containColorUI, banColorUI, numberOfColorsUI, requiredColorSchemeUI;

    public TMP_Text missingColorsText, wrongColorsText, numberDifferenceText, requiredColorSchemeText;

    public TMP_Text scoreText,SubmitButtonText;
    public Button submitButton;

    public TMP_Text colorSchemeText,toneText;

    public float okTextFontSize = 50f;
    public float normalTextFontSize = 25f;

    // Start is called before the first frame update
    void Awake()
    {
        designRequirement = GetComponent<DesignRequirement>();
    }

    public void UpdateRequirement()
    {
        designRequirement.Evaluate();
        if (designRequirement.missingColors.Count == 0)
        {
            ChangeText(missingColorsText, "OK", okTextFontSize, Color.green);
        }
        else
        {
            string txt = "Missing: \n";
            foreach (string color in designRequirement.missingColors)
            {
                txt += " " + color;
            }
            ChangeText(missingColorsText, txt, normalTextFontSize, Color.white);
        }

        //no wrong colors
        if (designRequirement.wrongColors.Count == 0)
        {
            ChangeText(wrongColorsText, "OK", okTextFontSize, Color.green);
        }
        else
        {
            string txt = "Currently contains: \n";
            foreach (string color in designRequirement.wrongColors)
            {
                txt += " " + color;
            }
            ChangeText(wrongColorsText, txt, normalTextFontSize, Color.white);
        }
        if(designRequirement.minNumberofColors!= -1 && designRequirement.maxNumberofColors!= -1)
        {
            int n = designRequirement.GetOutOfRangeNumber(designRequirement.minNumberofColors, designRequirement.maxNumberofColors);
            string txt ="";
            if (n == 0)
            {
                ChangeText(numberDifferenceText, "OK!", okTextFontSize, Color.green);
            }
            else
            {
                if (n > 0)
                {
                    txt = "Missing " + n;
                }
                if (n < 0)
                {
                    txt = n + "too many";
                }
                ChangeText(numberDifferenceText, txt, normalTextFontSize, Color.white);
            }
        }

        colorSchemeText.text ="Color Scheme: "+ designRequirement.colorScheme.ToString();
        toneText.text = "Tone: " + designRequirement.EvaluateTone();
        scoreText.text = "Score: "+designRequirement.score*100 + "%";

        if (designRequirement.score < 0.6)
        {
            submitButton.interactable = false;
        }
        else
        {
            submitButton.interactable = true;
            if (designRequirement.score == 1)
            {
                //change button style
                submitButton.transform.parent.GetComponent<Image>().color = Color.green;
            }
            else
            {
                submitButton.GetComponent<Image>().color = Color.white;
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
        if (designRequirement.bannedColors.Count != 0) //has contain color requirement
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
            numberOfColorsUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;

            numberDifferenceText = numberOfColorsUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }

        if (designRequirement.requiredcolorScheme != DesignRequirement.ColorScheme.None)
        {
            requiredColorSchemeUI.SetActive(true);
            requiredColorSchemeText = requiredColorSchemeUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            requiredColorSchemeUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Required Color scheme: \n" + designRequirement.requiredcolorScheme;
        }
        else
        {
            requiredColorSchemeUI.SetActive(false);
        }

    }


    private void ChangeText(TMP_Text tmpText,string txt,float size,Color bgColor)
    {
        //if changed status
        if(tmpText.text!=txt)
        {
            float duration = 0.1f;
            float intensity = 0.1f;
            if (size != tmpText.fontSize)
            {
                duration = 0.3f;
                intensity = 0.3f;
            }
            tmpText.transform.DOShakeScale(duration, intensity);
        }

        tmpText.text = txt;
        tmpText.fontSize = size;
        tmpText.transform.parent.GetComponent<Image>().color = bgColor;
    }

}
