using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]

public struct Asset 
{
    public string name;
    public double amount;
    public TMP_Text tmp_name;
    public TMP_Text tmp_amount;

    public Asset(string n, double a)
    {
        name = n;
        amount = a; 
        tmp_name = null;
        tmp_amount = null;
    }

    public void updateText()
    {
        tmp_name.text = amount + "";
        tmp_amount.text = amount + "";
    }

    public void AssignTextMesh(TMP_Text tname, TMP_Text tamount)
    {
        tmp_name = tname;
        tmp_amount = tamount;
    }

}

[System.Serializable]
public struct Liability
{
    public string name;
    public double amount;
    public TMP_Text tmp_name;
    public TMP_Text tmp_amount;
    public Liability(string n, double a)
    {
        name = n;
        amount = a;
        tmp_name = null;
        tmp_amount = null;
    }

    public void updateText()
    {
        tmp_name.text = amount + "";
        tmp_amount.text = amount+"";
    }

    public void AssignTextMesh(TMP_Text tname, TMP_Text tamount)
    {
        tmp_name = tname;
        tmp_amount = tamount;
    }
}

public class BalanceSheet : MonoBehaviour
{
    [SerializeField] GameObject balanceSheetObject,assetsPanel,liabilitiesPanel;

    public List<Asset> assets;
    public GameObject assetHolder,liabilityHolder; // the text box prefab that contains 2 text meshes to display namew and amount
    [SerializeField] List<GameObject> aHolders, lHolders; //keep track of transform position

    public double networth;
    public TMP_Text tmp_networth;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Asset asset in assets)
        {
            Transform last_a = aHolders[aHolders.Count - 1].transform;
            Transform last_l = lHolders[aHolders.Count - 1].transform;

            float x = last_a.position.x;
            float y= last_a.position.y -50;
            float z = last_a.position.z;
            
            GameObject aHolder = Instantiate(assetHolder,new Vector3(x,y,z),Quaternion.identity,assetsPanel.transform);

            x = last_l.position.x;
            y = last_l.position.y - 50;
            z = last_l.position.z;
            GameObject lHolder = Instantiate(liabilityHolder, new Vector3(x, y, z), Quaternion.identity, assetsPanel.transform);

            asset.AssignTextMesh(aHolder.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), aHolder.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>());
            asset.updateText();
            aHolders.Add(aHolder);
        }

        foreach(Asset a in assets) { networth += a.amount; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
