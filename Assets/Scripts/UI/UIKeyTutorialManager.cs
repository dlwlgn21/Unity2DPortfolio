using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _rollKeyText = _UIKeys.transform.GetChild(4).gameObject;
        _attackKeyText.transform.localScale = Vector3.zero;
        _blockKeyText.transform.localScale = Vector3.zero;
        _rollKeyText.transform.localScale = Vector3.zero;
        Object.DontDestroyOnLoad(_UIKeys);
    }

    public void ActiveMoveKeys()        
    { 
        _moveKeys.SetActive(true);
        _moveKeys.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic);
    }
    public void UnactiveMoveKeys()     { _moveKeys.SetActive(false); }
    public void ActiveBattleKeys()      
    { 
        _battleKeys.SetActive(true);
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
        _attackKeyText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic);
    }
    private void UnactiveAttackKeyText()
    {
        _attackKeyText.SetActive(false);
    }
    private void ActiveRollKeyText()
    {
        _rollKeyText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic);
    }
    private void UnactiveRollKeyText()
    {
        _rollKeyText.SetActive(false);
    }


    private void ActiveBlockKeyText()
    {
        _blockKeyText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic);
    }
    private void UnactiveBlockKeyText()
    {
        _blockKeyText.SetActive(false);
    }

    #endregion
}
