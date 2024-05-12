using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ETutorialCountText
{ 
    ROLL,
    BACK_ATTACK,
    BLOCK,
}

public enum ETutorialTraning
{
    ATTACK_TRAINING,
    ROLL_TRAINING,
    BACK_ATTACK_TRAINING,
    BLOCK_TRAINING,
}

public class UIKeyTutorialManager
{
    public GameObject KeyZ { get; private set; }
    public GameObject KeyX { get; private set; }
    public GameObject KeyC { get; private set; }

    private GameObject _UIKeys;
    private GameObject _moveKeys;
    private GameObject _battleKeys;
    private GameObject _attackKeyText;
    private GameObject _blockKeyText;
    private GameObject _rollKeyText;
    private GameObject _rollCountTexts;
    private GameObject _backAttackTexts;
    private TextMeshProUGUI _rollCountText;
    private TextMeshProUGUI _blockCountText;
    private TextMeshProUGUI _backAttackCountText;
    private TextMeshProUGUI _successText;

    private OpenInteractBox _rollOpenInteractBox;
    private OpenInteractBox _backAttackOpenInteractBox;
    private OpenInteractBox _blockOpenInteractBox;
    
    private const float ACTIVE_OBJECTS_TW_SCALE_END_VALUE = 1f; 
    private const float ACTIVE_OBJECTS_TW_SCALE_DURATION_VALUE = 0.5f;
    private const float COUNT_TEXT_TW_SCALE_END_VALUE = 2f;
    private const float COUNT_TEXT_TW_SCALE_DURATION_VALUE = 0.5f;

