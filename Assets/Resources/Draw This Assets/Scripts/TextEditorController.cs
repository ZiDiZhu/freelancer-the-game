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

public class TextEditorController : MonoBehaviour
{
    public TextEditorPanel text_editor_panel;

    private static TextEditorController instance;

    public static TextEditorController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    public void openUpEditor(TextTool text)
    {
        text_editor_panel.gameObject.SetActive(true);
        text_editor_panel.openUpEditor(text);

    }

}
