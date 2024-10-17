using define;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private AudioSource[] _audioSources = new AudioSource[(int)ESoundType.Count];

    private Dictionary<string, AudioClip> _clips = new();
    public void Init()
    {
        if (GameObject.Find("@SoundManager") == null)
        {
            GameObject root = new() { name = "@SoundManager" };
            GameObject sfxGo = new() { name = "SFX" };
            GameObject bgmGo = new() { name = "BGM" };
            sfxGo.transform.parent = root.transform;
            bgmGo.transform.parent = root.transform;
            _audioSources[(int)ESoundType.Sfx] = sfxGo.AddComponent<AudioSource>();
            _audioSources[(int)ESoundType.Bgm] = bgmGo.AddComponent<AudioSource>();
            UnityEngine.Object.DontDestroyOnLoad(root);
            _audioSources[(int)ESoundType.Sfx].loop = false;
            _audioSources[(int)ESoundType.Bgm].loop = true;
            PlayerController.PlayerChangeStateEventHandler -= Managers.Sound.OnPlayerChangeState;
            PlayerController.PlayerChangeStateEventHandler += Managers.Sound.OnPlayerChangeState;
        }
    }

    public void OnPlayerChangeState(EPlayerState eState)
    {
        switch (eState)
        {
            case EPlayerState.Idle:
                break;
            case EPlayerState.Run:
                break;
            case EPlayerState.Roll:
                Managers.Sound.Play(DataManager.SFX_PLAYER_ROLLING_PATH);
                break;
            case EPlayerState.Jump:
                Managers.Sound.Play(DataManager.SFX_PLAYER_JUMP_PATH);
                break;
            case EPlayerState.Climb:
                Managers.Sound.Play(DataManager.SFX_PLAYER_LAND_PATH);
                break;
            case EPlayerState.Fall:
                break;
            case EPlayerState.FallToTwiceJump:
                Managers.Sound.Play(DataManager.SFX_PLAYER_JUMP_PATH);
                break;
            case EPlayerState.TwiceJumpToFall:
                break;
            case EPlayerState.Land:
                Managers.Sound.Play(DataManager.SFX_PLAYER_LAND_PATH);
                break;
            case EPlayerState.NormalAttack_1:
                Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_1_PATH);
                break;
            case EPlayerState.NormalAttack_2:
                Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_2_PATH);
                break;
            case EPlayerState.NormalAttack_3:
                Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_3_PATH);
                break;
            case EPlayerState.SkillCast:
                break;
            case EPlayerState.SkillSpawn:
                break;
            case EPlayerState.HitByMelleAttack:
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
            case EPlayerState.Block:
                break;
            case EPlayerState.BlockSucces:
                Managers.Sound.Play(DataManager.SFX_PLAYER_BLOCK_SUCESS_PATH);
                break;
            case EPlayerState.Die:
                break;
            case EPlayerState.Count:
                break;
        }
    }

    public void Play(string path, ESoundType eType = ESoundType.Sfx)
    {
        if (!path.Contains("Sound"))
        {
            path = $"Sound/{path}";
        }
        AudioClip clip = GetOrAddAudioClip(path);
        switch (eType)
        {
            case ESoundType.Sfx:
                if (clip == null)
                {
                    Debug.DebugBreak();
                    return;
                }
                _audioSources[(int)eType].PlayOneShot(clip);
                break;
            case ESoundType.Bgm:
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
        // TODO : 이거 문제 일어날 수 있으니까 나중에 Clear 잘 짜자.
        PlayerController.PlayerChangeStateEventHandler -= Managers.Sound.OnPlayerChangeState;
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
