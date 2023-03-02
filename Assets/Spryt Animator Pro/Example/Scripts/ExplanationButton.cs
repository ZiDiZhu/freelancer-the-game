using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExplanationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [TextArea] public string myText;
    public Text textField;
    
    private SprytSingle sSprite;
	private bool mouseOver;
//Do a bit of a hack to compensate for Unity Layout group
    private int recalibrateOneFrameLater = 1;
    private bool doRecalibration = true;

    void Start() {
        sSprite = GetComponent<SprytSingle>();
    }

    private void Update() {
        if (doRecalibration && recalibrateOneFrameLater-- == 0) {
    //Do a bit of a hack to compensate for Unity Layout group
            sSprite.PositionOriginRecalibrate();
            doRecalibration = false;
        }
    }

    public void OnPointerEnter(PointerEventData evd) {
		sSprite.Frame = 1f; //Switch to highlighted state
        sSprite.ActivateSine(BaseSpryt.SineType.y, false);
        mouseOver = true;
	}

	public void OnPointerExit (PointerEventData evd) {
		sSprite.Frame = 0f; //Go back to normal
        sSprite.ActivateSineRotation();
        mouseOver = false;
	}
		
	public void OnPointerClick (PointerEventData evd) {
        textField.text = myText;
        sSprite.ActivateSineScale();
	}

    public void OnPointerDown(PointerEventData eventData) {
        sSprite.Frame = 2f; //Indicate click has occured
    }

    public void OnPointerUp(PointerEventData eventData) {
        sSprite.Frame = mouseOver ? 1f : 0f; //Go back to highlighted state or neutral state depending on whether the mouse is still over the button
    }
}