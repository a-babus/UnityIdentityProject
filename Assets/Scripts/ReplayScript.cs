using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayScript : MonoBehaviour
{

    AudioManager audioManager;

    public void ReplayGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.PlaySFX(audioManager.gameOver);
        DontDestroyOnLoad(audioManager.gameObject);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(2);
    }

}
