using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRespawn : MonoBehaviour
{

    public float threshold;

    AudioManager audioManager;

    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            audioManager.PlaySFX(audioManager.failedFromPlatform);
            transform.position = new Vector3(-40.15865f, 1.9996f, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
}
