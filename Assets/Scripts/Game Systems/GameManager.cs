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
    [SerializeField] private GameObject _blockerToPuzzle;
    [SerializeField] private GameObject _blockerToFinal;

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


        if (_blockerToPuzzle != null && _completedCombatLevel == true)
        {
            Destroy(_blockerToPuzzle);
        }

        if (_blockerToFinal != null && _completedPuzzleLevel == true)
        {
            Destroy(_blockerToFinal);
        }
    }
}
