using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip playerWalkSound, 
                            playerJumpSound,
                            playerAttackSound;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        playerWalkSound = Resources.Load<AudioClip>("playerWalk");
        playerJumpSound = Resources.Load<AudioClip>("playerJump");
        playerAttackSound = Resources.Load<AudioClip>("playerAttack");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip) {
        switch (clip) {
            case "playerWalk":
                audioSrc.PlayOneShot (playerWalkSound);
                break;
            case "playerJump":
                audioSrc.PlayOneShot (playerJumpSound);
                break;
            case "playerAttack":
                audioSrc.PlayOneShot (playerAttackSound);
                break;
        }
    }
}
