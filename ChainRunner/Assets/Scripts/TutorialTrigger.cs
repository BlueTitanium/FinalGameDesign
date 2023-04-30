using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public bool triggerOn = true;
    private bool doOnce = false;
    public string title, description;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadOptions();
        if (doOnce && triggerOn)
        {
            Destroy(gameObject);
        }
        if(doOnce && !triggerOn)
        {
            Destroy(this);
        }
    }

    public void Tutorial()
    {
        TutorialManager.t.StartTutorial(title, description);
        doOnce = true;
        SaveOptions();
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetInt("tutorial"+title, doOnce ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void LoadOptions()
    {
        if (!PlayerPrefs.HasKey("tutorial" + title))
        {
            SaveOptions();
        }
        doOnce = PlayerPrefs.GetInt("tutorial" + title) == 1 ? true : false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerOn && !doOnce && collision.gameObject.CompareTag("Player"))
        {
            TutorialManager.t.StartTutorial(title, description);
            doOnce = true;
            SaveOptions();
            Destroy(gameObject);
        }
    }
}
