using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Stat
{
    public string name;
    public double value;
    public double maxValue;
    public Stat(string n, double v, double m)
    {
        name = n;
        value = v;
        maxValue = m;
    }
}

public class PlayerStats : MonoBehaviour
{

    public List<Stat> stats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
