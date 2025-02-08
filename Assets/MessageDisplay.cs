using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplay : MonoBehaviour
{
    public GameObject startMessageObject;
    public GameObject messageObject;
    public float displayDuration = 3f;

    private void Start()
    {
        if (messageObject != null)
        {
            messageObject.SetActive(false);
        }
        if (startMessageObject != null)
        {
            StartCoroutine(ShowAndHideMessage(startMessageObject));
        }
    }

    public void ShowMessage()
    {
        if (messageObject != null)
        {
            StartCoroutine(ShowAndHideMessage(messageObject));
        }
    }

    private IEnumerator ShowAndHideMessage(GameObject message)
    {
        message.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        message.SetActive(false);
    }
}
