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
                            playerGrabItemSound,
                            itemFallSound,
                            itemHitSound,
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
        playerGrabItemSound = Resources.Load<AudioClip>("playerGrabItem");
        itemFallSound = Resources.Load<AudioClip>("itemFall");
        itemHitSound = Resources.Load<AudioClip>("itemHit");
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
                audioSrc.clip = playerWalkSound;
                audioSrc.Play();
                break;
            case "playerJump":
                audioSrc.clip = playerJumpSound;
                audioSrc.Play();
                break;
            case "playerAttack":
                audioSrc.clip = playerAttackSound;
                audioSrc.Play();
                break;
            case "playerChain": //throw chain
                //audioSrc.volume = 0.1f;
                audioSrc.clip = playerChainSound;
                audioSrc.Play();
                break;
            case "woosh":
                audioSrc.clip = wooshSound;
                audioSrc.Play();
                break;
            case "playerPowerUp":
                audioSrc.clip = playerPowerUpSound;
                audioSrc.Play();
                break;
            case "playerGrabItem":
                audioSrc.clip = playerGrabItemSound;
                audioSrc.Play();
                break;
            case "itemFall":
                audioSrc.clip = itemFallSound;
                audioSrc.Play();
                break;
            case "itemHit":
                audioSrc.clip = itemHitSound;
                audioSrc.Play();
                break;

            case "attune":
                audioSrc.clip = attuneSound;
                audioSrc.Play();
                break;
        }
    }
}
