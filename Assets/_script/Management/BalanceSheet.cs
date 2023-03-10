using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Asset 
{
    public AssetObject assetObject;
    public TMP_Text tmp_name;
    public TMP_Text tmp_amount;

    public bool sellable;
    public Asset(AssetObject ao)
    {
        assetObject = ao;
    }
    public void updateText()
    {
        tmp_name.text = assetObject.name + "";
        tmp_amount.text = assetObject.value + "";
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
        ClearPanels();
        ClearBalanceSheet(true,true);
        GenerateBalanceSheet(true,true);
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
            aHolder.transform.GetChild(2).gameObject.SetActive(true); //sell button
            aHolder.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => SellAsset(a));
        }
        aHolders.Add(aHolder);
    }

    public void BuyItem(GameObject item)
    {
        assets.Add(new Asset(item.GetComponent<StoreItem>().assetObject));
        assets[0].assetObject.value -= item.GetComponent<StoreItem>().assetObject.value;//WARNINg asset0 hard wired to wallet
        ClearBalanceSheet(true, false);
        GenerateBalanceSheet(true, false);
    }

    public void SellAsset(Asset a)
    {
        assets[0].assetObject.value += a.assetObject.value;//Warning: first item = wallet;
        assets.Remove(a);
        ClearBalanceSheet(true,false);
        GenerateBalanceSheet(true,false);
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

    public void ClearPanels()
    {
        int n = assetsPanel.transform.childCount;
        for (int i = n - 1; i > 0; i--)
        {
            Destroy(assetsPanel.transform.GetChild(i).gameObject);
        }
        n = liabilitiesPanel.transform.childCount;
        for (int i = n - 1; i > 0; i--)
        {
            Destroy(liabilitiesPanel.transform.GetChild(i).gameObject);
        }
    }

    //clear the UI
    public void ClearBalanceSheet(bool clearAssets,bool clearLiabilities)
    {
        if (clearAssets)
        {
            for (int i = aHolders.Count - 1; i > 0; i--)
            {
                Destroy(aHolders[i]);
                aHolders.Remove(aHolders[i]);
                
            }
        }
        if (clearLiabilities)
        {
            for (int i = lHolders.Count - 1; i > 0; i--)
            {
                Destroy(lHolders[i]);
            }
        }
    }

    public void GenerateBalanceSheet(bool generateAssets, bool generateLiabilities)
    {
        if (generateAssets)
        {
            foreach (Asset asset in assets)
            {
                AddAsset(asset);
            }
        }

        if (generateLiabilities)
        {
            foreach (Liability l in liabilities)
            {
                AddLiability(l);
            }
        }
        
        getNetWorth();
    }

    public double getNetWorth()
    {
        double nw = 0;
        foreach(Asset a in assets)
        {
            nw += a.assetObject.value;
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
