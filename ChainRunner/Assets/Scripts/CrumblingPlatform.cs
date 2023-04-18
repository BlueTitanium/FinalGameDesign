using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float time = 1f;
    bool isCrumbling = false;
    [SerializeField] private BoxCollider2D b;
    [SerializeField] private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        if (!isCrumbling)
        {
            isCrumbling = true;
            yield return new WaitForSeconds(time);
            b.enabled = false;
            sr.enabled = false;
            yield return new WaitForSeconds(time*2);
            b.enabled = true;
            sr.enabled = true;
            yield return new WaitForSeconds(time/2);
            isCrumbling = false;
        }
    }

}
