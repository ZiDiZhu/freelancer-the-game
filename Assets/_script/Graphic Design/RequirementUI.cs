using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using Util;
using ColorUtil;

//displays UI from DesignRequirement, should attach to same GO
public class RequirementUI : MonoBehaviour
{
    public Image clientProfile,playerProfile;

    //Other Scripts attached to this gameObject
    private DesignRequirement dR; //must be on same gameObject

    public GameObject mustIncludeColorsUI;
    public GameObject doNotIncludeColorsUI;
    public GameObject numberOfColorsUI;
    public GameObject requiredColorSchemeUI;
    public TMP_Text SubmitButtonText;
    public Button submitButton;

    public TMP_Text missingColorsText, wrongColorsText, numberDifferenceText, requiredColorSchemeText, readabilityText;

    public TMP_Text colorSchemeText,toneText;

    public float okTextFontSize = 50f;
    public float normalTextFontSize = 25f;

    public GameObject endScreen; //shows once Submitted.
    public TMP_Text resultText;
    public Slider starsSlider;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {

        dR = GetComponent<DesignRequirement>();
        endScreen.SetActive(false);
        submitButton.interactable = true;
        submitButton.onClick.AddListener(() => UpdateEndScreen());
        InitializeRequirementList();
        UpdateRequirement();
        clientProfile.sprite = dR.commissionObject.client.pfp;
    }

    public void UpdateEndScreen()
    {
        endScreen.SetActive(true);
        int starsScore = MathUtils.PecentageToOutOfFive(dR.PercentageScore());
        if (starsScore == 5)
        {
            
        }
        starsSlider.value = (0.2f*(float)starsScore);
        resultText.text = MathUtils.PecentageToOutOfFive(dR.PercentageScore())+" Stars!";
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
            if (missingColorsText != null)
            {
                ChangeText(missingColorsText, txt, normalTextFontSize, Color.white);
            }
        }

        //no wrong colors
        if (dR.wrongColors.Count == 0&&dR.bannedColors.Count!=0)
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
            int n = dR.missingNumberOfColors;

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
            doNotIncludeColorsUI.SetActive(true);
            string txt = "Do Not Include: \n";
            foreach (string color in dR.bannedColors)
            {
                txt += color + " ";
            }
            doNotIncludeColorsUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = txt;
            wrongColorsText = doNotIncludeColorsUI.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        }
        else
        {
            doNotIncludeColorsUI.SetActive(false);
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
            requiredColorSchemeUI.SetActive(false);
        }

    }

    public void Readability(bool readable)
    {
        if (readable)
        {
            SetEvalTextToOk(readabilityText);
        }
        else
        {
            ChangeText(readabilityText, "Visuals are cluttered", normalTextFontSize, Color.white);
        }
    }


    public void SetEvalTextToOk(TMP_Text tmpText)
    {
        ChangeText(tmpText, "OK", okTextFontSize, Color.green);
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
