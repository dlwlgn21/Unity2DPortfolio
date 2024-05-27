using define;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private AudioSource[] _audioSources = new AudioSource[(int)ESoundType.COUNT];

    private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
    public void Init()
    {
        GameObject root = new GameObject() { name = "@SoundManager" };
        GameObject sfxGo = new GameObject() { name = "SFX" };
        GameObject bgmGo = new GameObject() { name = "BGM" };
        sfxGo.transform.parent = root.transform;
        bgmGo.transform.parent = root.transform;
        _audioSources[(int)ESoundType.SFX] = sfxGo.AddComponent<AudioSource>();
        _audioSources[(int)ESoundType.BGM] = bgmGo.AddComponent<AudioSource>();
        UnityEngine.Object.DontDestroyOnLoad(root);
        _audioSources[(int)ESoundType.SFX].loop = false;
        _audioSources[(int)ESoundType.BGM].loop = true;
    }


    public void Play(string path, ESoundType eType = ESoundType.SFX)
    {
        if (!path.Contains("Sound"))
        {
            path = $"Sound/{path}";
        }
        AudioClip clip = GetOrAddAudioClip(path);
        switch (eType)
        {
            case ESoundType.SFX:
                if (clip == null)
                {
                    return;
                }
                _audioSources[(int)eType].PlayOneShot(clip);
                break;
            case ESoundType.BGM:
                AudioSource bgmSource = _audioSources[(int)eType];
                if (bgmSource.isPlaying)
                {
                    bgmSource.Stop();
                }
                bgmSource.Play();
                break;
        }
    }

    public void Clear()
    {
        _clips.Clear();
    }

    private AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip clip;
        if (_clips.TryGetValue(path, out clip) == false)
        {
            clip = Managers.Resources.Load<AudioClip>(path);
            if (clip == null)
            {
                Debug.Assert(false);
            }
            return clip;
        }
        return clip;
    }

}
