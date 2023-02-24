using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Commission", menuName = "New Commission")]
public class CommissionObject : ScriptableObject
{
    //public string description;
    public ClientObject client;
    public double pay;

    //public string[] openingDialogues;

    public List<string> mustIncludeColors,doNotIncludeColors;
    public int minNumberColors, maxNumberColors;//set to -1 means no restriction

    public DesignRequirement.ColorScheme requiredColorScheme;

}
