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

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private float minimum_zoom = 10;
    private float maximum_zoom = 20;


    private void Start()
    {
        zoomIn();
    }

    public void zoomIn()
    {
        GetComponent<Camera>().orthographicSize -= 1;
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize, minimum_zoom, maximum_zoom); // clamp camera size
    }

    public void zoomOut()
    {
        GetComponent<Camera>().orthographicSize += 1;
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize, minimum_zoom, maximum_zoom);
    }
}
