using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum SoundNames:byte
{
    Explosion, PopCollect, Oof, Pop, Push, PopcorenBurst
}
[Serializable]
public class CorrectedSoundClip
{
    public AudioClip clip;
    [Range(0, 1)] public float volume;

}
[Serializable]
public class Sound
{
    public SoundNames name;
    public CorrectedSoundClip[] audioClips;
    public float minPitch;
    public float maxPitch;
}
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] private Sound[] sounds;
    [SerializeField] private OneShotSound oneShotSoundPreFab;
    public static void PlayOneShotSoundAt(SoundNames name, Vector3 position, float delay = 0)
    {
        instance.StartCoroutine(instance.PlayOneShotSoundAtCoroutine(name,position,delay));     
    }

    private IEnumerator  PlayOneShotSoundAtCoroutine(SoundNames name, Vector3 position,float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        CorrectedSoundClip clip = null;
        float minPitch = 0;
        float maxPitch = 0;
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sounds[i].name == name)
            {
                clip = sound.audioClips[UnityEngine.Random.Range(0, sound.audioClips.Length)];
                minPitch = sound.minPitch; maxPitch = sound.maxPitch;
            }
        }
        if (clip == null)
        {
            Debug.LogError("NO SOUND FOUND!");
        }
        else
        {
            OneShotSound oneShotSound = Instantiate(oneShotSoundPreFab, position, Quaternion.identity);
            oneShotSound.Play(clip.clip, minPitch, maxPitch, clip.volume);
        }

    }

}
