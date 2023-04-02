using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private bool detectPlayerEnter;
    [SerializeField] private bool detectPlayerLeave;
    [SerializeField] private bool detectPlayerStay;

    public delegate void OnPlayerTriggered();
    public OnPlayerTriggered PlayerExitedCallback, PlayerEnterCallback, PlayerStayCallback;

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

    void OnTriggerStay2D(Collider2D other) {
        if (detectPlayerStay && other.CompareTag("Player")) {
            PlayerStayCallback?.Invoke();
        }
    }
}

