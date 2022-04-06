using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObjectCollection employeeList;
    public GameObjectCollection npcList;
    public GameObjectCollection npcVipList;
    public GameObjectCollection tableList;
    public GameObjectCollection stackList;

    public GameEventBase ordered;

    public float employeeMoveTime = 2f;
    public float npcMoveTime = 2f;

    public int employeeNumber = 1;
    public int npcNumber = 4;
    public int npcVipNumber = 2;


    public Transform employeSpawnPos;
    public Transform npcSpawnPos1;
    public Transform npcSpawnPos2;
    public Transform vipSpawnPos;
    public Transform vipSpawnPos2;


    private List<Transform> npcSpawnPositions = new List<Transform>();
    private List<Transform> vipSpawnPositions = new List<Transform>();


    private GameObject employee;
    private GameObject npc;
    private GameObject vip;


    private void Awake()
    {
        stackList.Clear();
        employeeList.Clear();
        npcList.Clear();
        npcVipList.Clear();
        npcSpawnPositions.Add(npcSpawnPos1);
        npcSpawnPositions.Add(npcSpawnPos2);
        vipSpawnPositions.Add(vipSpawnPos);
        vipSpawnPositions.Add(vipSpawnPos2);
    }

    private void Start()
    {
        StartCoroutine(EmployeeSpawn());
        StartCoroutine(NpcSpawn());
        StartCoroutine(NpcVipSpawn());
    }

    public void WorkAnimatorsStart()
    {
        foreach (var item in employeeList)
        {
            if (!item.GetComponent<EmployeeDisplay>().animStartEmp /*&& item.GetComponent<EmployeeDisplay>().currentTable*/)
            {
                if (item.GetComponent<EmployeeDisplay>().animStart)
                {
                    item.GetComponent<Animator>().SetTrigger("Work");
                    item.GetComponent<EmployeeDisplay>().animStartEmp = true;
                    item.GetComponent<EmployeeDisplay>().animStart = false;
                }
            }
            if (!item.GetComponent<EmployeeDisplay>().animStartEmpVip /*&& item.GetComponent<EmployeeDisplay>().currentTable*/)
            {
                if (item.GetComponent<EmployeeDisplay>().vipAnimStart)
                {
                    item.GetComponent<Animator>().SetTrigger("WorkVip");
                    item.GetComponent<EmployeeDisplay>().animStartEmpVip = true;
                    item.GetComponent<EmployeeDisplay>().vipAnimStart = false;
                }
            }
        }
        if (npcList.FirstOrDefault().GetComponent<NpcDisplay>().move && npcList.FirstOrDefault().GetComponent<NpcVipDisplay>() == null)
        {
            npcList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Sit");
        }
        if (npcVipList.FirstOrDefault().GetComponent<NpcVipDisplay>().move && npcVipList.FirstOrDefault().GetComponent<NpcVipDisplay>() != null)
        {
            npcVipList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Sit");
        }
    }

    IEnumerator EmployeeSpawn()
    {
        while (true)
        {
            if (employeeList.Count < employeeNumber)
            {
                employee = ObjectPool.instance.SpawnFromPool("Employee", employeSpawnPos.position, Quaternion.identity);
                employeeList.Add(employee);
                employee.GetComponent<Animator>().SetTrigger("Idle");
            }
            if (tableList.LastOrDefault().GetComponent<TableDisplay>().isActive)
            {
                if (!employee.GetComponent<EmployeeDisplay>().move)
                {
                    yield return new WaitForSeconds(.2f);
                    tableList.LastOrDefault().GetComponent<TableDisplay>().employeeWorks = true;
                    employee.GetComponent<Animator>().SetTrigger("Walk");
                    employee.GetComponent<EmployeeDisplay>().move = true;
                    employee.transform.DOLookAt(tableList.LastOrDefault().GetComponent<TableDisplay>().employeePos.position, employeeMoveTime / 6);
                    yield return employee.transform.DOMove(tableList.LastOrDefault().GetComponent<TableDisplay>().employeePos.position, employeeMoveTime).WaitForCompletion();
                    if (tableList.LastOrDefault().transform.position.x <= 0)
                    {
                        yield return employee.transform.DORotate(Vector3.up * 90, employeeMoveTime / 6).WaitForCompletion();
                    }
                    else
                    {
                        yield return employee.transform.DORotate(Vector3.up * 90, employeeMoveTime / 6).WaitForCompletion();
                    }
                }
                if (!employee.GetComponent<EmployeeDisplay>().isSitting)
                {
                    employee.GetComponent<Animator>().SetTrigger("PedicureIdle");
                    employee.GetComponent<EmployeeDisplay>().isSitting = true;
                }
                if (!employee.GetComponent<EmployeeDisplay>().isSittingVip)
                {
                    employee.GetComponent<Animator>().SetTrigger("PedicureIdle");
                    employee.GetComponent<EmployeeDisplay>().isSittingVip = true;
                }
            }
            yield return null;
        }
    }

    int npcTarget;
    IEnumerator NpcSpawn()
    {
        while (true)
        {
            //yield return new WaitForSeconds(1f);
            if (npcList.Count < npcNumber && npcList.Count == 0)
            {
                for (int i = 0; i < npcSpawnPositions.Count; i++)
                {
                    npc = ObjectPool.instance.SpawnFromPool("Npc", npcSpawnPositions[i].position, Quaternion.identity);
                    npcList.Add(npc);
                    npc.GetComponent<Animator>().SetTrigger("Idle");
                }
            }
            yield return new WaitForSeconds(3f);
            if (npcList.FirstOrDefault())
            {
                if (!npcList.FirstOrDefault().GetComponent<NpcDisplay>().move)
                {
                    npcTarget = Random.Range(0, tableList.Count);
                    for (int i = 0; i < tableList.Count; i++)
                    {
                        if (npcTarget == i && tableList[i].GetComponent<TableDisplay>().employeeWorks && !tableList[i].GetComponent<TableDisplay>().isFull && tableList[i].GetComponent<VipTableDisplay>() == null)
                        {
                            npcList.FirstOrDefault().GetComponent<NpcDisplay>().move = true;
                            if (tableList[i].GetComponent<VipTableDisplay>() == null)
                            {
                                tableList[i].GetComponent<TableDisplay>().isOrdered = true;
                            }
                            ordered.Raise();
                            npcList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Walk");
                            npcList.FirstOrDefault().transform.DOLookAt(tableList[i].GetComponent<TableDisplay>().npcPos.position, npcMoveTime / 6);
                            if (tableList[i].transform.position.x <= 0)
                            {
                                yield return npcList.FirstOrDefault().transform.DOMove(tableList[i].GetComponent<TableDisplay>().npcPos.position, npcMoveTime).WaitForCompletion();
                                yield return npcList.FirstOrDefault().transform.DORotate(Vector3.up * -90, npcMoveTime / 6).WaitForCompletion();
                                npcList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Sit");
                                if (tableList[i].GetComponent<VipTableDisplay>() == null)
                                {
                                    tableList[i].GetComponent<TableDisplay>().isFull = true;
                                    tableList[i].GetComponent<TableDisplay>().isOrdered = false;
                                }
                            }
                            else
                            {
                                yield return npcList.FirstOrDefault().transform.DOMove(tableList[i].GetComponent<TableDisplay>().npcPos.position, npcMoveTime).WaitForCompletion();
                                yield return npcList.FirstOrDefault().transform.DORotate(Vector3.up * -90, npcMoveTime / 6).WaitForCompletion();
                                npcList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Sit");
                                if (tableList[i].GetComponent<VipTableDisplay>() == null)
                                {
                                    tableList[i].GetComponent<TableDisplay>().isFull = true;
                                    tableList[i].GetComponent<TableDisplay>().isOrdered = false;
                                }
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator NpcVipSpawn()
    {
        while (true)
        {
            //yield return new WaitForSeconds(1f);
            if (npcVipList.Count < npcVipNumber && npcVipList.Count == 0)
            {
                for (int i = 0; i < vipSpawnPositions.Count; i++)
                {
                    vip = ObjectPool.instance.SpawnFromPool("Vip", vipSpawnPositions[i].position, Quaternion.identity);
                    npcVipList.Add(vip);
                    vip.GetComponent<Animator>().SetTrigger("Idle");
                }
            }
            yield return new WaitForSeconds(3f);
            if (npcVipList.FirstOrDefault())
            {
                if (!npcVipList.FirstOrDefault().GetComponent<NpcVipDisplay>().move)
                {
                    npcTarget = Random.Range(0, tableList.Count);
                    for (int i = 0; i < tableList.Count; i++)
                    {
                        if (npcTarget == i && tableList[i].GetComponent<TableDisplay>().employeeWorks && !tableList[i].GetComponent<TableDisplay>().isFull && tableList[i].GetComponent<VipTableDisplay>() != null)
                        {
                            npcVipList.FirstOrDefault().GetComponent<NpcVipDisplay>().move = true;
                            if (tableList[i].GetComponent<VipTableDisplay>() != null)
                            {
                                tableList[i].GetComponent<TableDisplay>().isOrdered = true;
                            }
                            ordered.Raise();
                            npcVipList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Walk");
                            npcVipList.FirstOrDefault().transform.DOLookAt(tableList[i].GetComponent<TableDisplay>().npcPos.position, npcMoveTime / 6);
                            if (tableList[i].transform.position.x <= 0)
                            {
                                yield return npcVipList.FirstOrDefault().transform.DOMove(tableList[i].GetComponent<TableDisplay>().npcPos.position, npcMoveTime).WaitForCompletion();
                                yield return npcVipList.FirstOrDefault().transform.DORotate(Vector3.up * -90, npcMoveTime / 6).WaitForCompletion();
                                if (tableList[i].GetComponent<PedicureStation>())
                                {
                                    npcVipList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Pedicure");
                                }
                                else
                                {
                                    npcVipList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Sit");
                                }
                                if (tableList[i].GetComponent<VipTableDisplay>() != null)
                                {
                                    tableList[i].GetComponent<TableDisplay>().isFull = true;
                                    tableList[i].GetComponent<TableDisplay>().isOrdered = false;
                                    //tableList[i].GetComponent<TableDisplay>().arrowCanvas.SetActive(true);
                                }
                            }
                            else
                            {
                                yield return npcVipList.FirstOrDefault().transform.DOMove(tableList[i].GetComponent<TableDisplay>().npcPos.position, npcMoveTime).WaitForCompletion();
                                yield return npcVipList.FirstOrDefault().transform.DORotate(Vector3.up * -90, npcMoveTime / 6).WaitForCompletion();
                                if (tableList[i].GetComponent<PedicureStation>())
                                {
                                    npcVipList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Pedicure");
                                }
                                else
                                {
                                    npcVipList.FirstOrDefault().GetComponent<Animator>().SetTrigger("Sit");
                                }
                                if (tableList[i].GetComponent<VipTableDisplay>() != null)
                                {
                                    tableList[i].GetComponent<TableDisplay>().isFull = true;
                                    tableList[i].GetComponent<TableDisplay>().isOrdered = false;
                                    //tableList[i].GetComponent<TableDisplay>().arrowCanvas.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
    private void Update()
    {
        employeeNumber = tableList.Count;
    }

}
