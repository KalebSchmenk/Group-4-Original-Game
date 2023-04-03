using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
public void StartGame(){
    SceneManager.LoadScene("PuzzleDesign");
}

public void QuitGame(){
    //!!!! Remove before building the game!!!!
    UnityEditor.EditorApplication.isPlaying = false;
    //!!!!                                !!!!

    Application.Quit();
}
}
