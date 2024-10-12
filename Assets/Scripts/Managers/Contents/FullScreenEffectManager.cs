using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EFullScreenEffectType
{
    SCENE_TRANSITION,
    MONSTER_BLIND_EFFECT,
}
public class FullScreenEffectManager
{
    private GameObject _fadeout;
    private Image _blackFadeImg;
    private Image _blindEffectImg;
    private const float FADE_OUT_TIME = 3f;
    private const float BLIND_EFFECT_FADE_OUT_TIME = 1f;

    private bool _isStartBlindEffect = false;
    public void Init()
    {
        if (_fadeout == null)
        {
            GameObject fadeOut = Managers.Resources.Load<GameObject>("Prefabs/UI/UIFadeOut");
            Debug.Assert(fadeOut != null);
            _fadeout = Object.Instantiate(fadeOut);
            _fadeout.name = "UIFadeOut";
            Object.DontDestroyOnLoad(_fadeout);
            _blackFadeImg = Utill.GetComponentInChildrenOrNull<Image>(_fadeout, "BlackFadeImg");
            _blindEffectImg = Utill.GetComponentInChildrenOrNull<Image>(_fadeout, "BlindEffectImg");
            _blackFadeImg.enabled = false;
            _blindEffectImg.enabled = false;
            _fadeout.SetActive(false);
        }
    }

    public void StartFullScreenEffect(EFullScreenEffectType eType)
    {
        switch (eType)
        {
            case EFullScreenEffectType.SCENE_TRANSITION:
                {
                    _blackFadeImg.enabled = true;
                    _fadeout.SetActive(true);
                    _blackFadeImg.DOFade(0f, FADE_OUT_TIME).SetEase(Ease.InOutExpo).OnComplete(OnSceneTransitionFadeEffectCompleted);
                }
                break;
            case EFullScreenEffectType.MONSTER_BLIND_EFFECT:
                if (_isStartBlindEffect == false)
                {
                    _isStartBlindEffect = true;
                    _blindEffectImg.enabled = true;
                    _fadeout.SetActive(true);
                    _blindEffectImg.DOFade(0f, BLIND_EFFECT_FADE_OUT_TIME).SetEase(Ease.InOutExpo).OnComplete(OnBlindEffectCompleted);
                }
                break;
            default:
                break;
        }
    }

    private void OnSceneTransitionFadeEffectCompleted()
    {
        _blackFadeImg.color = Color.black;
        _blackFadeImg.enabled = false;
        _fadeout.SetActive(false);
    }
    private void OnBlindEffectCompleted()
    {
        _isStartBlindEffect = false;
        _blindEffectImg.color = Color.black;
        _blindEffectImg.enabled = false;
        _fadeout.SetActive(false);
    }

}
