using UnityEngine;
using UnityEngine.EventSystems;
public class OneShotOnMouseOver : MonoBehaviour, IPointerEnterHandler
{
	public SprytSingle sSprite;
		
	public void OnPointerEnter(PointerEventData evd) {
		sSprite.PlayOneShot(false);
	}
}