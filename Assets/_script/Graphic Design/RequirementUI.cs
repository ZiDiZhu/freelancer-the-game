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
    public DesignRequirement dR;

    public GameObject mustIncludeColorsUI, banColorUI, numberOfColorsUI, requiredColorSchemeUI;

    private TMP_Text missingColorsText, wrongColorsText, numberDifferenceText, requiredColorSchemeText;

    public TMP_Text scoreText,SubmitButtonText;
    public Button submitButton;

    public TMP_Text colorSchemeText,toneText;

    public float okTextFontSize = 50f;
    public float normalTextFontSize = 25f;

    public GameObject endScreen; //shows once Submitted.
    public TMP_Text resultText;

    // Start is called before the first frame update
    void Awake()
    {
        dR = GetComponent<DesignRequirement>();
        endScreen.SetActive(false);
        submitButton.onClick.AddListener(() => UpdateEndScreen());
        UpdateRequirement();
    }

    public void UpdateEndScreen()
    {
        endScreen.SetActive(true);
        if (dR.score == 1)
        {
            resultText.text = "Perfect Job!";
        }else if (dR.score <= 1 && dR.score >= 0.6)
        {
            resultText.text = "OK job!";
        }
        else
        {
            resultText.text = "That Was BAD!";
        }
    }

    public void UpdateRequirement()
    {
        dR.Evaluate();
        if (dR.requiredColors.Count!=0&& dR.missingColors.Count == 0)
        {
            ChangeText(missingColorsText, "OK", okTextFontSize, Color.green);
        }
        else if(dR.requiredColors.Count != 0 && dR.missingColors.Count != 0)
        {
            string txt = "Missing: \n";
            foreach (string color in dR.missingColors)
            {
                txt += " " + color;
            }
            if(missingColorsText!=null)
                ChangeText(missingColorsText, txt, normalTextFontSize, Color.white);
        }

        //no wrong colors
        if (dR.wrongColors.Count == 0)
        {
            ChangeText(wrongColorsText, "OK", okTextFontSize, Color.green);
        }
        else
        {
            string txt = "Currently contains: \n";
            foreach (string color in dR.wrongColors)
            {
                txt += " " + color;
            }
            ChangeText(wrongColorsText, txt, normalTextFontSize, Color.white);
        }
        if(dR.minNumberofColors!= -1 && dR.maxNumberofColors!= -1)
        {
            int n = dR.GetOutOfRangeNumber(dR.minNumberofColors, dR.maxNumberofColors);
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

        colorSchemeText.text ="Color Scheme: "+ dR.colorScheme.ToString();
        toneText.text = "Tone: " + dR.EvaluateTone();
        scoreText.text = "Score: "+dR.score*100 + "%";

        //if fits the color scheme
        if (dR.requiredcolorScheme != DesignRequirement.ColorScheme.None)
        {
            if (dR.colorScheme == dR.requiredcolorScheme)
            {
                ChangeText(requiredColorSchemeText, "OK", okTextFontSize, Color.green);
            }
            else
            {
                ChangeText(requiredColorSchemeText, "Current Color Scheme: " + dR.colorScheme.ToString(), normalTextFontSize, Color.white);
            }
        }
        

        if (dR.score < 0.6)
        {
            submitButton.interactable = false;
        }
        else
        {
            submitButton.interactable = true;
            if (dR.score == 1)
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
        if (dR.requiredColors.Count != 0) //has contain color requirement
        {
            mustIncludeColorsUI.SetActive(true);
            string txt = "Must Include: \n";
            foreach (string color in dR.requiredColors)
            {
                txt += color + " ";
            }
            mustIncludeColorsUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;
            missingColorsText = mustIncludeColorsUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }
        else
        {
            mustIncludeColorsUI.SetActive(false);
        }

        // "Do Not Include"
        if (dR.bannedColors.Count != 0) //has contain color requirement
        {
            banColorUI.SetActive(true);
            string txt = "Do Not Include: \n";
            foreach (string color in dR.bannedColors)
            {
                txt += color + " ";
            }
            banColorUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;
            wrongColorsText = banColorUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }
        else
        {
            mustIncludeColorsUI.SetActive(false);
        }
        if (dR.minNumberofColors == -1 && dR.maxNumberofColors == -1)
        {
            numberOfColorsUI.SetActive(false);
        }
        else
        {
            string txt = "";
            if(dR.minNumberofColors == dR.maxNumberofColors)
            {
                txt += "Must contain " + dR.minNumberofColors + " distinct colors";
            }else if (dR.minNumberofColors != -1)
            {
                txt += "At least " + dR.minNumberofColors+ " distinct colors ";
            }
            else if (dR.maxNumberofColors != -1)
            {
                txt += "At most " + dR.maxNumberofColors + " distinct colors ";
            }
            numberOfColorsUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;

            numberDifferenceText = numberOfColorsUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }

        if (dR.requiredcolorScheme != DesignRequirement.ColorScheme.None)
        {
            requiredColorSchemeUI.SetActive(true);
            requiredColorSchemeText = requiredColorSchemeUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            requiredColorSchemeUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Required Color scheme: \n" + dR.requiredcolorScheme;
        }
        else
        {
            //requiredColorSchemeUI.SetActive(false);
        }

    }


    private void ChangeText(TMP_Text tmpText,string txt,float size,Color bgColor)
    {
        //if changed status
        if(tmpText!=null&&tmpText.text!=txt)
        {
            if (tmpText.text != txt)
            {
                float duration = 0.1f;
                float intensity = 0.1f;
                if (size != tmpText.fontSize)
                {
                    duration = 0.3f;
                    intensity = 0.3f;
                }
                tmpText.transform.DOShakeScale(duration, intensity);

                tmpText.text = txt;
                tmpText.fontSize = size;
                tmpText.transform.parent.GetComponent<Image>().color = bgColor;
            }
            
        }

        
    }

}
