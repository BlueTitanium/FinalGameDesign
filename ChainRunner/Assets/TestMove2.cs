using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove2 : MonoBehaviour
{
    private float startPos;
    private float length;
    private GameObject cam;
    [SerializeField]private float parallaxEffectVert;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        startPos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.y * (1-parallaxEffectVert));
        float distance = (cam.transform.position.y * parallaxEffectVert);

        transform.position = new Vector3(transform.position.x, startPos + distance, transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -=length;
        }

    }
}