using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFade : MonoBehaviour
{
    public Light2D light;
    public float FadeTime = 1f;
    public float Length = 3f;
    public float intensity = .35f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        float timer = 0;
        while (timer < FadeTime)
        {
            timer += Time.deltaTime;
            light.intensity = Mathf.Lerp(0, intensity, timer / FadeTime);
            yield return null;
        }
        if (Length != 0)
        {
            yield return new WaitForSeconds(Length);
            timer = 0;
            while (timer < FadeTime)
            {
                timer += Time.deltaTime;
                light.intensity = Mathf.Lerp(intensity, 0, timer / FadeTime);
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
