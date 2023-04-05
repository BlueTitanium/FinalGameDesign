using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public GameObject continueButton;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene("JessDemo");
        PlayerPrefs.DeleteAll();
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("curLevel"));
    }
}
