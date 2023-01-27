using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Client", menuName = "New Client")]
public class ClientObject : ScriptableObject
{
    public string name;
    public string title;
    public Sprite pfp;
    public CommissionObject[] commissionObjects;
}
