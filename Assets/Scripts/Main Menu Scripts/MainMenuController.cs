using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] GameObject mainMenuObject;
    [SerializeField] GameObject howtoplayObject;
    [SerializeField] AudioSource menuSounds;
    [SerializeField] AudioClip buttonPressClip;
    // Start is called before the first frame update

private void Start() {
    menuSounds.clip = buttonPressClip;
}
public void StartGame(){
    StartCoroutine(SoundBeforeSceneChange("PuzzleDesign"));
}

public void QuitGame(){               
    StartCoroutine(SoundBeforeSceneChange("quit"));
}


public void HowToPlay(){
    menuSounds.Play();
    mainMenuObject.SetActive(false);
    howtoplayObject.SetActive(true);
}

public void BackButton(){
    menuSounds.Play();
    howtoplayObject.SetActive(false);
    mainMenuObject.SetActive(true);
}

private IEnumerator SoundBeforeSceneChange(string scene){
    menuSounds.Play();
    yield return new WaitForSecondsRealtime(0.3f);
    if(scene == "quit"){
        Application.Quit();
        //!!!! Remove before building the game!!!!
        UnityEditor.EditorApplication.isPlaying = false;
        //!!!!      
    }
    else{
        SceneManager.LoadScene(scene);
    }
}

}
