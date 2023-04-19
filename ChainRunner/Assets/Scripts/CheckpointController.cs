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


    public void setCheckPoint(int id)
    {
        PlayerController.p.TakeHeal(PlayerController.p.maxHP);
        curCheckPointID = id;
        CheckpointController.c.SaveOptions();
        GameManager.g.ShowTitleEffect("Rift Attuned");
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            curCheckPointID = (curCheckPointID + 1) % checkpoints.Length;
            p.transform.position = checkpoints[curCheckPointID].position;
            SaveOptions();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            curCheckPointID -= 1;
            if(curCheckPointID < 0)
            {
                curCheckPointID = checkpoints.Length - 1;
            }
            p.transform.position = checkpoints[curCheckPointID].position;
            SaveOptions();
        }
    }
}
