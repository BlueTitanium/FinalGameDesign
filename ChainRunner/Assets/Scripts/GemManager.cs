using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public static GemManager g;
    public int[] gemStates = { 0, 0, 0 };

    public GemDoor door;
    public GameObject[] UIGems;
    public GameObject[] DoorGems;


    // Start is called before the first frame update
    void Start()
    {
        g = this;
        LoadOptions();
    }

    public void ShowGemStates()
    {
        switch (gemStates[0])
        {
            case 0:
                UIGems[0].SetActive(false);
                DoorGems[0].SetActive(false);
                break;
            case 1:
                UIGems[0].SetActive(true);
                DoorGems[0].SetActive(false);
                break;
            case 2:
                UIGems[0].SetActive(false);
                DoorGems[0].SetActive(true);
                break;
        }
        switch (gemStates[1])
        {
            case 0:
                UIGems[1].SetActive(false);
                DoorGems[1].SetActive(false);
                break;
            case 1:
                UIGems[1].SetActive(true);
                DoorGems[1].SetActive(false);
                break;
            case 2:
                UIGems[1].SetActive(false);
                DoorGems[1].SetActive(true);
                break;
        }
        switch (gemStates[2])
        {
            case 0:
                UIGems[2].SetActive(false);
                DoorGems[2].SetActive(false);
                break;
            case 1:
                UIGems[2].SetActive(true);
                DoorGems[2].SetActive(false);
                break;
            case 2:
                UIGems[2].SetActive(false);
                DoorGems[2].SetActive(true);
                break;
        }

        bool readyToOpen = true;
        foreach(var v in gemStates)
        {
            if(v != 2)
            {
                readyToOpen = false;
                break;
            }
        }
        door.readyToOpen = readyToOpen;
    }

    private void OnApplicationQuit()
    {
        SaveOptions();
    }
    

    public void SaveOptions()
    {
        PlayerPrefs.SetInt("gemstate0", gemStates[0]);
        PlayerPrefs.SetInt("gemstate1", gemStates[1]);
        PlayerPrefs.SetInt("gemstate2", gemStates[2]);
        PlayerPrefs.Save();
        ShowGemStates();
    }
    public void LoadOptions()
    {
        if (!PlayerPrefs.HasKey("gemstate0"))
        {
            SaveOptions();
        }
        gemStates[0] = PlayerPrefs.GetInt("gemstate0");
        gemStates[1] = PlayerPrefs.GetInt("gemstate1");
        gemStates[2] = PlayerPrefs.GetInt("gemstate2");
        ShowGemStates();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
