using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Touch;

public class NailPainter : MonoBehaviour
{
    [SerializeField] RectTransform arrow;
    [SerializeField]Renderer nailRenderer;
    [SerializeField] ParticleSystem endSparkle;
    [SerializeField] GameObject confettiPS;
    [SerializeField] Animator brushAnimator;


    Vector3 arrowRotation;
    float offsetAmount= -1;
    bool painting = false;



    // Start is called before the first frame update
    void Start()
    {
        RotateArrow();
        brushAnimator.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        arrowRotation = arrow.transform.eulerAngles;
        print("arrow:" + arrowRotation.z);
        if (painting)
        {
            print("Parinting in progress");
            nailRenderer.material.SetFloat("Vector1_286f4a8ae81342799bb806e754aa5585", offsetAmount);
        }

    }


    private void OnEnable()
    {
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
    }

    private void HandleFingerDown(LeanFinger finger)
    {
        DOTween.Pause("arrowtween");
        if (arrowRotation.z > -28 && arrowRotation.z - 360 < 28)
        {
            //Debug.Log("Arrow Rotation: " + arrow.rotation.z);
            painting = true;
            brushAnimator.enabled = true;
            DOTween.To(() => nailRenderer.material.GetFloat("Vector1_286f4a8ae81342799bb806e754aa5585"), x => offsetAmount = x, 0, 1.5f).SetEase(Ease.Linear).OnComplete(() => { endSparkle.Play(); painting = false; });            
            StartCoroutine(Confetti());
            
        }
    }
    private void HandleFingerUp(LeanFinger finger)
    {

    }
    private void HandleFingerUpdate(LeanFinger finger)
    {

    }

    private void RotateArrow()
    {
        arrow.DORotate(new Vector3(0, 0, -90), 2).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo).SetId("arrowtween");
    }

    IEnumerator Confetti()
    {
        yield return new WaitForSeconds(3);
        confettiPS.SetActive(true);
    }
}
