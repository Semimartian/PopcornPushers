using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    public void Play(AudioClip clip, float pitchMin, float pitchMax, float volume = 1)
    {
        source.clip = clip;
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.volume = volume;
        source.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
