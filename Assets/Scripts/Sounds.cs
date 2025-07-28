using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] _sounds;
    AudioSource _audioSource;

    public void Initialize(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }   
    
    public void PlaySound(int num)
    {
        _audioSource.pitch = Random.Range(0.95f, 1.05f);
        _audioSource.PlayOneShot(_sounds[num]);
    }
}
