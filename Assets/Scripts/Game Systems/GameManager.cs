using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Completion Bools")]
    public static bool _completedCombatLevel = false;
    public static bool _completedPuzzleLevel = false;
    public static bool _completedFinalLevel = false;

    [Header("Level Access Blockers")]
    [SerializeField] private GameObject _blockerToPuzzle;
    [SerializeField] private GameObject _blockerToFinal;

    void Start()
    {
        CheckpointManager._currentCheckpoint = null;

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
