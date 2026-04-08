using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    public float growSpeed = 4f;
    public float fadeSpeed = 3f;
    public float lifeTime = 0.4f;

    private SpriteRenderer sr;
    private Color startColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.localScale += Vector3.one * growSpeed * Time.deltaTime;

        if (sr != null)
        {
            Color c = sr.color;
            c.a -= fadeSpeed * Time.deltaTime;
            sr.color = c;
        }
    }
}