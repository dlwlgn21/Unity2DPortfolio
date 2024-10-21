using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public enum ETutorialCountText
{ 
    Roll,
    BackAttack,
    Block,
}

public enum ETutorialTraning
{
    AttackTraning,
    RollTraning,
    BackAttackTraning,
    BlockTraning,
    SkillTraning,
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject _rollOpenInteractBox;
    [SerializeField] GameObject _backAttackOpenInteractBox;
    [SerializeField] GameObject _blockOpenInteractBox;
    [SerializeField] GameObject _skillOpenInteractBox;
    
    
    GameObject _UIKeys;
    GameObject _moveKeys;
    GameObject _attackTitleText;
    GameObject _attackExplainText;
    GameObject _blockTitleText;
    GameObject _blockExplainText;
    GameObject _rollTitleText;
    GameObject _rollCountTexts;
    GameObject _rollExplainText;
    GameObject _backAttackTitleTexts;


    TextMeshProUGUI _rollCountText;
    TextMeshProUGUI _blockCountText;
    TextMeshProUGUI _backAttackCountText;
    TextMeshProUGUI _successText;
    TextMeshProUGUI _skillTutorialExplainText;
    
    const float TW_SCALE_END_VALUE = 1f; 
    const float TW_SCALE_DURATION_VALUE = 0.5f;
    const float COUNT_TEXT_TW_SCALE_END_VALUE = 2f;
    const float COUNT_TEXT_TW_SCALE_DURATION_VALUE = 0.5f;

    private void Awake()
    {
        #region Assign
        _UIKeys = transform.Find("UIKeys").gameObject;
        _moveKeys = _UIKeys.transform.Find("MoveKeys").gameObject;
        _moveKeys.transform.localScale = Vector3.zero;
        _attackTitleText = _UIKeys.transform.Find("AttackTutorialTitleText").gameObject;
        _attackExplainText = _attackTitleText.transform.Find("AttackTutorialExplainText").gameObject;
        _blockTitleText = _UIKeys.transform.Find("BlockTutorialTitleText").gameObject;
        _blockExplainText = _blockTitleText.transform.Find("BlockTutorialExplainText ").gameObject;
        _blockCountText = _blockTitleText.transform.Find("BlockCountText").gameObject.GetComponent<TextMeshProUGUI>();
        _rollTitleText = _UIKeys.transform.Find("RollTutorialTitleText").gameObject;
        _rollExplainText = _rollTitleText.transform.Find("RollTutorialExplainText ").gameObject;
        _rollCountTexts = _rollTitleText.transform.Find("RollCountTexts").gameObject;
        _rollCountText = _rollCountTexts.transform.Find("RollCountText").gameObject.GetComponent<TextMeshProUGUI>();
        _backAttackTitleTexts = _UIKeys.transform.Find("BackAttackTutorialTitleText").gameObject;
        _backAttackCountText = _backAttackTitleTexts.transform.Find("BackAttackCountText").gameObject.GetComponent<TextMeshProUGUI>();
        _successText = _UIKeys.transform.Find("SuccessText").gameObject.GetComponent<TextMeshProUGUI>();
        _skillTutorialExplainText = _UIKeys.transform.Find("SkillTutorialExplainText").GetComponent<TextMeshProUGUI>();
        #endregion
        #region SetToZero
        _attackTitleText.transform.localScale = Vector3.zero;
        _attackExplainText.transform.localScale = Vector3.zero;
        _blockTitleText.transform.localScale = Vector3.zero;
        _blockExplainText.transform.localScale = Vector3.zero;
        _rollTitleText.transform.localScale = Vector3.zero;
        _rollCountTexts.transform.localScale = Vector3.zero;
        _rollExplainText.transform.localScale = Vector3.zero;
        _backAttackTitleTexts.transform.localScale = Vector3.zero;
        _skillTutorialExplainText.transform.localScale = Vector3.zero;
        _successText.transform.localScale = Vector3.zero;
        #endregion
        UnactiveMoveKeys();
    }

