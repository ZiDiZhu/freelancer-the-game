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
using System.IO;



public class BackgroundImageChooserPanel : MonoBehaviour
{

    [SerializeField]
    private FileChooserPanel file_chooser_prefab;


    [SerializeField]
    private Toggle background_image_toggle;


    [SerializeField]
    private Button select_image_button;


    [SerializeField]
    private MeshRenderer background_image;



    // Start is called before the first frame update
    void Start()
    {
        select_image_button.onClick.AddListener(() => openFileChooser());

        background_image.gameObject.SetActive(background_image_toggle.isOn);

        background_image_toggle.onValueChanged.AddListener(delegate 
        { 
            background_image.gameObject.SetActive(background_image_toggle.isOn); 
        });
    }

    private void openFileChooser()
    {
        FileChooserPanel file_chooser = Instantiate(file_chooser_prefab);

        file_chooser.transform.SetParent(FindObjectOfType<Canvas>().GetComponent<RectTransform>(), false);

        file_chooser.ok_button.onClick.AddListener(() => loadImage(file_chooser.chosen_file, file_chooser.gameObject));

        file_chooser.cancel_button.onClick.AddListener(() => Destroy(file_chooser.gameObject));

    }


    private void loadImage(string file,  GameObject to_destroy)
    {
        byte[] image_in_bytes = File.ReadAllBytes(file);

        Texture2D image = new Texture2D(1, 1, TextureFormat.ARGB32, false);

        image.LoadImage(image_in_bytes);

        image.Apply();

        background_image.material.mainTexture = image;

        Destroy(to_destroy);
    }
}
