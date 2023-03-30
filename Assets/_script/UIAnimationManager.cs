using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script that controls animating UI stuff
public class UIAnimationManager : MonoBehaviour
{
    public UISpriteAnimator playerSprite;//moves when you move
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            playerSprite.isAnimating = true;
        }
        else
        {
            playerSprite.isAnimating = false;
        }
    }
}
