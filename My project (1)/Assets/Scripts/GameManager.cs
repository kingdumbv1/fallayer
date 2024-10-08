using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject pauseMenu;
    public GameObject UI;
    public PlayerController playerData;

    public Image healthBar;
    public TextMeshProUGUI clipCounter;
    public TextMeshProUGUI ammoCounter;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerController>();
        UI = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(((float)playerData.health / (float)playerData.maxHealth), 0, 1);

        if (playerData.weaponID < 0)
        {
            clipCounter.gameObject.SetActive(false);
            ammoCounter.gameObject.SetActive(false);
        }

        else
        {
            clipCounter.gameObject.SetActive(true);
            clipCounter.text = "Clip: " + playerData.currentClip + "/" + playerData.clipSize;

            ammoCounter.gameObject.SetActive(true);
            ammoCounter.text = "Ammo: " + playerData.currentAmmo;
        }

        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;

            UI.SetActive(false);
            pauseMenu.SetActive(true);

            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            playerData.mousesensitivity = 0;
            playerData.mousesensx = 0;
            playerData.mousesensy = 0;
        }

        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
            Resume();
    }

    public void Resume()
    {
        isPaused = false;

        pauseMenu.SetActive(false);
        UI.SetActive(true);

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerData.mousesensitivity = 2;
        playerData.mousesensx = 2;
        playerData.mousesensy = 2;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}