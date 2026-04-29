using UnityEngine;
using System.Collections;

public class AppearEffect : MonoBehaviour
{
    public float duration = 1f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float t = 0;

        transform.localScale = Vector3.zero;

        Color c = sr.color;
        c.a = 0;
        sr.color = c;

        while (t < duration)
        {
            t += Time.deltaTime;
            float v = t / duration;

            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, v);

            c.a = v;
            sr.color = c;

            yield return null;
        }
    }
}