using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip playerWalkSound, 
                            playerJumpSound,
                            playerAttackSound,
                            playerHurtSound,

                            enemyHurtSound,
                            enemySwordSound,
                            ghostWhooshSound,
                            //enemyArrowSound,
                            bossAttackSound, // cat spawns from ground
                            warningSound, // red exclamation

                            playerChainSound,
                            chainMissSound,
                            chainIllegalSound,
                            wooshSound,

                            playerPowerUpSound,
                            playerGrabItemSound,

                            molotovSound,
                            fireSound,
                            itemFallSound,
                            itemHitSound,

                            attuneSound,
                            doorOpenSound,
                            gemDoorSound,
                            typingSound,
                            selectionSound, // if want for button hover
                            clickSound; // if want button click
                    
    static AudioSource audioSrc;

    public static float walkTime = .35f;
    private static float walkTimeLeft = 0;

    // public static float attackWaitTime = 2f;
    // private static float attackWaitTimeLeft = 2f;

    // Start is called before the first frame update
    void Start()
    {
        playerWalkSound = Resources.Load<AudioClip>("playerWalk");
        playerJumpSound = Resources.Load<AudioClip>("playerJump");
        playerAttackSound = Resources.Load<AudioClip>("playerAttack");
        playerHurtSound = Resources.Load<AudioClip>("playerHurt");

        enemyHurtSound = Resources.Load<AudioClip>("enemyHurt");
        enemySwordSound = Resources.Load<AudioClip>("enemySwordNoDelay");
        ghostWhooshSound = Resources.Load<AudioClip>("ghostWhoosh");
        //enemyArrowSound = Resources.Load<AudioClip>("enemyArrow");
        bossAttackSound = Resources.Load<AudioClip>("bossAttack");
        warningSound = Resources.Load<AudioClip>("warning");

        playerChainSound = Resources.Load<AudioClip>("playerChain");
        chainMissSound = Resources.Load<AudioClip>("chainMiss");
        chainIllegalSound = Resources.Load<AudioClip>("chainIllegal");
        wooshSound = Resources.Load<AudioClip>("woosh");

        playerPowerUpSound = Resources.Load<AudioClip>("playerPowerUp");
        playerGrabItemSound = Resources.Load<AudioClip>("playerGrabItem");

        molotovSound = Resources.Load<AudioClip>("molotov");
        fireSound = Resources.Load<AudioClip>("fire");
        itemFallSound = Resources.Load<AudioClip>("itemFall"); // straight projectile
        itemHitSound = Resources.Load<AudioClip>("itemHit"); // grav 

        attuneSound = Resources.Load<AudioClip>("attune");
        doorOpenSound = Resources.Load<AudioClip>("doorOpen");
        gemDoorSound = Resources.Load<AudioClip>("gemDoor");
        typingSound = Resources.Load<AudioClip>("typing1");
        selectionSound = Resources.Load<AudioClip>("selection"); // button hover
        clickSound = Resources.Load<AudioClip>("click"); // button click

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(walkTimeLeft > 0)
        {
            walkTimeLeft -= Time.deltaTime;
        }
        // if(attackWaitTimeLeft > 0)
        // {
        //     attackWaitTimeLeft -= Time.deltaTime;
        // }
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
                audioSrc.PlayOneShot(playerJumpSound, 0.1f);
                break;
            case "playerAttack":
                //audioSrc.clip = playerAttackSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(playerAttackSound, 0.8f);
                break;
            case "playerHurt":
                audioSrc.PlayOneShot(playerHurtSound);
                break;

            ///////////

            case "enemyHurt":
                audioSrc.PlayOneShot(enemyHurtSound, 0.3f);
                break;
            case "enemySword":
                // audioSrc.clip = enemySwordSound;
                // if (attackWaitTimeLeft <= 0)
                // {
                //     audioSrc.Play();
                //     attackWaitTimeLeft = attackWaitTime;
                // }
                audioSrc.PlayOneShot(enemySwordSound, 0.5f);
                break;
            case "ghostWhoosh":
                audioSrc.PlayOneShot(ghostWhooshSound, 0.5f);
                break;
            // case "enemyArrow":;
            //     audioSrc.PlayOneShot(enemyArrowSound);
            //     break;
            case "bossAttack":
                audioSrc.PlayOneShot(bossAttackSound);
                break;
            case "warning":
                audioSrc.PlayOneShot(warningSound, 0.5f);
                break;

            ///////////

            case "playerChain": //throw chain
                audioSrc.PlayOneShot(playerChainSound);
                break;
            case "chainMiss": 
                audioSrc.PlayOneShot(chainMissSound);
                break;
            case "chainIllegal": 
                audioSrc.PlayOneShot(chainIllegalSound, 0.8f);
                break;
            case "woosh":
                //audioSrc.clip = wooshSound;
                //audioSrc.Play();
                audioSrc.PlayOneShot(wooshSound);
                break;

            ///////////

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

            ///////////

            case "molotov":
                audioSrc.PlayOneShot(molotovSound, 0.5f);
                break;
            case "fire":
                audioSrc.PlayOneShot(fireSound);
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
                audioSrc.PlayOneShot(attuneSound, 0.2f);
                break;
            case "doorOpen":
                audioSrc.PlayOneShot(doorOpenSound);
                break;
            case "gemDoor":
                audioSrc.PlayOneShot(gemDoorSound);
                break;
            case "typing":
                if (!audioSrc.isPlaying) {
                    //audioSrc.pitch = Random.Range(.2f,5f);
                    audioSrc.PlayOneShot(typingSound, 0.5f);
                }
                audioSrc.pitch = 1f;
                break;
            case "selection":
                audioSrc.PlayOneShot(selectionSound);
                break;
            case "click":
                audioSrc.PlayOneShot(clickSound);
                break;
        }
    }
}
