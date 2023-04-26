using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager g;

    [Header("Pause Menu")]
    [SerializeField]
    private Animator pauseScreen;
    private bool paused = false;
    private bool pausing = false;
    public float masterVolume = .5f;
    public float musicVolume = .5f;
    public float sfxVolume = .5f;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    [Header("Interaction UI")]
    [SerializeField]
    private GameObject ItemPickup;
    [SerializeField]
    private GameObject Interaction;

    [Header("Effects")]
    [SerializeField]
    private TextMeshProUGUI[] titleEffectTexts;
    [SerializeField]
    private Animation titleEffect;
    [SerializeField]
    private Animation hudAnim;
    [SerializeField]
    private Animator LoadingAnimator;
    // Start is called before the first frame update
    void Start()
    {
        g = this;
        Time.timeScale = 1;
        LoadOptions();
    }
    public void RemoveHud()
    {
        hudAnim.Stop();
        hudAnim.Play("Hud_Leave");
    }
    public void BringBackHud()
    {
        hudAnim.Stop();
        hudAnim.Play("Hud_Open");
    }
    public void ShowTitleEffect(string text)
    {
        titleEffect.Stop();

        foreach (TextMeshProUGUI t in titleEffectTexts)
        {
            t.text = text;
        }
        
        titleEffect.Play();
    }

    public void ItemPickupDisplayToggle(bool toggle)
    {
        ItemPickup.SetActive(toggle);
    }
    public void InteractionDisplayToggle(bool toggle)
    {
        Interaction.SetActive(toggle);
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    public void LoadOptions()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            SaveOptions();
        }
        masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }
    public void UpdateMixerVolume()
    {
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;
        SaveOptions();
        UpdateMixerVolume();
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        SaveOptions();
        UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        sfxVolume = value;
        SaveOptions();
        UpdateMixerVolume();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PausedState" || pauseScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "NotPausedState")
            {
                pausing = false;
            }
            pauseScreen.ResetTrigger("Pause");
            pauseScreen.ResetTrigger("Unpause");
            if (paused)
            {
                StartCoroutine(Unpause());
            } else
            {
                StartCoroutine(Pause());
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadNextScene("JessDemo");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadNextScene("LIMBO");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadNextScene("LUST");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadNextScene("LUSTBoss");
        }
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

    private IEnumerator Pause()
    {
        if (!pausing)
        {
            Time.timeScale = 0;
            pausing = true;
            paused = true;
            pauseScreen.SetTrigger("Pause");
            yield return new WaitUntil(() => pauseScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PauseOpen");
            yield return new WaitUntil(() => pauseScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PausedState");
            pausing = false;
        }
    }

    public void Resume()
    {
        pauseScreen.ResetTrigger("Pause");
        pauseScreen.ResetTrigger("Unpause");
        StartCoroutine(Unpause());
    }

    private IEnumerator Unpause()
    {
        if (!pausing)
        {
            pausing = true;
            paused = false;
            pauseScreen.SetTrigger("Unpause");
            yield return new WaitUntil(() => pauseScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PauseOpen");
            yield return new WaitUntil(() => pauseScreen.GetCurrentAnimatorClipInfo(0)[0].clip.name == "NotPausedState");
            pausing = false;
            Time.timeScale = 1;
            
        }
    }

    public void Exit()
    {
        Time.timeScale = 1;
        LoadNextScene("StartMenu");
    }
}
