using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHook : MonoBehaviour
{
    [SerializeField]
    private PlayerController p;
    public LineRenderer l;
    public Transform startPoint;
    public Transform hookPoint;

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
            StartHook();
        }
        
        l.SetPosition(0, startPoint.position);
        l.SetPosition(1, hookPoint.position);
        
    }

    void StartHook()
    {
        transform.position = p.transform.position;
        l.gameObject.SetActive(true);
        hookPoint.gameObject.SetActive(true);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
    }
}
