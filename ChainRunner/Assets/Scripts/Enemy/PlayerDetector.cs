using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private bool detectPlayerEnter;
    [SerializeField] private bool detectPlayerLeave;

    public delegate void OnPlayerTriggered();
    public OnPlayerTriggered PlayerExitedCallback, PlayerEnterCallback;

    void OnTriggerEnter2D(Collider2D other) {
        if (detectPlayerEnter && other.CompareTag("Player")) {
            PlayerEnterCallback?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (detectPlayerLeave && other.CompareTag("Player")) {
            PlayerExitedCallback?.Invoke();
        }
    }
}

