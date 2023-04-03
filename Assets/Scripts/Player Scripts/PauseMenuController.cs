using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;

public class PauseMenuController : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction pause;
    

    [SerializeField] AudioSource menuSoundsObject;
    [SerializeField] AudioClip buttonPressClip;
   
    [SerializeField] Button resumeGameButton;
    [SerializeField] Button quitGameButton;

    [SerializeField] GameObject pauseMenu;

    private float storedCinemachineXSpeed;
    private float storedCinemachineYSpeed;
    public CinemachineFreeLook cinemachineFL;
private void Awake() {
    playerControls = new PlayerInputActions();
    storedCinemachineXSpeed = cinemachineFL.m_XAxis.m_MaxSpeed;
    storedCinemachineYSpeed = cinemachineFL.m_YAxis.m_MaxSpeed;

}

private void OnEnable() {
    pause = playerControls.Player.Pause;
    pause.Enable();
}

private void OnDisable() {
    pause.Disable();
    
}

public void Update() {
        if(pause.triggered){
            if (pauseMenu.activeSelf == false){
                PauseGame();
            }
            else{
                ResumeGame();
            }
        }
    }

    public void PauseGame(){

        
        Debug.Log("Game Paused");
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audioSources){
            a.mute = true;
        }
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        cinemachineFL.m_XAxis.m_MaxSpeed = 0.0f;
        cinemachineFL.m_YAxis.m_MaxSpeed = 0.0f;
    }

    public void ResumeGame(){
        menuSoundsObject.PlayOneShot(buttonPressClip);
        
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audioSources){
            a.mute = false;
        }
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        cinemachineFL.m_XAxis.m_MaxSpeed = storedCinemachineXSpeed;
        cinemachineFL.m_YAxis.m_MaxSpeed = storedCinemachineYSpeed;
    }

    public void QuitGame(){
        StartCoroutine(SoundBeforeMainMenu());
    }


    private IEnumerator SoundBeforeMainMenu(){
            menuSoundsObject.PlayOneShot(buttonPressClip);
            yield return new WaitForSecondsRealtime(0.3f);
            
            SceneManager.LoadScene("MainMenu");
        }












}
