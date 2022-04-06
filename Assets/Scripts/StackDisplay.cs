using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;

public class StackDisplay : MonoBehaviour
{
    public GameObjectCollection stackList;
    public GameEventBase workAnimatorStart;
    PlayerController _playerController;

    void Start()
    {
        if (stackList)
        {
            stackList.Add(gameObject);
        }
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Table"))
        {
            _playerController.stackNumberHanded--;
            workAnimatorStart.Raise();
            gameObject.SetActive(false);
        }
    }


}
