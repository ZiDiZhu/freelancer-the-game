using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct Record
{
    int stars; //out of 5

}

//draft script for player's score-tracking
public class PlayerStats : ScriptableObject
{

    public List<Commission> pastCommissions;

    public int wallet;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteLevel()
    {

    }

    public void ResetSave()
    {

    }

}
