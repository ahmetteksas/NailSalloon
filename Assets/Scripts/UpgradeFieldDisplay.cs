using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UpgradeFieldDisplay : MonoBehaviour
{
    private GameObject nailTable;
    public GameObject text;
    public GameObject textNumber;
    public GameObject orderCanvas;
    public TextMeshPro costText;
    public int costAmount = 50;
    private int costAmountFirst;
    public SpriteRenderer outline;
    private float r = 0.9882353f;
    private float g = 0.4039216f;
    private float b = 1;

    private Color newColor;

    public float colorChangeTime;
    private bool changeColor;

    Collider colBase;

    public bool ordered;
    public float scaleTime = .5f;

    private void Awake()
    {
        nailTable = GetComponentInChildren<TableDisplay>().gameObject;
        costText.text = "$ " + costAmount.ToString();
        colBase = GetComponent<BoxCollider>();
    }

    void Start()
    {
        newColor = new Color(r, g, b);
    }
    GameObject currentObject;
    public static bool currentObjectSelected;
    string currentName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //currentObjectSelected = true;
            if (!currentObjectSelected)
            {
                currentObjectSelected = true;
                currentName = gameObject.name;
            }
            costAmountFirst = other.gameObject.GetComponent<PlayerController>().money;
            decreaseMoney = false;
            difAmount = costAmountFirst - costAmount;
            changeColor = false;
            costMoney = handedMoney;
        }
    }


    public void OrderedCanvasOpen()
    {
        StartCoroutine(CloseCanvas());
        ordered = true;
    }

    IEnumerator CloseCanvas()
    {
        if (GetComponentInChildren<TableDisplay>().isActive && GetComponentInChildren<TableDisplay>().isOrdered)
        {
            yield return new WaitForSeconds(2f);
            orderCanvas.SetActive(true);
            yield return orderCanvas.transform.DOScale(new Vector3(.015f, .015f, .015f), scaleTime)
                 .WaitForCompletion();
            yield return new WaitForSeconds(3f);
            orderCanvas.SetActive(false);
        }
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            if (!decreaseMoney)
            {
                yield return outline.DOColor(newColor, colorChangeTime).WaitForCompletion();
                yield return outline.DOColor(Color.white, colorChangeTime).WaitForCompletion();
            }
            yield return null;
        }
    }

    int difAmount;
    bool decreaseMoney;
    int costMoney;
    int handedMoney;
    bool colliderTrue;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") /*&& !decreaseMoney*/)
        {
            //currentObject = gameObject;
            if (!changeColor)
            {
                if (!decreaseMoney && costAmountFirst >= 50)
                {
                    StartCoroutine(ChangeColor());
                    changeColor = true;
                }
            }
            difAmount--;
            handedMoney = costMoney;
            if (costAmount > 0 && handedMoney < 50 && other.gameObject.GetComponent<PlayerController>().money >= costAmount && currentName == gameObject.name)
            {
                costMoney++;
                costAmount--;
            }
            //if (costAmount > 0 && costAmountFirst >= 50)
            //{
            //    costAmount--;
            //}
            if (other.gameObject.GetComponent<PlayerController>().money > costAmount /*other.gameObject.GetComponent<PlayerController>().money > 0*/ && currentName == gameObject.name && currentObjectSelected/*difAmount/*other.gameObject.GetComponent<PlayerController>().money*/ /*> -51 && costAmount > -1*/)
            {
                if (handedMoney < 50)
                {
                    other.gameObject.GetComponent<PlayerController>().money--;
                }
            }
            if (costAmount == 0 && handedMoney == 50 && currentName == gameObject.name/*other.gameObject.GetComponent<PlayerController>().money == 0*/)
            {
                colliderTrue = true;
                nailTable.GetComponent<MeshRenderer>().enabled = true;
                if (colliderTrue)
                {
                    nailTable.GetComponent<MeshCollider>().enabled = true;
                    other.gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.right * 4000f);
                    colliderTrue = false;
                }
                text.SetActive(false);
                textNumber.SetActive(false);
                colBase.enabled = false;
                //Physics.IgnoreLayerCollision(6, 11, true);
                handedMoney = 50;
                currentObjectSelected = false;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") /*&& currentName == gameObject.name*/)
        {
            handedMoney = costMoney;
            decreaseMoney = true;
            StopCoroutine(ChangeColor());
            currentObject = null;
            //currentName = gameObject.name;
        }
    }


    void Update()
    {
        costText.text = "$ " + costAmount.ToString();
    }
}
