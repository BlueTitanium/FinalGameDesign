using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public GameObject continueButton;
    private Animator a;
    private string nextLevel;
    public AudioClip clip;
    public GameObject Regular, NewGameHolder;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (!PlayerPrefs.HasKey("curLevel"))
        {
            continueButton.SetActive(false);
        } else
        {
            continueButton.SetActive(true);
        }
        a = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    public void ActuallyLoadScene()
    {
        SceneManager.LoadScene(nextLevel);
    }

    IEnumerator SetDifficultyScale(float dmg, float hp)
    {
        PlayerPrefs.DeleteAll();
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetFloat("DamageMultiplier", dmg);
        PlayerPrefs.SetFloat("HealthMultiplier", hp);
        PlayerPrefs.Save();
        print("Set");
    }

    public void Easy()
    {
        StartCoroutine(SetDifficultyScale(3, 10));
        NewGame();
    }
    public void Normal()
    {
        StartCoroutine(SetDifficultyScale(1.5f, 2f));
        NewGame();
    }
    public void Hard() {
        StartCoroutine(SetDifficultyScale(1,.8f));
        NewGame();
    }

    public void Back()
    {
        NewGameHolder.SetActive(false);
        Regular.SetActive(true);
    }

    public void GoToNewGameScreen()
    {
        NewGameHolder.SetActive(true);
        Regular.SetActive(false);
    }

    public void NewGame()
    {
        nextLevel = "JessDemo";
        
        a.SetTrigger("NextScene");
    }

    public void Continue()
    {
        nextLevel = PlayerPrefs.GetString("curLevel");
        a.SetTrigger("NextScene");
    }

    public void Credits()
    {
        nextLevel = "END";
        a.SetTrigger("NextScene");
    }
}
