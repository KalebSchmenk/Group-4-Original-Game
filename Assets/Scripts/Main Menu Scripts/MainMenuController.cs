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
    [SerializeField] GameObject optionsObject;
    [SerializeField] GameObject kbmControlView;
    [SerializeField] GameObject gamepadControlView;
    [SerializeField] GameObject quitConformObject;
    // Start is called before the first frame update

    private void Start() {
        menuSounds.clip = buttonPressClip;
    }
    public void StartGame()
    {
        GameManager._hasCompletedTutorial = false;
        GameManager._completedCombatLevel = false;
        GameManager._completedPuzzleLevel = false;
        GameManager._completedFinalLevel = false;

        StartCoroutine(SoundBeforeSceneChange("HUBLevel"));
    }

    public void QuitGame(){
        menuSounds.Play();               
        quitConformObject.SetActive(true);
        mainMenuObject.SetActive(false);
        howtoplayObject.SetActive(false);
    }


    public void HowToPlay(){
        menuSounds.Play();
        mainMenuObject.SetActive(false);
        howtoplayObject.SetActive(true);
        quitConformObject.SetActive(false);
    }

    public void BackButton(){
        menuSounds.Play();
        howtoplayObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(true);
        quitConformObject.SetActive(false);
    }

    private IEnumerator SoundBeforeSceneChange(string scene){
        menuSounds.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        if(scene == "quit"){
            Application.Quit();   
        }
        else{
            SceneManager.LoadScene(scene);
        }
    }

    public void Options(){
        menuSounds.Play();
        mainMenuObject.SetActive(false);
        optionsObject.SetActive(true);
        quitConformObject.SetActive(false);
    }

     public void KBMControlSwitch() {
        menuSounds.Play();
        kbmControlView.SetActive(true);
        gamepadControlView.SetActive(false);
        }

    public void GamepadControlSwitch() {
        menuSounds.Play();
        gamepadControlView.SetActive(true);
        kbmControlView.SetActive(false);
        }

    public void QuitConfirm(){
        StartCoroutine(SoundBeforeSceneChange("quit"));
    }

}
