using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class Menu : MonoBehaviour
{
    public AudioSource fpsPlayerSource;
    public Slider fxSlider;

    public static float fxVolume = 0.5f;

    public GameObject mainCamera;
    public GameObject fpsController;

    public GameObject menuScreen;
    public GameObject optionScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject loseScreen;

    public TextMeshProUGUI scoreText;

    private FirstPersonController controllerScript;

    private IEnumerator quitCoroutine;

    private bool isPaused = false;
    private bool gameWon = false;

    private static Menu instance;

    public void Awake()
    {
        mainCamera.SetActive(true);
        fpsController.SetActive(false);
        menuScreen.SetActive(true);
    }

    private void Start()
    {
        controllerScript = fpsController.GetComponent<FirstPersonController>();
        fxSlider.value = fxVolume;

        quitCoroutine = QuitTheGame(4.0f);

        if (instance == null) {
            instance = this;
        }
    }

    private void Update()
    {
        fpsPlayerSource.volume = fxVolume;

        if (Input.GetKeyDown(KeyCode.Escape) && menuScreen.activeSelf == false && optionScreen.activeSelf == false && winScreen.activeSelf == false && loseScreen.activeSelf == false)
        {
            UsePauseMenu();
        }
    }

    public void SetFXVolume(float vol)
    {
        fxVolume = vol;
    }

    public void UsePauseMenu()
    {
        isPaused = !isPaused;
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        Checklist.ChangeListActive();
        controllerScript.enabled = !controllerScript.isActiveAndEnabled;
    }

    public void OptionsMenuSwitch()
    {
        optionScreen.SetActive(false);
        if (isPaused)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            menuScreen.SetActive(true);
        }
    }

    public static void GameOver(int score)
    {
        if (instance != null) {
            instance.gameWon = true;

            instance.mainCamera.SetActive(true);
            instance.fpsController.SetActive(false);

            Checklist.ChangeListActive();

            instance.scoreText.text = score + "%";
            instance.winScreen.SetActive(true);

            instance.StartCoroutine(instance.quitCoroutine);
        }
    }

    public static void YouLose()
    {
        if (instance != null) {
            instance.mainCamera.SetActive(true);
            instance.fpsController.SetActive(false);

            Checklist.ChangeListActive();

            instance.loseScreen.SetActive(true);

            instance.StartCoroutine(instance.quitCoroutine);
        }
    }

    public void Play()
    {
        mainCamera.SetActive(false);
        fpsController.SetActive(true);

        menuScreen.SetActive(false);

        Checklist.ChangeListActive();
        HollyPotionMaking.Reset();
    }

    public void RestartGame()
    {
        fpsController.transform.position = new Vector3(2.1f, 2, 0);

        if (gameWon)
        {
            winScreen.SetActive(false);
        }
        else
        {
            loseScreen.SetActive(false);
        }
        menuScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator QuitTheGame(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Application.Quit();
        }
    }    
}