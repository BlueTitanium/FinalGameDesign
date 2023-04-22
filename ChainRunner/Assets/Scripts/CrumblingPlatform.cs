using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public float time = 1f;
    bool isCrumbling = false;
    [SerializeField] private BoxCollider2D b;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator a;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isCrumbling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        if (!isCrumbling)
        {
            isCrumbling = true;
            a.SetTrigger("Crumble");
            yield return new WaitUntil(() => a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "CrumblingPlatformsCrumble");
            yield return new WaitUntil(() => a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "CrumblingPlatformsGone");
            yield return new WaitForSeconds(time);
            a.SetTrigger("Uncrumble");
            yield return new WaitUntil(() => a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "CrumblingPlatformsCrumble");
            yield return new WaitUntil(() => a.GetCurrentAnimatorClipInfo(0)[0].clip.name == "CrumblingPlatforms");
            isCrumbling = false;
        }
    }

}
