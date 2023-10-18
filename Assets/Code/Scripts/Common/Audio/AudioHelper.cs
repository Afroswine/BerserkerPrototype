using UnityEngine;

public static class AudioHelper
{
    public static float VolumeModifier = 1f;

    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        GameObject audioObject = new GameObject("Audio2D");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume * VolumeModifier;

        audioSource.Play();
        Object.Destroy(audioObject, clip.length);

        return audioSource;
    }

    public static AudioSource PlayClip3D(AudioClip clip, float volume, Transform transform)
    {
        GameObject audioObject = new GameObject("Audio3D");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume * VolumeModifier;

        return audioSource;
    }
}
