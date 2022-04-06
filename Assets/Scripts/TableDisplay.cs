using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;

public class TableDisplay : MonoBehaviour
{
    public GameObjectCollection tableList;
    public GameObjectCollection tableVipList;

    public bool vip;
    public bool listed;

    private MeshRenderer nMesh;
    public bool isActive;
    public bool isFull;
    public bool employeeWorks;
    public bool isOrdered;

    public Transform employeePos;
    public Transform npcPos;

    public GameObject arrowCanvas;


    private void Awake()
    {
        tableList.Clear();
        tableVipList.Clear();
        nMesh = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        if (listed && !vip)
        {
            tableList.Add(gameObject);
        }
        if (listed && vip)
        {
            tableVipList.Add(gameObject);
        }
    }

    public void IsFullEvent()
    {
        isFull = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stack"))
        {
            if (arrowCanvas)
            {
                arrowCanvas.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (nMesh.enabled)
        {
            isActive = true;
            listed = true;
            //tableList.Add(gameObject);
        }
        else
        {
            isActive = false;
        }
        if (listed)
        {
            tableList.Add(gameObject);
        }
        if (GetComponent<VipTableDisplay>() != null)
        {
            vip = true;
        }
        else
        {
            vip = false;
        }
        if (Input.GetMouseButtonDown(0) && arrowCanvas)
        {
            arrowCanvas.SetActive(false);
        }
    }
    
}
