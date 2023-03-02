using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SprytSineData", menuName = "Spryt Animator Pro/Spryt Sine Effect Data", order = 2)]
///<summary>A Scriptable Object which holds all the data for the x / y, x / y Scale, and Angle Sine effects used by Spryt.
///<para>This can be used to create a batch of Sine effects which can be re-used throughout the project, or even between projects.</para></summary>
public class SSineData : ScriptableObject
{
    public Sine xSine;
    public Sine ySine;
    public Sine xScaleSine;
    public Sine yScaleSine;
    public Sine angleSine;
}