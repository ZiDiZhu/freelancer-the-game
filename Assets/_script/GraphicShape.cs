using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//temp, test for a "graphical template"
public class GraphicShape : MonoBehaviour
{
    public GameObject[] shape;//shapes in a template, which are buttons
    public GameObject selectedShape;

    private void Start()
    {
        for(int i = 0; i < shape.Length; i++)
        {
            int n = i;//to prevent variable capturing
            shape[n].GetComponent<Button>().onClick.AddListener(() => buttonCallBack(shape[n]));//so that clicking on button triggers a callback
        }

    }


    private void buttonCallBack(GameObject myShape)
    {
        Debug.Log("Pressed button" + myShape.name);
        if (selectedShape == myShape)
        {
            selectedShape = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
            
        else
            selectedShape = myShape;
    }



}
