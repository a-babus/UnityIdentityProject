using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleJump : MonoBehaviour
{
    AudioManager audioManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioManager.PlaySFX(audioManager.jump);
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
}
