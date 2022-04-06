using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using System.Linq;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public GameObjectCollection tableList;
    public GameObjectCollection stackList;
    public GameObjectCollection employeeList;

    public List<Transform> stackListTransforms = new List<Transform>();

    public float stackJumpTime = .5f;

    public TextMeshProUGUI moneyText;
    public int money = 50;
    public GameObject plate;

    public Transform upgradeCenter;
    private float distanceUpgradeCenter;
    public GameObject upgradeUI;
    public TextMeshPro stackNumberHandedText;
    public int stackNumberLimit = 2;
    public int stackNumberHanded = 0;


    private void Awake()
    {
        moneyText.text = money.ToString() + " $";
    }

    public bool colliderTrue;

    UpgradeFieldDisplay _upgradeFieldDisplay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StackCabinet"))
        {
            if (_upgradeFieldDisplay.ordered)
            {
                StartCoroutine(StackSpawn());
            }
            stackToDropped = false;
        }
        if (other.gameObject.CompareTag("Dollar"))
        {
            money += 100;
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("StackCabinet"))
        {

        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Table"))
        {
            //colliderTrue = true;
            //if (other.gameObject.GetComponent<MeshRenderer>().enabled == true)
            //{
            //    gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.right * 1500f);
            //}
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Table"))
        {
            //colliderTrue = false;
        }
    }


    IEnumerator StackSpawn()
    {
        //if (tableList.Count == 1)
        //{
        for (int i = 0; i < stackNumberLimit; i++)
        {
            stackList[i].GetComponent<MeshRenderer>().enabled = true;
            stackList[i].transform.SetParent(stackListTransforms[i].transform);
            stackNumberHanded++;
            yield return stackList[i].transform.DOJump(stackList[i].transform.position, 3f, 1, stackJumpTime).WaitForCompletion();
            stackList[i].transform.localPosition = Vector3.zero;
        }
        //}
        yield return null;
    }

    float _differance;
    public bool stackToDropped;
    List<GameObject> _newStack = new List<GameObject>();
    public void StackDrop(GameObject _go)
    {
        GameObject _droppedStack = ObjectPool.instance.SpawnFromPool("Stack", transform.position, Quaternion.identity);
        _droppedStack.transform.DOJump(/*tableList.LastOrDefault()*/_go.transform.position, 3f, 1, stackJumpTime);
        foreach (var item in stackList)
        {
            if (item.GetComponent<MeshRenderer>().enabled == true)
            {
                _newStack.Add(item);
            }
        }

        _newStack.LastOrDefault().GetComponent<MeshRenderer>().enabled = false;
    }

    public void PlateActiveted()
    {
        plate.SetActive(true);
    }

    CanvasManager _canvasManager;
    float _differanceEmployee;
    public bool animStart;
    void Update()
    {
        //Physics.IgnoreLayerCollision(6, 11, false);

        _canvasManager = FindObjectOfType<CanvasManager>();
        _upgradeFieldDisplay = tableList.LastOrDefault().GetComponentInParent<UpgradeFieldDisplay>();
        moneyText.text = money.ToString() + " $";
        foreach (var item in tableList)
        {
            _differance = Vector3.Distance(transform.position, item.transform.position);
            if (_differance <= 1.5f /*&& tableList.LastOrDefault().GetComponent<TableDisplay>().isFull*/)
            {
                if (item.GetComponent<TableDisplay>().isFull)
                {
                    foreach (var item3 in employeeList)
                    {
                        _differanceEmployee = Vector3.Distance(transform.position, item3.transform.position);
                        if (_differanceEmployee <= 1.5f)
                        {
                            //if (!animStart)
                            //{
                            //    item3.GetComponent<Animator>().SetTrigger("Work");
                            //    animStart = true;
                            //}
                        }
                    }
                    if (!stackToDropped)
                    {
                        foreach (var item2 in stackList)
                        {
                            if (item2.GetComponent<MeshRenderer>().enabled == true)
                            {
                                StackDrop(item);
                                stackToDropped = true;
                            }
                        }
                    }
                }
            }
        }
        //_differance = Vector3.Distance(transform.position, tableList.LastOrDefault().transform.position);
        distanceUpgradeCenter = Vector3.Distance(transform.position, upgradeCenter.position);
        //if (_differance > 1.5f)
        //{
        //    Physics.IgnoreLayerCollision(6, 11, false);
        //}
        //else
        //{
        //    Physics.IgnoreLayerCollision(6, 11, false);
        //}
        if (distanceUpgradeCenter <= 1.5f && !_canvasManager.isPressed)
        {
            upgradeUI.SetActive(true);
        }
        else
        {
            upgradeUI.SetActive(false);
        }
        if (distanceUpgradeCenter >= 1.5f)
        {
            _canvasManager.isPressed = false;
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        if (stackNumberHanded <= stackNumberLimit)
        {
            stackNumberHandedText.text = stackNumberHanded.ToString() + "/" + stackNumberLimit.ToString();
        }
    }
}
