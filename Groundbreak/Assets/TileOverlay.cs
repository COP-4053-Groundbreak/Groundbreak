using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlay : MonoBehaviour
{
    float animationPoint;
    float startTime;
    bool isAtPeak = false;
    public float speed = 1;
    public Color startColor;
    public Color endColor;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = this.GetComponent<Renderer>().material;
        mat.color = Color.clear;
        /*startTime = Time.time;
        startColor = new Color(255,255,255,164);
        endColor = new Color(0, 193, 255, 164);*/
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float transitionDuration = 0.5f;

    // Gradually transition between 0 (no danger) and 1 (danger)
    // over the specified transition duration in seconds.
    animationPoint = Mathf.MoveTowards(
                     animationPoint,
                     isAtPeak ? 0f : 1f,
                     Time.deltaTime/transitionDuration);

    // Sine wave.
    float frac = Mathf.Sin(Time.time - startTime) * speed;

    // Our colour at the low trough of the sine wave is determined by our danger level.
    var lowColor = Color.Lerp(startColor, endColor, animationPoint);

    // Our colour at the high peak of the sine wave is always orange.
    // (Though you could lerp a highColor variable for this purpose too).
    mat.SetColor("_EmissiveColor", Color.Lerp(lowColor, endColor, frac)); */
    }
}
