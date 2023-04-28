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

    public void NewGame()
    {
        nextLevel = "JessDemo";
        PlayerPrefs.DeleteAll();
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
