using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip playerWalkSound, 
                            playerJumpSound,
                            playerAttackSound,
                            playerChainSound,
                            wooshSound,
                            playerPowerUpSound,
                            attuneSound;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        playerWalkSound = Resources.Load<AudioClip>("playerWalk");
        playerJumpSound = Resources.Load<AudioClip>("playerJump");
        playerAttackSound = Resources.Load<AudioClip>("playerAttack");
        playerChainSound = Resources.Load<AudioClip>("playerChain");
        wooshSound = Resources.Load<AudioClip>("woosh");
        playerPowerUpSound = Resources.Load<AudioClip>("playerPowerUp");
        attuneSound = Resources.Load<AudioClip>("attune");

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
            case "playerChain": //throw chain
                //audioSrc.volume = 0.1f;
                audioSrc.PlayOneShot (playerChainSound);
                break;
            case "woosh":
                audioSrc.PlayOneShot (wooshSound);
                break;
            case "playerPowerUp":
                audioSrc.PlayOneShot (playerPowerUpSound);
                break;

            case "attune":
                audioSrc.PlayOneShot (attuneSound);
                break;
        }
    }
}
