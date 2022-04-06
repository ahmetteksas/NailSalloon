using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using DG.Tweening;

public class EmployeeDisplay : MonoBehaviour
{
    public GameObjectCollection employeeList;
    public GameObjectCollection npcList;
    public GameObjectCollection vipNpcList;
    public GameObjectCollection tableList;


    public GameEventBase isFullEvent;

    public float npcMovementTime = 3f;

    private Transform cashierPos;
    private Transform finishPos;
    private Transform moneyPos;

    public bool isWorking;
    public bool move;
    public bool isSitting;
    public bool animStartEmp;
    public bool isSittingVip;
    public bool animStartEmpVip;

    PlayerController _playerController;
    private Animator animBase;

    public GameObject twoTimes;


    private void Awake()
    {
        cashierPos = GameObject.Find("CashierPosition").transform;
        finishPos = GameObject.Find("FinishPosition").transform;
        moneyPos = GameObject.Find("DollarSpawnPosition").transform;
    }

    private void OnEnable()
    {
        animBase = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    public bool animStart;
    public bool vipAnimStart;
    public IEnumerator MoveNpc()
    {
        yield return new WaitForFixedUpdate();
        if (npcList.FirstOrDefault() && npcList.FirstOrDefault().GetComponent<NpcDisplay>().move && !npcList.FirstOrDefault().GetComponent<NpcVipDisplay>())
        {
            //animStart = true;
            npcList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Walk");
            //isSitting = false;
            npcList.FirstOrDefault().transform.DOLookAt(cashierPos.position, npcMovementTime / 6);
            //_playerController.animStart = true;
            animStartEmp = false;
            animBase.SetTrigger("PedicureIdle");
            yield return npcList.FirstOrDefault().transform.DOMove(cashierPos.position, npcMovementTime).WaitForCompletion();
            GameObject _money = ObjectPool.instance.SpawnFromPool("Dollar", moneyPos.position, Quaternion.identity);
            npcList.FirstOrDefault().transform.DOLookAt(finishPos.position, npcMovementTime / 6);
            yield return npcList.FirstOrDefault().transform.DOMove(finishPos.position, npcMovementTime).WaitForCompletion();
            isSitting = false;
            isFullEvent.Raise();
            if (npcList.FirstOrDefault())
            {
                npcList.FirstOrDefault().SetActive(false);
            }
            npcList.Remove(npcList.FirstOrDefault());
        }
    }
    public IEnumerator MoveVipNpc()
    {
        yield return new WaitForFixedUpdate();
        if (vipNpcList.FirstOrDefault() && vipNpcList.FirstOrDefault().GetComponent<NpcVipDisplay>().move && vipNpcList.FirstOrDefault().GetComponent<NpcVipDisplay>())
        {
            //animStart = true;
            vipNpcList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Walk");
            //isSittingVip = false;
            vipNpcList.FirstOrDefault().transform.DOLookAt(cashierPos.position, npcMovementTime / 3);
            //_playerController.animStart = true;
            animStartEmpVip = false;
            animBase.SetTrigger("PedicureIdle");
            yield return vipNpcList.FirstOrDefault().transform.DOMove(cashierPos.position, npcMovementTime).WaitForCompletion();
            if (TapCanvasDisplay.moreMoney)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject _money = ObjectPool.instance.SpawnFromPool("Dollar", moneyPos.position, Quaternion.identity);
                }
                twoTimes.SetActive(true);
            }
            else
            {
                GameObject _money = ObjectPool.instance.SpawnFromPool("Dollar", moneyPos.position, Quaternion.identity);
            }
            vipNpcList.FirstOrDefault().transform.DOLookAt(finishPos.position, npcMovementTime / 6);
            yield return vipNpcList.FirstOrDefault().transform.DOMove(finishPos.position, npcMovementTime).WaitForCompletion();
            isSittingVip = false;
            isFullEvent.Raise();
            if (vipNpcList.FirstOrDefault())
            {
                vipNpcList.FirstOrDefault().SetActive(false);
            }
            vipNpcList.Remove(vipNpcList.FirstOrDefault());
        }
    }

    float _differance;
    float _differanceVip;
    float _differanceTable;
    float _differancePlayer;
    public GameObject currentTable;
    private void Update()
    {
        _differancePlayer = Vector3.Distance(transform.position, _playerController.gameObject.transform.position);
        if (npcList.FirstOrDefault())
        {
            _differance = Vector3.Distance(transform.position, npcList.FirstOrDefault().transform.position);
        }
        if (vipNpcList.FirstOrDefault())
        {
            _differanceVip = Vector3.Distance(transform.position, vipNpcList.FirstOrDefault().transform.position);
        }
        foreach (var item in tableList)
        {
            _differanceTable = Vector3.Distance(transform.position, item.transform.position);
            if (_differanceTable <= 3f && item.GetComponent<TableDisplay>().isFull)
            {
                currentTable = item;
            }
        }

        if (_differance <= 3f && _differancePlayer <= 3)
        {
            animStart = true;
        }
        else
        {
            animStart = false;
        }
        if (_differanceVip <= 3f && _differancePlayer <= 3)
        {
            vipAnimStart = true;
        }
        else
        {
            vipAnimStart = false;
        }
    }
}
