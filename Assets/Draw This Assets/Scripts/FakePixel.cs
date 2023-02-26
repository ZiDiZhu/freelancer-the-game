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

public class FakePixel : MonoBehaviour
{

    [SerializeField]
    private Shader default_Shader;

    // Start is called before the first frame update
    void Start()
    {
        // create new material
        GetComponent<MeshRenderer>().material = new Material(default_Shader);

        // set material color to the chosen color in the draw engine class
        GetComponent<MeshRenderer>().material.color = FindObjectOfType<DrawEngine>().chosen_color;
    }

}
