using UnityEngine;

public class AmbientRandomSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] sounds;

    public float minDelay = 4f;
    public float maxDelay = 10f;

    private float timer;
    private float nextTime;

    void Start()
    {
        SetNextTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextTime)
        {
            PlayRandom();
            timer = 0f;
            SetNextTime();
        }
    }

    void PlayRandom()
    {
        if (sounds.Length == 0) return;

        int index = Random.Range(0, sounds.Length);

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.2f, 0.5f);

        audioSource.PlayOneShot(sounds[index]);
    }

    void SetNextTime()
    {
        nextTime = Random.Range(minDelay, maxDelay);
    }
}