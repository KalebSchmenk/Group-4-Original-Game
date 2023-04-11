using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressionManager : MonoBehaviour
{
    private enum CurrentLevel
    {
        CombatLevel,
        PuzzleLevel,
        FinalLevel
    }

    [Header("Current Scene")]
    [SerializeField] private CurrentLevel _currentLevel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            {
                switch (_currentLevel)
                {
                    case CurrentLevel.CombatLevel:

                        GameManager._completedCombatLevel = true;

                        SceneManager.LoadScene("HUBLevel");

                        break;

                    case CurrentLevel.PuzzleLevel:

                        GameManager._completedPuzzleLevel = true;

                        SceneManager.LoadScene("HUBLevel");

                        break;

                    case CurrentLevel.FinalLevel:

                        GameManager._completedFinalLevel = true;

                        SceneManager.LoadScene("HUBLevel");

                        break;
                }
            }
        }
    }
}
