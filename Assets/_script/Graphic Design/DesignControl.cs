using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//Action script for the design app. Replace old script "GraphicShape"
public class DesignControl : MonoBehaviour
{
    [SerializeField] private DesignRequirement designRequirement; //manages requirements, attached to to same gameobject as this script, "Design manager"

    public GameObject canvasGroup; //contains all canvas elements
    public List<GameObject> canvasElements; //things in your canvas

    public Image colorReference; //the current color on Color Picker

    public Button dragBtn, bucketBtn; //delete btn to be added

    public Texture2D dragCursor,bucketCursor;

    //mode of interaction with shape. The corresponding button should be highlighted
    
    public enum InteractionMode
    {
        Drag, //drag and drop the shape on the canvas. Disable for template
        Bucket, //colors the shape
        Delete //deletes the shape. Disable for templates
    }
    public InteractionMode currentMode = InteractionMode.Bucket;

    private void Awake()
    {
        designRequirement = gameObject.GetComponent<DesignRequirement>();
        InitializeButtons();
        InitializeCanvasElements(canvasGroup);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //link buttons to callbacks
    private void InitializeButtons()
    {
        dragBtn.onClick.AddListener(() => DragBtnClicked());
        bucketBtn.onClick.AddListener(() => BucketBtnClicked());
    }

    //Link canvas elements to callback. (has to be clickable) 
    public void InitializeCanvasElements(GameObject canvasGroup)
    {
        canvasElements.Clear();
        for(int i = 0; i < canvasGroup.transform.childCount; i++)
        {
            int n = i;
            canvasElements.Add(canvasGroup.transform.GetChild(n).gameObject);
        }
        for (int i = 0; i < canvasElements.Count; i++)
        {
            int n = i;//to prevent variable capturing
            canvasElements[n].GetComponent<Button>().onClick.AddListener(() => CanvasElementClicked(canvasElements[n]));//so that clicking on button triggers a callback
        }

    }

    
    public void changeCursor(bool isOn)
    {
        if(isOn == true)
        {
            if (currentMode == InteractionMode.Bucket)
            {
                Cursor.SetCursor(bucketCursor, Vector2.zero, CursorMode.Auto);
            }
            else if (currentMode == InteractionMode.Drag)
            {
                Cursor.SetCursor(dragCursor, Vector2.zero, CursorMode.Auto);
            }
        }else if(isOn == false)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        
    }


    //calls when drag (move) button gets clicked
    void DragBtnClicked()
    {
        currentMode = InteractionMode.Drag;
    }
    void BucketBtnClicked()
    {
        currentMode = InteractionMode.Bucket;
    }

    void CanvasElementClicked(GameObject elem)
    {
        if(currentMode == InteractionMode.Bucket)
        {
            //set clicked element color to the colorpicker color
            elem.GetComponent<Image>().color = colorReference.color;
        }

        designRequirement.Evaluate();
        
    }


}
