using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public string TargetTag = "Player";

    public KeyCode InteractionKey = KeyCode.E;

    [SerializeField] private GameObject MessageToDisplay;

    private bool _isPlayerInside;

    private static int numberOfItemsCollected = 0;

    private bool doorsOpen = false;

    private void Update()
    {
        if (_isPlayerInside)
        {
            CheckForInteraction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            _isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            _isPlayerInside = false;
        }
    }

    ///// Interaction Methods /////
    private void CheckForInteraction()
    {
        if (Input.GetKeyDown(InteractionKey))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        numberOfItemsCollected++;
        if (numberOfItemsCollected == 1 && !doorsOpen)
        {
            DesactivateDoors();
            StartCoroutine(ShowAndHideMessage());
        }
    }

    private void DesactivateDoors()
    {
        GameObject obj = GameObject.Find("Doors");
        if (obj != null)
        {
            obj.SetActive(false);
            doorsOpen = true;
            Debug.Log("Doors disapeared");
        }
    }

    private IEnumerator ShowAndHideMessage()
    {
        Debug.Log("Trying to open the doors");
        MessageToDisplay.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        MessageToDisplay.SetActive(false);

    }

}
