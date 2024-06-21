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


    public void OnPlayerChangeState(EPlayerState eState)
    {
        switch (eState)
        {
            case EPlayerState.IDLE:
                break;
            case EPlayerState.RUN:
                break;
            case EPlayerState.ROLL:
                Managers.Sound.Play(DataManager.SFX_PLAYER_ROLLING_PATH);
                break;
            case EPlayerState.JUMP:
                break;
            case EPlayerState.CLIMB:
                break;
            case EPlayerState.FALL:
                break;
            case EPlayerState.FALL_TO_TWICE_JUMP:
                break;
            case EPlayerState.TWICE_JUMP_TO_FALL:
                break;
            case EPlayerState.LAND:
                break;
            case EPlayerState.NORMAL_ATTACK_1:
                Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_1_PATH);
                break;
            case EPlayerState.NORMAL_ATTACK_2:
                Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_2_PATH);
                break;
            case EPlayerState.NORMAL_ATTACK_3:
                Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_3_PATH);
                break;
            case EPlayerState.CAST_LAUNCH:
                break;
            case EPlayerState.CAST_SPAWN:
                break;
            case EPlayerState.HITTED:
                int randIdx = UnityEngine.Random.Range(0, 1);
                if (randIdx % 2 == 0)
                {
                    Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_1_PATH);
                }
                else
                {
                    Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_2_PATH);
                }
                break;
            case EPlayerState.BLOCKING:
                break;
            case EPlayerState.BLOCK_SUCESS:
                break;
            case EPlayerState.DIE:
                break;
            case EPlayerState.COUNT:
                break;
        }
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
