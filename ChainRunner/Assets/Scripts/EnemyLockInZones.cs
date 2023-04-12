using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockInZones : MonoBehaviour
{

    private bool doOnce = false;
    private bool ready = false;
    public GameObject[] enemiesToSpawn;
    public Transform[] spawnPoints;

    public GameObject[] entrances;
    public List<GameObject> enemiesLeft;
    private int enemiesCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var g in entrances)
        {
            g.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            enemiesCount = 0;
            foreach(var e in enemiesLeft)
            {
                if(e != null && e.transform.childCount > 1)
                {
                    enemiesCount += 1;
                } else
                {
                    Destroy(e);
                }
            }
            if (enemiesCount == 0) {
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
        foreach(var g in entrances)
        {
            g.SetActive(true);
        }

        int spawnIndex = 0;

        foreach (var e in enemiesToSpawn)
        {
            GameObject g = Instantiate(e, spawnPoints[spawnIndex].position, e.transform.rotation);
            spawnIndex++;
            if (spawnIndex >= spawnPoints.Length)
            {
                spawnIndex = 0;
            }
            enemiesLeft.Add(g);
        }
        ready = true;
    }

    void Deactivate()
    {
        ready = false;
        foreach (var g in entrances)
        {
            g.SetActive(false);
        }
    }
}
