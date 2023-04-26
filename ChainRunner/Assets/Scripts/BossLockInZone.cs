using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLockInZone : MonoBehaviour
{

    private bool doOnce = false;
    private bool ready = false;

    public GameObject cam;
    

    public GameObject bossObject;
    //public Transform spawnPoint;

    public GameObject[] entrances;
    public List<GameObject> enemiesLeft;
    private int enemiesCount = 0;

    public GameObject[] Rewards;

    [Header("Soundtrack")]
    public AudioSource BGM;
    public AudioClip regular, battle;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var g in entrances)
        {
            g.SetActive(false);
        }
        cam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            enemiesCount = 0;
            foreach (var e in enemiesLeft)
            {
                if (e != null && e.transform.childCount > 1)
                {
                    enemiesCount += 1;
                }
                else
                {
                    Destroy(e);
                }
            }
            if (enemiesCount == 0)
            {
                enemiesLeft.Clear();
                Deactivate();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!doOnce && collision.gameObject.CompareTag("Player"))
        {
            doOnce = true;
            Activate();
        }
    }



    void Activate()
    {
        //GameManager.g.ShowTitleEffect("BOSS BATTLE!");

        foreach (var g in entrances)
        {
            g.SetActive(true);
        }

        cam.SetActive(true);

        GameObject o = Instantiate(bossObject, bossObject.transform.position, bossObject.transform.rotation);
        enemiesLeft.Add(o);

        BGM.Stop();
        BGM.clip = battle;
        BGM.Play();

        ready = true;
    }

    void Deactivate()
    {
        cam.SetActive(false);
        ready = false;
        foreach (var g in entrances)
        {
            g.SetActive(false);
        }
        GameManager.g.ShowTitleEffect("Boss Slain!");

        BGM.Stop();
        BGM.clip = regular;
        BGM.Play();

        StartCoroutine(SpawnRewards());
    }

    IEnumerator SpawnRewards()
    {
        yield return null;
        foreach (var g in Rewards)
        {
            GameObject a = Instantiate(g, PlayerController.p.transform.position, g.transform.rotation);
            yield return null;
            yield return new WaitUntil(() => a == null);
            yield return null;
        }
    }

}
