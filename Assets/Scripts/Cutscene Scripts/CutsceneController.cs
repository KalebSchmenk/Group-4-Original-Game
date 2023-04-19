using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] float _leaveCutsceneIn = 30f;

    [SerializeField] bool _isStartCutscene = false;
    [SerializeField] bool _isEndCutscene = false;

    void Start()
    {
        StartCoroutine(LeaveCutsceneIn(_leaveCutsceneIn));
    }

    private IEnumerator LeaveCutsceneIn (float leaveIn)
    {
        yield return new WaitForSeconds(leaveIn);

        if (_isStartCutscene)
        {
            SceneManager.LoadScene("HUBLevel");
        }

        if (_isEndCutscene)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
