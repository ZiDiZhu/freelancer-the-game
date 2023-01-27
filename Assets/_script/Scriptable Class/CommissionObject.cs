using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Commission", menuName = "New Commission")]
public class CommissionObject : ScriptableObject
{
    public string title;
    public string description;
    public string clientName;
    public double pay;

    public string[] openingDialogues;

    public bool[] fulfilled;
    public string[] requirements;
}
