using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Asset", menuName = "New Asset")]
public class AssetObject : ScriptableObject
{
    public string name;
    public double value;
    public bool sellable;
    public Sprite picture;
}
