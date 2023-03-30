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

public class SizeChooserPanel : MonoBehaviour
{
    [SerializeField]
    private Slider size_slider;

    // Start is called before the first frame update
    void OnEnable()
    {
        size_slider.onValueChanged.RemoveAllListeners();

        size_slider.onValueChanged.AddListener(delegate
        {
            // change the chosen size value in the draw engine class to the slider value
            FindObjectOfType<DrawEngine>().chosen_size = size_slider.value;
        });
    }
    public void toggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
