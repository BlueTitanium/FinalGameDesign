using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHook : MonoBehaviour
{
    [SerializeField]
    private PlayerController p;
    [SerializeField]
    private LineRenderer l;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    public Transform hookPoint;
    [SerializeField]
    private Rigidbody2D hookRb;
    [SerializeField]
    private float hookSpeed = 20f;
    [SerializeField]
    private float retractSpeed = 20f;
    [SerializeField]
    private float retractDelay = .4f;
    [SerializeField]
    private float distance = 10f;
    [SerializeField]
    private float maxDistanceAllowed = 20f;
    [SerializeField]
    private LayerMask layer;
    private Vector2 originalDir;
    private Vector2 endPoint;
    private bool hitObject = false;
    public bool hookSent = false;
    public bool retractingHook = false;
    // Start is called before the first frame update
    void Start()
    {
        l.gameObject.SetActive(false);
        hookPoint.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
           
            if (!hookSent && !retractingHook)
            {
                StartHook();
            } else if (hookSent && !retractingHook)
            {
                StartCoroutine(RetractHook(0));
            }
            
        }
        
        l.SetPosition(0, startPoint.position);
        l.SetPosition(1, hookPoint.position);
        
        if(hookSent && !retractingHook)
        {
            Vector2 checkDir = ((Vector2) hookPoint.position - endPoint).normalized;
            if (Vector2.Dot(checkDir, originalDir) > 0)
            {
                hookRb.velocity = Vector2.zero;
                hookPoint.position = endPoint;
                if (!hitObject)
                {
                    StartCoroutine(RetractHook(retractDelay));
                } else
                {
                    p.GrappleToLocation(originalDir, endPoint);
                }
            }
            
            if (Vector2.Distance(hookPoint.position, startPoint.position) > maxDistanceAllowed)
            {
                StartCoroutine(RetractHook(0));

            }
            //if (Input.GetMouseButtonDown(0) && hitObject)
            //{
            //    p.GrappleToLocation(originalDir, endPoint);
            //}
        }
    }

    void StartHook()
    {
        p.StartHook();
        hookSent = true;
        transform.position = p.transform.position;
        hookPoint.position = startPoint.position;
        hitObject = false;
        l.gameObject.SetActive(true);
        hookPoint.gameObject.SetActive(true);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        
        RaycastHit2D hit;
        hit = Physics2D.Raycast(startPoint.position, transform.up, distance, layer);
        if (hit)
        {
            endPoint = hit.point;
            hitObject = true;
        } else
        {
            endPoint = startPoint.position + transform.up * distance;
        }
        originalDir = transform.up.normalized;
        hookRb.velocity = transform.up.normalized * hookSpeed;
    }
    public void TryRetractHook()
    {
        StartCoroutine(RetractHook(0));
    }
    IEnumerator RetractHook(float delay)
    {
        if (!retractingHook)
        {
            hitObject = false;
            hookRb.velocity = Vector2.zero;
            retractingHook = true;
            yield return new WaitForSeconds(delay);
            while (Vector2.Distance(hookPoint.position, startPoint.position) > .1f)
            {
                hookPoint.position = Vector3.MoveTowards(hookPoint.position, startPoint.position, retractSpeed * 100 * Time.deltaTime);
                yield return null;
            }
            EndHook();
            retractingHook = false;
            hookSent = false;
        }
    }

    void EndHook()
    {
        l.gameObject.SetActive(false);
        hookPoint.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Hello");
        if (collision.gameObject.CompareTag("Ground"))
        {
            hookRb.velocity = Vector2.zero;
        }
    }
}
