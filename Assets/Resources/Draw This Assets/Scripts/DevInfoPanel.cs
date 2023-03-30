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

public class DevInfoPanel : MonoBehaviour
{
    public void toggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
