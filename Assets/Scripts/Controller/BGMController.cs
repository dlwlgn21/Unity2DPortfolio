using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    AudioSource _source;
    Coroutine _fadeOutCo;
    const float FADE_OUT_BGM_TIME_IN_SEC = 1.0f;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (_fadeOutCo != null)
        {
            StopCoroutine(_fadeOutCo);
            _fadeOutCo = StartCoroutine(FadeOutBGMCo(clip));
            return;
        }
        if (_source.isPlaying)
        {
            _fadeOutCo = StartCoroutine(FadeOutBGMCo(clip));
            return;
        }
        _source.clip = clip;
        _source.Play();
    }

    IEnumerator FadeOutBGMCo(AudioClip clip)
    {
        float elapsedTime = 0f;
        float oriVolume = _source.volume;
        while (_source.volume > 0f)
        {
            _source.volume = Mathf.Lerp(1, 0, elapsedTime / FADE_OUT_BGM_TIME_IN_SEC);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _source.Stop();
        _source.volume = oriVolume;
        _source.clip = clip;
        _source.Play();
        _fadeOutCo = null;
    }
}
