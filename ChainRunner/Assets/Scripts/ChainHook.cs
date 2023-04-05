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
    [SerializeField]
    private LayerMask illegalLayer;
    [SerializeField]
    private LayerMask bringBackLayer;
    [SerializeField]
    private Transform ChainHookRotator;
    [SerializeField]
    private Transform ValidityDisplayer;
    [SerializeField]
    private SpriteRenderer ValidityDisplayerSprite;

    private Vector2 originalDir;
    private Vector2 endPoint;
    private bool hitObject = false;
    private bool bringBack = false;
    private GameObject objectHit;

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
        if ((!p.LockFlipDirection) && Input.GetMouseButtonDown(1))
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

        
        if (hookSent && !retractingHook)
        {
            ValidityDisplayerSprite.color = new Color(ValidityDisplayerSprite.color.r, ValidityDisplayerSprite.color.g, ValidityDisplayerSprite.color.b, 0f);
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
        } else if(!hookSent && !retractingHook)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ChainHookRotator.position = p.transform.position;
            ChainHookRotator.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - ChainHookRotator.position);
            ValidityDisplayer.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - ChainHookRotator.position);
            RaycastHit2D hit = Physics2D.Raycast(p.transform.position, ChainHookRotator.up, distance, layer);
            RaycastHit2D checkBringBack = Physics2D.Raycast(p.transform.position, ChainHookRotator.up, distance, bringBackLayer);
            RaycastHit2D checkIllegal = Physics2D.Raycast(p.transform.position, ChainHookRotator.up, distance, illegalLayer);
            float[] distances = { distance + 1, distance + 1, distance + 1, distance };

            if (hit)
            {
                distances[0] = Vector2.Distance(p.transform.position, hit.point);
            }
            if (checkBringBack)
            {
                distances[1] = Vector2.Distance(p.transform.position, checkBringBack.point);
            }
            if (checkIllegal)
            {
                distances[2] = Vector2.Distance(p.transform.position, checkIllegal.point);
            }
            int lowest = GetIndexOfLowestValue(distances);
            switch (lowest)
            {
                case 0:
                    ValidityDisplayerSprite.color = new Color(ValidityDisplayerSprite.color.r, ValidityDisplayerSprite.color.g, ValidityDisplayerSprite.color.b, .6f);
                    break;
                case 1:
                    ValidityDisplayerSprite.color = new Color(ValidityDisplayerSprite.color.r, ValidityDisplayerSprite.color.g, ValidityDisplayerSprite.color.b, .6f);
                    break;
                default:
                    ValidityDisplayerSprite.color = new Color(ValidityDisplayerSprite.color.r, ValidityDisplayerSprite.color.g, ValidityDisplayerSprite.color.b, .08f);
                    break;
            }
        }
    }
    public int GetIndexOfLowestValue(float[] arr)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < value)
            {
                index = i;
                value = arr[i];
            }
        }
        return index;
    }
    void StartHook()
    {
        p.StartHook();
        hookSent = true;
        transform.position = p.transform.position;
        hookPoint.position = startPoint.position;
        hitObject = false;
        bringBack = false;
        l.gameObject.SetActive(true);
        hookPoint.gameObject.SetActive(true);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        
        RaycastHit2D hit = Physics2D.Raycast(p.transform.position, transform.up, distance, layer);
        RaycastHit2D checkBringBack = Physics2D.Raycast(p.transform.position, transform.up, distance, bringBackLayer);
        RaycastHit2D checkIllegal = Physics2D.Raycast(p.transform.position, transform.up, distance, illegalLayer);
        float[] distances = { distance + 1, distance + 1, distance + 1, distance+.1f};

        if (hit)
        {
            distances[0] = Vector2.Distance(p.transform.position, hit.point);
        }
        if (checkBringBack)
        {
            distances[1] = Vector2.Distance(p.transform.position, checkBringBack.point);
        }
        if (checkIllegal)
        {
            distances[2] = Vector2.Distance(p.transform.position, checkIllegal.point);
        }
        int lowest = GetIndexOfLowestValue(distances);
        switch (lowest)
        {
            case 0:
                endPoint = hit.point;
                hitObject = true;
                objectHit = hit.collider.gameObject;
                var p = objectHit.GetComponent<Projectile>();
                if (p != null)
                {
                    print("hit");
                    p.rb.velocity = Vector2.zero;
                }
                break;
            case 1:
                
                endPoint = checkBringBack.point;
                objectHit = checkBringBack.collider.gameObject;
                bringBack = true;
                hitObject = false;
                break;
            case 2:
                endPoint = checkIllegal.point;
                hitObject = false;
                break;
            default:
                endPoint = startPoint.position + transform.up * (distance - 1);
                hitObject = false;
                break;
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
                if(bringBack && objectHit!=null)
                    objectHit.transform.position = Vector3.MoveTowards(hookPoint.position, startPoint.position, retractSpeed * 100 * Time.deltaTime);
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
        if (objectHit != null)
        {
            var p = objectHit.GetComponent<Projectile>();
            if (p != null)
            {
                Destroy(objectHit);
            }
        }
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
