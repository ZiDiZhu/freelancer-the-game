using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketManager : MonoBehaviour
{
    public List<Asset> items; //sellable non-consumable items
    public GameObject itemHolder; //template
    [SerializeField] List<GameObject> iHolders; //first one is template! (do not delete)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
