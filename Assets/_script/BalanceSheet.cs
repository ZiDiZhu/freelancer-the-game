using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Asset 
{
    public string name;
    public double amount;
    public TMP_Text tmp_name;
    public TMP_Text tmp_amount;

    public bool sellable;

    public Asset(string n, double a)
    {
        name = n;
        amount = a; 
    }

    public void updateText()
    {
        tmp_name.text = name + "";
        tmp_amount.text = amount + "";
    }

    public void AssignTextMesh(TMP_Text tname, TMP_Text tamount)
    {
        tmp_name = tname;
        tmp_amount = tamount;
    }

}

[System.Serializable]
public class Liability
{
    public string name;
    public double amount;
    public TMP_Text tmp_name;
    public TMP_Text tmp_amount;
    public Liability(string n, double a)
    {
        name = n;
        amount = a;
    }

    public void updateText()
    {
        tmp_name.text = name + "";
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
    public List<Liability> liabilities;
    public GameObject assetHolder,liabilityHolder; // the text box prefab that contains 2 text meshes to display namew and amount
    [SerializeField] List<GameObject> aHolders, lHolders; //keep track of transform position

    public double networth;
    public TMP_Text tmp_networth;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBalanceSheet();
    }

    public void GenerateBalanceSheet()
    {
        foreach (Asset asset in assets)
        {
            AddAsset(asset);
        }

        foreach (Liability l in liabilities)
        {
            AddLiability(l);
        }

        getNetWorth();
    }

    public void AddAsset(Asset a)
    {
        Transform last_a = aHolders[aHolders.Count - 1].transform;

        float x = last_a.position.x;
        float y = last_a.position.y - 50;
        float z = last_a.position.z;

        GameObject aHolder = Instantiate(assetHolder, new Vector3(x, y, z), Quaternion.identity, assetsPanel.transform);

        a.AssignTextMesh(aHolder.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), aHolder.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>());
        a.updateText();

        if(a.sellable == true)
        {
            aHolder.transform.GetChild(2).gameObject.SetActive(true);
        }


        aHolders.Add(aHolder);
    }

    public void AddLiability(Liability l)
    {
        Transform last_l = lHolders[lHolders.Count - 1].transform;

        float x = last_l.position.x;
        float y = last_l.position.y - 50;
        float z = last_l.position.z;
        GameObject lHolder = Instantiate(liabilityHolder, new Vector3(x, y, z), Quaternion.identity, liabilitiesPanel.transform);

        l.AssignTextMesh(lHolder.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>(), lHolder.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>());
        l.updateText();
        lHolders.Add(lHolder);
    }

    //clear the UI
    public void ClearBalanceSheet()
    {
        for(int i = aHolders.Count - 1; i > 0; i--)
        {
            Destroy(aHolders[i]);
        }
    }

    public double getNetWorth()
    {
        double nw = 0;
        foreach(Asset a in assets)
        {
            nw += a.amount;
        }
        foreach(Liability l in liabilities)
        {
            nw -= l.amount;
        }
        networth = nw;
        tmp_networth.text = nw + "";
        return nw;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
