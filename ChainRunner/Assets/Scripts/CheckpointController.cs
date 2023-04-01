using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    public PlayerController p;
    public static CheckpointController c;
    public int curCheckPointID = 0;
    public Transform[] checkpoints;
    // Start is called before the first frame update
    void Start()
    {
        c = this;
        LoadOptions();
        p.transform.position = checkpoints[curCheckPointID].position;
    }
    public void SaveOptions()
    {
        PlayerPrefs.SetInt("curCheckPointID", curCheckPointID);
        PlayerPrefs.SetString("curLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void LoadOptions()
    {
        if (!PlayerPrefs.HasKey("curCheckPointID"))
        {
            SaveOptions();
        }
        if(SceneManager.GetActiveScene().name == PlayerPrefs.GetString("curLevel"))
        {
            curCheckPointID = PlayerPrefs.GetInt("curCheckPointID");
        } else
        {
            SaveOptions();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
