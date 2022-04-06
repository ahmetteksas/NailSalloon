using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreMoneyDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator DestroyObject ()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