    public void ActiveMoveKeys()        
    { 
        _moveKeys.SetActive(true);
        _moveKeys.transform.DOScale(TW_SCALE_END_VALUE, TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
        PlayActiveSound();
    }
    public void UnactiveMoveKeys()     { _moveKeys.SetActive(false); }


    public void ActiveAttackTutorialUIObjects()
    {
        ActiveAttackKeyTexts();
        _rollOpenInteractBox.SetActive(false);
        PlayActiveSound();
    }
    public void UnactiveAttackTutorialUIObjects()
    {
        UnactiveAttackKeyTexts();
        _rollOpenInteractBox.SetActive(true);
    }
    public void ActiveRollTutorialUIObjects()
    {
        ActiveRollTexts();
        _backAttackOpenInteractBox.SetActive(false);
        PlayActiveSound();
    }
    public void UnactiveRollTutorialUIObjects()
    {
        UnactiveRollTexts();
        _backAttackOpenInteractBox.SetActive(true);
    }

    public void ActiveBackAttackTutorialUIObjects()
    {
        _backAttackTitleTexts.transform.DOScale(TW_SCALE_END_VALUE, TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
        _blockOpenInteractBox.SetActive(false);
        PlayActiveSound();
    }

    public void UnctiveBackAttackTutorialUIObjects()
    {
        _backAttackTitleTexts.SetActive(false);
        _blockOpenInteractBox.SetActive(true);
    }

    public void ActiveBlockTutorialUIObjects()
    {
        ActiveBlockTexts();
        _skillOpenInteractBox.SetActive(false);
        PlayActiveSound();
    }

    public void UnactiveBlockTutorialUIObjects()
    {
        UnactiveBlockTexts();
        _skillOpenInteractBox.SetActive(true);
    }

    public void ActiveSkillTutorialUIObjects()
    {
        ActiveSkillText();
        PlayActiveSound();
    }

    public void UnactiveSkillTutorialUIObjects()
    {
        UnactiveSkillText();
    }

    public void ActiveSuccessText(ETutorialTraning eType)
    {
        switch (eType)
        {
            case ETutorialTraning.AttackTraning:
                _successText.text = "기본공격 트레이닝 성공!";
                break;
            case ETutorialTraning.RollTraning:
                _successText.text = "구르기 트레이닝 성공!";
                break;
            case ETutorialTraning.BackAttackTraning:
                _successText.text = "백어택 트레이닝 성공!";
                break;
            case ETutorialTraning.BlockTraning:
                _successText.text = "막기 트레이닝 성공!";
                break;
            case ETutorialTraning.SkillTraning:
                _successText.text = "스킬 트레이닝 성공!\n3초 후에 현실로 점프합니다!";
                break;
        }
        Managers.Sound.Play(DataManager.SFX_QUEST_SUCESS);
        _successText.transform.DOScale(TW_SCALE_END_VALUE, 1.5f).SetEase(Ease.OutElastic).OnComplete(OnSuccessTextScaleTWEnd);
    }



    public void IncreaseCountText(ETutorialCountText eType, int count)
    {
        switch (eType)
        {
            case ETutorialCountText.Roll:
                _rollCountText.text = count.ToString();
                _rollCountText.gameObject.transform.DOScale(COUNT_TEXT_TW_SCALE_END_VALUE, COUNT_TEXT_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic).OnComplete(OnRollCountTextScaleTWEnd);
                break;
            case ETutorialCountText.BackAttack:
                _backAttackCountText.text = count.ToString();
                _backAttackCountText.gameObject.transform.DOScale(COUNT_TEXT_TW_SCALE_END_VALUE, COUNT_TEXT_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic).OnComplete(OnBackAttackCountTextScaleTWEnd);
                break;
            case ETutorialCountText.Block:
                _blockCountText.text = count.ToString();
                _blockCountText.gameObject.transform.DOScale(COUNT_TEXT_TW_SCALE_END_VALUE, COUNT_TEXT_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic).OnComplete(OnBlockCountTextScaleTWEnd);
                break;
        }
    }



    public void OnRollCountTextScaleTWEnd()
    {
        _rollCountText.transform.localScale = Vector3.one;
    }
    public void OnBackAttackCountTextScaleTWEnd()
    {
        _backAttackCountText.transform.localScale = Vector3.one;
    }
    public void OnBlockCountTextScaleTWEnd()
    {
        _blockCountText.transform.localScale = Vector3.one;
    }

    #region Private

    void ActiveAttackKeyTexts()
    {
        StartScaleTW(_attackTitleText.transform);
        StartScaleTW(_attackExplainText.transform);
    }
    void UnactiveAttackKeyTexts()
    {
        _attackTitleText.SetActive(false);
        _attackExplainText.SetActive(false);
    }
    void ActiveRollTexts()
    {
        StartScaleTW(_rollTitleText.transform);
        StartScaleTW(_rollCountTexts.transform);
        StartScaleTW(_rollExplainText.transform);
    }
    void UnactiveRollTexts()
    {
        _rollTitleText.SetActive(false);
        _rollCountTexts.SetActive(false);
        _rollExplainText.SetActive(false);
    }

    void ActiveBlockTexts()
    {
        StartScaleTW(_blockTitleText.transform);
        StartScaleTW(_blockExplainText.transform);
    }
    void UnactiveBlockTexts()
    {
        _blockExplainText.SetActive(false);
        _blockTitleText.SetActive(false);
    }

    void ActiveSkillText()
    {
        _skillTutorialExplainText.DOScale(1f, 1f).SetEase(Ease.InOutElastic);
    }
    void UnactiveSkillText()
    {
        _skillTutorialExplainText.DOScale(0f, 1f).SetEase(Ease.InOutElastic);
    }
    void OnSuccessTextScaleTWEnd()
    {
        _successText.transform.DOScale(0f, 1f);
    }

    void StartScaleTW(Transform transform)
    {
        Managers.Tween.StartUIScaleTW(transform, TW_SCALE_END_VALUE, TW_SCALE_DURATION_VALUE);
    }

    void PlayActiveSound()
    {
        Managers.Sound.Play(DataManager.SFX_UI_DIALOG_BOX_POPUP);
    }
    #endregion
}
