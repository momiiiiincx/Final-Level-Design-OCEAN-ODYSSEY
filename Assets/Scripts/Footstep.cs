using UnityEngine;

public class Footstep : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepClips;

    public float stepDelay = 0.4f; // เว้นช่วงเสียง

    private float timer;

    void Update()
    {
        float move = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));

        if (move > 0.1f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                PlayFootstep();
                timer = stepDelay;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[index]);
    }
}