using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcVipDisplay : MonoBehaviour
{
    public bool move;
    public List<GameObject> models = new List<GameObject>();

    private int modelType;
    PlayerController playerController;



    private void OnEnable()
    {
        modelType = Random.Range(0, models.Count);
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        for (int i = 0; i < models.Count; i++)
        {
            if (modelType == i)
            {
                models[i].SetActive(true);
                models[i].GetComponentInChildren<NailDisplay>().gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
        }
    }

    public void OpenNails()
    {
        playerController.stackToDropped = false;
        if (move)
        {
            StartCoroutine(OpenNailsDelay());
        }
    }

    IEnumerator OpenNailsDelay()
    {
        yield return new WaitForSeconds(3f);
        foreach (var item in models)
        {
            if (item)
            {
                item.GetComponentInChildren<NailDisplay>().gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            }
        }
    }
}
