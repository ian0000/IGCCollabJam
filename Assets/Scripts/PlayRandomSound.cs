using UnityEngine;

/// <summary>
/// Added this class but I don't know that I'll actually end up using it
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips;

    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        var randint = Random.Range(0, _clips.Length);
        _audioSource.PlayOneShot(_clips[randint]);
    }
}
