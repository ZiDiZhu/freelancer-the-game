using System.Collections.Generic;
using UnityEngine;

///<summary>To be used on animated instance which will only ever use a single set of frames, SprytSingle provides a simple alternative to MultiSpryt which does not require the creation of Spryt objects.
///<para>Instead, a List of Sprites can be added directly to the Component in the Inspector.</para></summary>
public class SprytSingle : BaseSpryt
{
    ///<summary>A List of Sprites used in the Start method of SprytSingle to define its animation.</summary>
    public List<Sprite> spriteFrames = new List<Sprite>();

    public override void Start() {
        base.Start(); //Call the Start method in BaseSpryt to handle setting all private members
        if (spriteFrames.Count == 0) {
    //If no frames have been assigned to the animation, pull the Sprite off the renderer
            spriteFrames.Add(myRenderer.sprite);
        }
        AssignFrames(spriteFrames); //Assign the frames in the spriteFrames List which should be set in the Inspector
    }
}