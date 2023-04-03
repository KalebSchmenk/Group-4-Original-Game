using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;

public class GameOverController : MonoBehaviour
{

    [SerializeField] AudioSource menuSoundsObject;
    [SerializeField] AudioClip buttonPressClip;
   
    [SerializeField] Button restartGameButton;
    [SerializeField] Button quitGameButton;
    

    private float storedCinemachineXSpeed;
    private float storedCinemachineYSpeed;
    public CinemachineFreeLook cinemachineFL;

    [SerializeField] GameObject playerObject;
    private PlayerController playerScript;
    bool _gameOver;
    bool _win;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject winMenu;
    bool gameOverTriggered;

    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioClip menuMusicClip;
    
    // Start is called before the first frame update

    void Start()
    {
        playerScript = playerObject.GetComponent<PlayerController>();
        storedCinemachineXSpeed = cinemachineFL.m_XAxis.m_MaxSpeed;
        storedCinemachineYSpeed = cinemachineFL.m_YAxis.m_MaxSpeed;
        menuMusic.clip = menuMusicClip;
    }
    void Update(){
        _gameOver = playerScript._gameOver;
        _win = playerScript._win;
        
        if(_gameOver && _win == false && gameOverTriggered == false){
            GameOver();
            gameOverTriggered = true;
        }

        if(_win && gameOverTriggered == false){
            Win();
            gameOverTriggered = true;
        }
    }
    public void QuitGame()
    {
        StartCoroutine(SoundBeforeSceneChange("MainMenu"));
    }
    public void RestartGame(){
        StartCoroutine(SoundBeforeSceneChange("PuzzleDesign"));

    }

    void GameOver(){
            gameOverMenu.SetActive(true);
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in audioSources){
                a.mute = true;
            }
            menuMusic.mute = false;
            menuMusic.Play();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            cinemachineFL.m_XAxis.m_MaxSpeed = 0.0f;
            cinemachineFL.m_YAxis.m_MaxSpeed = 0.0f;
    }

    void Win(){
        winMenu.SetActive(true);
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in audioSources){
                a.mute = true;
            }
            menuMusic.mute = false;
            menuMusic.Play();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            cinemachineFL.m_XAxis.m_MaxSpeed = 0.0f;
            cinemachineFL.m_YAxis.m_MaxSpeed = 0.0f;
    }

    private IEnumerator SoundBeforeSceneChange(string scene){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(scene);
        cinemachineFL.m_XAxis.m_MaxSpeed = storedCinemachineXSpeed;
        cinemachineFL.m_YAxis.m_MaxSpeed = storedCinemachineYSpeed;
}
}
