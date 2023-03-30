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
using HSVPicker;

public class ColorChooserPanel : MonoBehaviour
{
    [SerializeField]
    private DrawEngine draw_engine; // reference to the draw engine

    [SerializeField]
    private Image display_color_image;


    [SerializeField]
    private Slider red_slider_color;

    [SerializeField]
    private Slider green_slider_color;

    [SerializeField]
    private Slider blue_slider_color;

    public ColorPicker colorPicker;

    // Start is called before the first frame update
    void Start()
    {
        updateColor();

        //red_slider_color.onValueChanged.AddListener(delegate { updateColor(); });
        //green_slider_color.onValueChanged.AddListener(delegate { updateColor(); });
        //blue_slider_color.onValueChanged.AddListener(delegate { updateColor(); });
       
    }

    public void updateColor()
    {
        //Color color = new Color(red_slider_color.value,
        //                                     green_slider_color.value,
        //                                     blue_slider_color.value,
        //                                     1);

        draw_engine.chosen_color = colorPicker.CurrentColor;

        display_color_image.color = colorPicker.CurrentColor;
    }

    public void toggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
