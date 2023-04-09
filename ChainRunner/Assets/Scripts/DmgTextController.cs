using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgTextController : MonoBehaviour
{
    public static DmgTextController d;
    public GameObject DamagePopup;
    int nextSortingIndex = 10;
    float sortingOrderTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        d = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(sortingOrderTimer > 0)
        {
            sortingOrderTimer -= Time.deltaTime;
            if(sortingOrderTimer <= 0)
            {
                nextSortingIndex = 10;
            }
        }
    }

    public void SpawnDmgText(float amt, Vector2 pos)
    {
        Vector2 actualPos = new Vector2(pos.x + Random.Range(-.5f, .5f), pos.y+Random.Range(-.5f,.5f));
        GameObject g = Instantiate(DamagePopup, pos, DamagePopup.transform.rotation);
        Destroy(g, 2f);
        var t = g.transform.GetChild(0).GetComponent<TextMeshPro>();
        t.text = "" + (int)amt;
        t.sortingOrder = nextSortingIndex;
        sortingOrderTimer = .5f;
        nextSortingIndex += 1;
        switch (amt)
        {
            case < 10:
                t.fontSize = 4;
                t.color = new Color(1,1,1);  
                break;
            case < 50:
                t.fontSize = 5;
                t.color = new Color(1, 1, 0);
                break;
            case < 100:
                t.fontSize = 6;
                t.color = new Color(1, .75f, 0);
                break;
            case < 200:
                t.fontSize = 7;
                t.color = new Color(1, .25f, 0);
                break;
            case < 500:
                t.fontSize = 7.5f;
                t.color = new Color(1f, 0, .25f);
                break;
            case < 1000:
                t.fontSize = 8;
                t.color = new Color(0.5f, 0, 0);
                break;
            default:
                t.fontSize = 8.5f;
                t.color = new Color(0, 0, 0);
                break;
        }
    }
}