    public void Init()
    {
        GameObject ori = Managers.Resources.Load<GameObject>("Prefabs/UI/UIKeys");
        _UIKeys = Object.Instantiate(ori);
        _moveKeys = _UIKeys.transform.GetChild(0).gameObject;
        _battleKeys = _UIKeys.transform.GetChild(1).gameObject;
        KeyZ = _battleKeys.transform.GetChild(0).gameObject;
        KeyX = _battleKeys.transform.GetChild(1).gameObject;
        KeyC = _battleKeys.transform.GetChild(2).gameObject;
        _moveKeys.transform.localScale = Vector3.zero;

        UnactiveMoveKeys();
        UnactiveBattleKeys();

        _attackKeyText = _UIKeys.transform.GetChild(2).gameObject;
        _blockKeyText = _UIKeys.transform.GetChild(3).gameObject;
        _blockCountText = _blockKeyText.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _rollKeyText = _UIKeys.transform.GetChild(4).gameObject;
        _rollCountTexts = _UIKeys.transform.GetChild(5).gameObject;
        _rollCountText = _rollCountTexts.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _backAttackTexts = _UIKeys.transform.GetChild(6).gameObject;
        _backAttackCountText = _backAttackTexts.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _successText = _UIKeys.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>();
        
        _attackKeyText.transform.localScale = Vector3.zero;
        _blockKeyText.transform.localScale = Vector3.zero;
        _rollKeyText.transform.localScale = Vector3.zero;
        _rollCountTexts.transform.localScale = Vector3.zero;
        _backAttackTexts.transform.localScale = Vector3.zero;
        _successText.transform.localScale = Vector3.zero;
        Object.DontDestroyOnLoad(_UIKeys);
    }
    public void Clear()
    {
        _UIKeys.SetActive(false);
    }
    public void ActiveMoveKeys()        
    { 
        _moveKeys.SetActive(true);
        _moveKeys.transform.DOScale(ACTIVE_OBJECTS_TW_SCALE_END_VALUE, ACTIVE_OBJECTS_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
    }
    public void UnactiveMoveKeys()     { _moveKeys.SetActive(false); }
    public void ActiveBattleKeys()      
    { 
        _battleKeys.SetActive(true);
        KeyZ.SetActive(true);
        KeyX.SetActive(true);
        KeyC.SetActive(true);
    }
    public void UnactiveBattleKeys()   { _battleKeys.SetActive(false); }

    public void ActiveAttackKeyTutorial()
    {
        ActiveAttackKey();
        ActiveAttackKeyText();
    }
    public void UnactiveAttackKeyTutorial()
    {
        UnactiveAttackKey();
        UnactiveAttackKeyText();
    }
    public void ActiveRollKeyTutorial()
    {
        ActiveRollKey();
        ActiveRollKeyText();
    }
    public void UnactiveRollKeyTutorial()
    {
        UnactiveRollKey();
        UnactiveRollKeyText();
    }

    public void ActiveBackAttackTutorial()
    {
        _backAttackTexts.transform.DOScale(ACTIVE_OBJECTS_TW_SCALE_END_VALUE, ACTIVE_OBJECTS_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
    }

    public void UnactiveBackAttackTutorial()
    {
        _backAttackTexts.SetActive(false);
    }

    public void ActiveSuccessText(ETutorialTraning eType)
    {
        switch (eType)
        {
            case ETutorialTraning.ATTACK_TRAINING:
                _successText.text = "기본공격 트레이닝 성공!";
                break;
            case ETutorialTraning.ROLL_TRAINING:
                _successText.text = "구르기 트레이닝 성공!";
                break;
            case ETutorialTraning.BACK_ATTACK_TRAINING:
                _successText.text = "백어택 트레이닝 성공!";
                break;
            case ETutorialTraning.BLOCK_TRAINING:
                _successText.text = "막기 트레이닝 성공!";
                break;
        }
        _successText.transform.DOScale(ACTIVE_OBJECTS_TW_SCALE_END_VALUE, 1.5f).SetEase(Ease.OutElastic).OnComplete(OnSuccessTextScaleTWEnd);

    }

    public void OnSuccessTextScaleTWEnd()
    {
        _successText.transform.DOScale(0f, 0.2f);
    }

    public void IncreaseCountText(ETutorialCountText eType, int count)
    {
        switch (eType)
        {
            case ETutorialCountText.ROLL:
                _rollCountText.text = count.ToString();
                _rollCountText.gameObject.transform.DOScale(COUNT_TEXT_TW_SCALE_END_VALUE, COUNT_TEXT_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic).OnComplete(OnRollCountTextScaleTWEnd);
                break;
            case ETutorialCountText.BACK_ATTACK:
                _backAttackCountText.text = count.ToString();
                _backAttackCountText.gameObject.transform.DOScale(COUNT_TEXT_TW_SCALE_END_VALUE, COUNT_TEXT_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic).OnComplete(OnBackAttackCountTextScaleTWEnd);
                break;
            case ETutorialCountText.BLOCK:
                _blockCountText.text = count.ToString();
                _blockCountText.gameObject.transform.DOScale(COUNT_TEXT_TW_SCALE_END_VALUE, COUNT_TEXT_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic).OnComplete(OnBlockCountTextScaleTWEnd);
                break;
        }
    }

    public void ActiveBlockKeyTutorial()
    {
        ActiveBlockKey();
        ActiveBlockKeyText();
    }

    public void UnactiveBlockKeyTutorial()
    {
        UnactiveBlockKey();
        UnactiveBlockKeyText();
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

    #region PRIVATE_SECTION
    private void ActiveAttackKey()
    {
        ActiveBattleKeys();
        KeyX.SetActive(false);
        KeyC.SetActive(false);
    }
    private void UnactiveAttackKey()
    {
        KeyZ.SetActive(false);
        UnactiveBattleKeys();
    }
    private void ActiveBlockKey()
    {
        ActiveBattleKeys();
        KeyZ.SetActive(false);
        KeyC.SetActive(false);
    }
    private void UnactiveBlockKey()
    {
        KeyX.SetActive(false);
        UnactiveBattleKeys();
    }
    private void ActiveRollKey()
    {
        ActiveBattleKeys();
        KeyZ.SetActive(false);
        KeyX.SetActive(false);
    }

    private void UnactiveRollKey()
    {
        KeyC.SetActive(false);
        UnactiveBattleKeys();
    }


    private void ActiveAttackKeyText()
    {
        _attackKeyText.transform.DOScale(ACTIVE_OBJECTS_TW_SCALE_END_VALUE, ACTIVE_OBJECTS_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
    }
    private void UnactiveAttackKeyText()
    {
        _attackKeyText.SetActive(false);
    }
    private void ActiveRollKeyText()
    {
        _rollKeyText.transform.DOScale(ACTIVE_OBJECTS_TW_SCALE_END_VALUE, ACTIVE_OBJECTS_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
        _rollCountTexts.transform.DOScale(ACTIVE_OBJECTS_TW_SCALE_END_VALUE, ACTIVE_OBJECTS_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
    }
    private void UnactiveRollKeyText()
    {
        _rollKeyText.SetActive(false);
        _rollCountTexts.SetActive(false);
    }

    private void ActiveBlockKeyText()
    {
        _blockKeyText.transform.DOScale(ACTIVE_OBJECTS_TW_SCALE_END_VALUE, ACTIVE_OBJECTS_TW_SCALE_DURATION_VALUE).SetEase(Ease.OutElastic);
    }
    private void UnactiveBlockKeyText()
    {
        _blockKeyText.SetActive(false);
    }

    #endregion
}
