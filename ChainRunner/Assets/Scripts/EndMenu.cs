using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField]
    private Animator LoadingAnimator;

    public TextMeshProUGUI text;
    [TextArea(15,10)]
    public string[] texts;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(PlayAllCredits());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayAllCredits()
    {
        text.text = "<b>CONDEMNED</b>";
        yield return new WaitForSeconds(1f);
        foreach(var t in texts)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(SwitchTexts(t));
        }
    }


    public IEnumerator SwitchTexts(string next)
    {
        float timer = 1;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, timer);
            yield return null;
        }
        text.text = next;
        timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            text.alpha = Mathf.Lerp(1, 0, timer);
            yield return null;
        }
    }

    public void MainMenu()
    {
        LoadNextScene("StartMenu");
    }

    public void LoadNextScene(string name)
    {
        StartCoroutine(ActuallyRestart(name));
    }

    private IEnumerator ActuallyRestart(string name)
    {
        LoadingAnimator.SetTrigger("Load");
        yield return new WaitUntil(() => LoadingAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LoadIn");
        yield return new WaitUntil(() => LoadingAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Loading");
        SceneManager.LoadScene(name);
    }
}
