/*
 * 
 * Developed by Olusola Olaoye, 2021
 * 
 * To only be used by those who purchased from the Unity asset store
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEditorPanel : MonoBehaviour
{
    private TextTool chosen_text_tool;

    [SerializeField]
    private Button close_button;

    [SerializeField]
    private TMPro.TMP_InputField text_input;

    [SerializeField]
    private Button[] color_buttons;


    public void openUpEditor(TextTool text)
    {
        chosen_text_tool = null;

        chosen_text_tool = text;

        text_input.text = chosen_text_tool.text.text;


        text_input.onValueChanged.RemoveAllListeners();
        text_input.onValueChanged.AddListener(
            delegate
            {
                chosen_text_tool.text.text = text_input.text; // chosen text should update every change in value
            });


        foreach(Button color_button in color_buttons)
        {
            color_button.onClick.RemoveAllListeners();
            color_button.onClick.AddListener(() => chosen_text_tool.text.color = color_button.GetComponent<Image>().color); // channge color of chosen text
        }

        close_button.onClick.RemoveAllListeners();
        close_button.onClick.AddListener(() => gameObject.SetActive(false));
    }


}
