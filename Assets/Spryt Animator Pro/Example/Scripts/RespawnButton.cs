using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RespawnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	public GameObject playerPrefab;
    public SprytSingle sSprite;

	private bool mouseOver;
    private GameObject spawnedPlayer;

	void Start() {
//Find the player in the Scene since one already exists
        spawnedPlayer = FindObjectOfType<Player>().gameObject;
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
        if (spawnedPlayer != null) {
            Destroy(spawnedPlayer);
        }
        spawnedPlayer = Instantiate(playerPrefab, null, true);
        sSprite.ActivateSineScale();
	}

    public void OnPointerDown(PointerEventData eventData) {
        sSprite.Frame = 2f; //Indicate click has occured
    }

    public void OnPointerUp(PointerEventData eventData) {
        sSprite.Frame = mouseOver ? 1f : 0f; //Go back to highlighted state or neutral state depending on whether the mouse is still over the button
    }
}