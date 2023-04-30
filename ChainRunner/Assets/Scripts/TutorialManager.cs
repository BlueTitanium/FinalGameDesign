using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager t;
    public Animator a;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    // Start is called before the first frame update
    void Start()
    {
        t = this;
    }

    public void Close()
    {
        PlayerController.p.allowControls = true;
        a.SetTrigger("Close");
    }

    public void StartTutorial(string t, string d)
    {
        PlayerController.p.allowControls = false;
        a.SetTrigger("Start");
        title.text = t;
        description.text = d;
    }

}
