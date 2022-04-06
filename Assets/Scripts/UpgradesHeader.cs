using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradesHeader : MonoBehaviour, IPointerDownHandler
{
    public GameObject staffSection;
    public GameObject upgradeSection;

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (EventSystem.current.useGUILayout)
                {
                    Debug.Log("Pressed Upgrade!!!");
                    upgradeSection.SetActive(true);
                    staffSection.SetActive(false);
                }
            }
        }
    }

    public void ChangeFile()
    {
        Debug.Log("Pressed Upgrade!!!");
        upgradeSection.SetActive(true);
        staffSection.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pressed Upgrade!!!");
    }
}
