using UnityEngine;

// TODO - maybe this should just be in AudioHelper...

[RequireComponent(typeof(AudioSource))]
public class AudioClipRandomizer : MonoBehaviour
{
    [Tooltip("If true, play a random clip on Awake.")]
    [SerializeField] bool _playOnAwake = true;
    [Tooltip("The audio clips to choose from.")]
    [SerializeField] AudioClip[] _clips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_playOnAwake)
            RandomizeAndPlay();
    }

    private void RandomizeClip()
    {
        int index = Mathf.RoundToInt(Random.Range(0, _clips.Length));
        //Debug.Log(gameObject + "RandomizeClip() set clip to: _clips[" + index + "]");
        _audioSource.clip = _clips[index];
    }

    public void RandomizeAndPlay()
    {
        RandomizeClip();
        _audioSource.Play();
    }
}
