using UnityEngine;
using UnityEngine.UI;

public class SampleController : MonoBehaviour
{
//Spryt
    public MultiSpryt mSprite;

    public float _speed;
    /// <summary>
    /// Speed is created as a property here so it can be manipulated by the Slider in the Canvas UI
    /// </summary>
    public float Speed { get { return _speed; } set { _speed = value; mSprite.speed = _speed; } }

//Swap between 2 sample Spryts; Called by button press in UI
    public Spryt sprite1;
    public Spryt sprite2;
    private bool sprytIsSwapped = false;
    public void SwapSpryt() {
        mSprite.Index = sprytIsSwapped ? sprite1 : sprite2;
        sprytIsSwapped = !sprytIsSwapped;
    }
    
//UI
    public Dropdown dropdown;
    public void SetColor(int _newColor) {
//Trigger the animation by resetting the effect; Called by Drop-down selection in UI
        switch(_newColor) {
            case 0: mSprite.Color = Color.white; break;
            case 1: mSprite.Color = Color.red; break;
            case 2: mSprite.Color = Color.green; break;
            case 3: mSprite.Color = Color.blue; break;
        }
    }
}