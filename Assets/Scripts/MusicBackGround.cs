using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBackGround : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PlayerPrefs.GetString("music") == "Yes" && !_audioSource.isPlaying)
            _audioSource.Play();
        else if (PlayerPrefs.GetString("music") == "No" && _audioSource.isPlaying)
            _audioSource.Stop();
    }
}
