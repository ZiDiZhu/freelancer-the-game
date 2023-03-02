using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SprytFrameList", menuName = "Spryt Animator Pro/Spryt (Frame List)", order = 1)]
///<summary>A Scriptable Object which is a List of Sprites. Used by MultiSpryt to switch between sets of frames.
///<para>Once created, a “Spryt” can be used throughout the project, or even between projects.</para></summary>
public class Spryt : ScriptableObject
{
    public List<Sprite> frames = new List<Sprite>();
}