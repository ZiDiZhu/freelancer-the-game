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

public class TextTool : MonoBehaviour
{

    public TMPro.TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        TextEditorController.Instance.openUpEditor(this);
    }

}
