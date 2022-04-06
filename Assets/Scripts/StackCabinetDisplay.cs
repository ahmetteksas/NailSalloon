using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCabinetDisplay : MonoBehaviour
{
    private Collider colBase;

    private void Awake()
    {
        colBase = GetComponent<Collider>();
    }

    void Start()
    {
        
    }

    public void ColliderEnable()
    {
        colBase.isTrigger = true;
    }

}
