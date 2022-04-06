using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.Characters.ThirdPerson;

public class CanvasManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public bool isPressed;
    public TextMeshProUGUI cashierCost;
    public TextMeshProUGUI workerCost;
    public int cashierCostAmount;
    public int workerCostAmount;
    public int capacityCostAmount;
    public int speedCostAmount;
    public GameObject tailParticle;

    public List<GameObject> cashierSlots = new List<GameObject>();
    public List<GameObject> workerSlots = new List<GameObject>();
    public List<GameObject> capacitySlots = new List<GameObject>();
    public List<GameObject> speedSlots = new List<GameObject>();

    private int cashierSlotCounter;
    private int workerSlotCounter;
    private int capacitySlotCounter;
    private int speedSlotCounter;




    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    //public void CloseCanvas()
    //{
    //    Debug.Log("PressedButton");
    //    upgradeCanvas.SetActive(false);
    //}

    IEnumerator CloseCanvasNew()
    {
        yield return new WaitForSeconds(1f);
        upgradeCanvas.SetActive(false);
        isPressed = true;
    }
    public void CashierSpeed()
    {
        if (playerController.money >= cashierCostAmount)
        {
            cashierSlotCounter++;
            playerController.money -= cashierCostAmount;
            playerController.gameObject.GetComponent<ThirdPersonCharacter>().moveSpeed = 6f;
            playerController.gameObject.GetComponent<ThirdPersonCharacter>().animBase.speed = 2.5f;
            tailParticle.SetActive(true);
            for (int i = 0; i < cashierSlotCounter; i++)
            {
                cashierSlots[i].SetActive(true);
            }
        }
        StartCoroutine(CloseCanvasNew());
    }
    public void WorkerSpeed()
    {
        isPressed = true;
        if (playerController.money >= workerCostAmount)
        {
            workerSlotCounter++;
            playerController.money -= workerCostAmount;
            for (int i = 0; i < workerSlotCounter; i++)
            {
                workerSlots[i].SetActive(true);
            }
        }
        StartCoroutine(CloseCanvasNew());
    }
    public void CapacityAmount()
    {
        if (playerController.money >= cashierCostAmount)
        {
            playerController.money -= capacityCostAmount;
            playerController.stackNumberLimit += 2;
            capacitySlotCounter++;
            for (int i = 0; i < capacitySlotCounter; i++)
            {
                capacitySlots[i].SetActive(true);
            }
        }
        StartCoroutine(CloseCanvasNew());
    }
    public void SpeedAmount()
    {
        speedSlotCounter++;
        playerController.money -= speedCostAmount;
        playerController.gameObject.GetComponent<ThirdPersonCharacter>().moveSpeed = 6f;
        playerController.gameObject.GetComponent<ThirdPersonCharacter>().animBase.speed = 2.5f;
        tailParticle.SetActive(true);
        for (int i = 0; i < speedSlotCounter; i++)
        {
            speedSlots[i].SetActive(true);
        }
        StartCoroutine(CloseCanvasNew());
    }
}
