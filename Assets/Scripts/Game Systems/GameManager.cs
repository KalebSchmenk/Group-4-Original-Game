using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Level Completion Bools")]
    public static bool _completedCombatLevel = false;
    public static bool _completedPuzzleLevel = false;
    public static bool _completedFinalLevel = false;
    public static bool _hasCompletedTutorial = false;

    [Header("Level Access Blockers")]
    [SerializeField] private GameObject _gateToPuzzle;
    [SerializeField] private GameObject _gateToFinal;

    [SerializeField] private GameObject _puzzleDoorLeft;
    [SerializeField] private GameObject _puzzleDoorRight;

    [SerializeField] private GameObject _finalDoorLeft;
    [SerializeField] private GameObject _finalDoorRight;

    [Header("Spawn After Tutorial Completion")]
    [SerializeField] Transform _positionOfNewSpawn;

    void Start()
    {
        CheckpointManager._currentCheckpoint = null;

        if (SceneManager.GetActiveScene().name == "HUBLevel" && _hasCompletedTutorial == true)
        {
            if (_positionOfNewSpawn == null) return;

            GameObject.FindGameObjectWithTag("Player").transform.position = _positionOfNewSpawn.position;
        }

        /*if (_completedCombatLevel == false)
        {
            Material mat = _gateToPuzzle.GetComponent<Renderer>().material;

            mat.DisableKeyword("_EMISSION");

            Debug.Log("Should be called");
        }*/


        if (_puzzleDoorLeft != null && _completedCombatLevel == true)
        {
            //rotate doors
        }

        if (_finalDoorLeft != null && _completedPuzzleLevel == true)
        {
            //rotate doors
        }
    }
}
