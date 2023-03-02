using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtMouse : MonoBehaviour
{
    public SprytSingle sprite;

    void Update() {
        sprite.Angle = DirectionToMouse();
    }

    /// <summary>
    /// Return the direction between two vectors. Yes, I know "Vector2.SignedAngle" exists, but this seems more reliable
    /// </summary>
    /// <param name="_a">The first vector</param>
    /// <param name="_b">The second vector</param>
    /// <returns></returns>
    public float Direction(Vector2 _a, Vector2 _b) {
        return Mathf.Rad2Deg*Mathf.Atan2(_a.y - _b.y, _a.x - _b.x);
    }
    /// <summary>
    /// Returns the direction from the current gameObject to the Mouse
    /// </summary>
    /// <returns>A float which is between -180 to 180</returns>
    public float DirectionToMouse() {
        return Direction(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
    }
}