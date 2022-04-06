using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapCanvasDisplay : MonoBehaviour
{
    public RectTransform arrow;
    public float rotateTime = 1f;
    public static bool moreMoney;

    IEnumerator ArrowMovement()
    {
        while (true)
        {
            yield return arrow.DORotateQuaternion(Quaternion.Euler(0, 0, -90), rotateTime).WaitForCompletion();
            yield return arrow.DORotateQuaternion(Quaternion.Euler(0, 0, 90), rotateTime).WaitForCompletion();
            yield return null;
        }
    }


    private void OnEnable()
    {
        StartCoroutine(ArrowMovement());
        //moreMoney = false;
    }

    private void OnDisable()
    {
        if (arrow.transform.rotation.eulerAngles.z <= 60 && arrow.transform.rotation.eulerAngles.z >= -60)
        {
            moreMoney = true;
        }
        else
        {
            moreMoney = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
