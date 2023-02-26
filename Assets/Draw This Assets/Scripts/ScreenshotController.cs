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
using System;

public class ScreenshotController : MonoBehaviour
{
    
    public void takeScreenShot()
    {
        StartCoroutine(processScreenShot());
    }
    private IEnumerator processScreenShot()
    {

        Canvas canvas = FindObjectOfType<Canvas>();
        canvas.enabled = false;
        yield return new WaitForEndOfFrame();
        // save in desktop location
        ScreenCapture.CaptureScreenshot(System.Environment.GetFolderPath(
                                        System.Environment.SpecialFolder.Desktop) + "/" + stripSlashesColumnsAndSpacesAway(DateTime.Now.ToString()) + ".png");
        canvas.enabled = true;
        yield return null;
    }

    // remove slashes from a string
    private string stripSlashesColumnsAndSpacesAway(string word)
    {
        string formatted_word = word.Replace("/", "");
        formatted_word = formatted_word.Replace(" ", "");
        formatted_word = formatted_word.Replace(":", "");

        return formatted_word;
    }

}
