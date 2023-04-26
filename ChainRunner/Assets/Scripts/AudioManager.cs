using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip playerWalkSound, 
                            playerJumpSound,
                            playerAttackSound,
                            playerChainSound,
                            chainMissSound,
                            chainIllegalSound,

                            wooshSound,
                            playerPowerUpSound,
                            playerGrabItemSound,
                            itemFallSound,
                            itemHitSound,
                            attuneSound;
    static AudioSource audioSrc;

    public static float walkTime = .35f;
    private static float walkTimeLeft = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerWalkSound = Resources.Load<AudioClip>("playerWalk");
        playerJumpSound = Resources.Load<AudioClip>("playerJump");
        playerAttackSound = Resources.Load<AudioClip>("playerAttack");
        playerChainSound = Resources.Load<AudioClip>("playerChain");
        chainMissSound = Resources.Load<AudioClip>("chainMiss");
        chainIllegalSound = Resources.Load<AudioClip>("chainIllegal");

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
        if(walkTimeLeft > 0)
        {
            walkTimeLeft -= Time.deltaTime;
        }
    }

    public static void PlaySound(string clip) {
        switch (clip) {
            case "playerWalk":
                audioSrc.clip = playerWalkSound;
                if (!audioSrc.isPlaying && walkTimeLeft <= 0)
                {
                    audioSrc.Play();
                    walkTimeLeft = walkTime;
                }
                break;
            case "playerJump":
                //audioSrc.clip = playerJumpSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(playerJumpSound, 0.15f);
                break;
            case "playerAttack":
                //audioSrc.clip = playerAttackSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(playerAttackSound);
                break;
            case "playerChain": //throw chain
                //audioSrc.volume = 0.1f;
                //audioSrc.clip = playerChainSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(playerChainSound);
                break;
            case "chainMiss": 
                audioSrc.PlayOneShot(chainMissSound);
                break;
            case "chainIllegal": 
                audioSrc.PlayOneShot(chainIllegalSound);
                break;

            case "woosh":
                //audioSrc.clip = wooshSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(wooshSound);
                break;
            case "playerPowerUp":
                //audioSrc.clip = playerPowerUpSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(playerPowerUpSound);
                break;
            case "playerGrabItem":
                //audioSrc.clip = playerGrabItemSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(playerGrabItemSound);
                break;
            case "itemFall":
                //audioSrc.clip = itemFallSound;
                audioSrc.PlayOneShot(itemFallSound);
                break;
            case "itemHit":
                //audioSrc.clip = itemHitSound;
                audioSrc.PlayOneShot(itemHitSound);
                break;

            case "attune":
                //audioSrc.clip = attuneSound;
                audioSrc.PlayOneShot(attuneSound);
                break;
        }
    }
}
