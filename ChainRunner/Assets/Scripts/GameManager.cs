using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        LoadOptions();
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
}