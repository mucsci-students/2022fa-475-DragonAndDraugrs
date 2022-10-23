using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvas;

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log(collision);
        if (collision.CompareTag("Player"))
        {
            canvas.SetActive(false);
        }
    }
}
