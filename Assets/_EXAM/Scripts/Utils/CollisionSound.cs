using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource audioSrc;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (audioSrc == null)
            return;

        float impactVolume = collision.relativeVelocity.magnitude;

        if (impactVolume >= 1.0f)
        {
            audioSrc.volume = impactVolume;
            audioSrc.Play();
        }
    }
}
