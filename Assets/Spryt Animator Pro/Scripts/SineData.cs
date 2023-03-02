using UnityEngine;

[CreateAssetMenu(fileName = "SineData", menuName = "Spryt Animator Pro/Sine Effect Data", order = 3)]
///<summary>A Scriptable Object which holds all the data for an instance of the Sine Class.
///<para>This can be used to create a Sine effect which can be re-used throughout the project, or even between projects.</para></summary>
public class SineData : ScriptableObject
{
    ///<summary>The divisor which controls how large of small a "slice of Pi" which is added to or subtracted from "index".
    ///<para>A larger divisor results in a slower effect, a smaller divisor results in a faster effect.</para></summary>
    public float divisor = 60f;

    ///<summary>A multiplier on the value of Index which is usually bounded between -1 to 1.</summary>
	public float magnitude = 1f;

    ///<summary>Measured in increments of Pi. A complete Sine curve has a duration of 2f while a quarter is 0.5f.
    ///<para>If the effect is supposed to have limited duration (instead of continuing forever) set this to a non-zero number.</para></summary>
	public float duration = 0f;

    ///<summary>The actual value which tracks where the effect is between a range of 0 to 2*Pi.</summary>
	public float index = 0f;

    ///<summary>A boolean to track whether the effect's duration has elapsed, if it had a fixed duartion.
    ///<para>Set this to false if you want an effect to play immediately.</para></summary>
    public bool effectActive = false;

    ///<summary>If using the expedited SineUpdate method, this controls the operation passed on the ref float.</summary>
    public Sine.Type updateType = Sine.Type.SineDuration;

    ///<summary>If using the Sine's Update method, this controls whether the index will count up to its maximum value or down to zero.</summary>
    public bool countDown = false;
}