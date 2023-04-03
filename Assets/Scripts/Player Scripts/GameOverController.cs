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
    
    

    

    // Start is called before the first frame update

    void Start()
    {
        playerScript = playerObject.GetComponent<PlayerController>();
        storedCinemachineXSpeed = cinemachineFL.m_XAxis.m_MaxSpeed;
        storedCinemachineYSpeed = cinemachineFL.m_YAxis.m_MaxSpeed;
    }

    void Update(){
        _gameOver = playerScript._gameOver;
        _win = playerScript._win;
        
        if(_gameOver && _win == false){
            GameOver();
        }

        if(_win){
            Win();
        }
    }




    public void QuitGame()
    {
        StartCoroutine(SoundBeforeMainMenu());
    }


    private IEnumerator SoundBeforeMainMenu()
    {
        cinemachineFL.m_XAxis.m_MaxSpeed = storedCinemachineXSpeed;
        cinemachineFL.m_YAxis.m_MaxSpeed = storedCinemachineYSpeed;
        menuSoundsObject.PlayOneShot(buttonPressClip);
        yield return new WaitForSecondsRealtime(0.3f);

        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }


    public void RestartGame(){
        StartCoroutine(SoundBeforeRestart());

    }



    private IEnumerator SoundBeforeRestart()
    {
        cinemachineFL.m_XAxis.m_MaxSpeed = storedCinemachineXSpeed;
        cinemachineFL.m_YAxis.m_MaxSpeed = storedCinemachineYSpeed;
        menuSoundsObject.PlayOneShot(buttonPressClip);
        yield return new WaitForSecondsRealtime(0.3f);

        Time.timeScale = 1;
        SceneManager.LoadScene("PuzzleDesign");
    }

    void GameOver(){
            gameOverMenu.SetActive(true);
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in audioSources){
                a.mute = true;
            }
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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            cinemachineFL.m_XAxis.m_MaxSpeed = 0.0f;
            cinemachineFL.m_YAxis.m_MaxSpeed = 0.0f;
    }
}
