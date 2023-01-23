using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Commission
{
    public string title;
    public string clientName;
    public double price;
    public bool completed;

    public TMP_Text tmp_title;
    public TMP_Text tmp_client;
    public TMP_Text tmp_price;

    public Commission(string t,string n,double p)
    {
        title = t;
        clientName = n;
        price = p;
        completed = false;
    }
    public void AssignTextMesh(TMP_Text tmp_t,TMP_Text tmp_c, TMP_Text tmp_p)
    {
        tmp_title = tmp_t;
        tmp_client = tmp_c;
        tmp_price = tmp_p;
        tmp_title.text = title;
        tmp_client.text = clientName;
        tmp_price.text = price + "";
    }
}


public class CommissionsManager : MonoBehaviour
{
    public GameObject commissionPanel;
    public BalanceSheet balanceSheet;


    public List<Commission> commissions;
    public GameObject commissionHolder;
    [SerializeField] List<GameObject> cHolders;
    // Start is called before the first frame update
    void Start()
    {

        foreach (Commission c in commissions)
        {
            Transform last_c = cHolders[cHolders.Count - 1].transform;
            float x = last_c.position.x;
            float y = last_c.position.y-80;
            float z = last_c.position.z;
            GameObject cHolder = Instantiate(commissionHolder, new Vector3(x, y, z), Quaternion.identity, commissionPanel.transform);
            c.AssignTextMesh(cHolder.transform.GetChild(0).GetComponent<TMP_Text>(), cHolder.transform.GetChild(1).GetComponent<TMP_Text>(), cHolder.transform.GetChild(2).GetComponent<TMP_Text>());
            cHolders.Add(cHolder);
        }
        
    }

    public void CompleteCommission(Commission c)
    {
        c.completed = true;
        //WARNING first iten should always be wallet hard 
        balanceSheet.assets[0].amount += c.price;
        balanceSheet.assets[0].updateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
