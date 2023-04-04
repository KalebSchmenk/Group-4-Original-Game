using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendToLevelController : MonoBehaviour
{
    public string _sendToLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_sendToLevel != null)
            {
                SceneManager.LoadScene(_sendToLevel);
            }
        }
    }
}
