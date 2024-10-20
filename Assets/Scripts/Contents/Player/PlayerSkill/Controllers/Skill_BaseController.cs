using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
using System.Runtime.InteropServices;
using data;
using UnityEngine.Events;


public enum EDeniedUseSkillCause
{ 
    NotEnoughMana,
    CoolTime,
    Count
}

public abstract class Skill_BaseController : MonoBehaviour
{
    static public UnityAction<EDeniedUseSkillCause> DeniedUseSkillEventHandler;
    protected PlayerController _pc;
    protected ESkillType _eSkillType;
    public ESkillSlot ECurrentSkillSlot { get; set; }
    protected float _initCoolTimeInSec;
    public float SkillCoolTimeInSec { get; private set; }
    public bool IsCanUseSkillByCoolTime { get; protected set; }
    protected bool _isUsingSkill;

    data.SkillInfo _skillInfo;
    Coroutine _countDownCo;
    public abstract void Init();

    public void InitForNextSceneLoad()
    {
        if (_countDownCo != null)
            StopCoroutine(_countDownCo);
        _countDownCo = null;
        IsCanUseSkillByCoolTime = true;
    }

    private void Awake()
    {
        Init();
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Assert(_pc != null);
    }

    public abstract bool TryUseSkill();
    public virtual void LevelUpSkill(ESkillType eType)
    {
        InitSkillInfoByTypeAndCoolTime(eType);
    }
    protected void StartCountdownCoolTime()
    {
        IsCanUseSkillByCoolTime = false;
        _countDownCo = StartCoroutine(StartCountDownCoolTimeCo(SkillCoolTimeInSec));
    }
    protected bool IsValidStateAndManaToUseSkill()
    {
        if (!Managers.Dialog.IsTalking &&
             !Managers.Pause.IsPaused &&
            IsCanUseSkillByCoolTime &&
            (_pc.ECurrentState == EPlayerState.Idle || _pc.ECurrentState == EPlayerState.Run) &&
             _pc.Stat.Mana >= _skillInfo.manaCost)
        {
            _pc.Stat.Mana -= _skillInfo.manaCost;
            return true;
        }

        if (_eSkillType != ESkillType.Roll)
        {
            if (_pc.Stat.Mana < _skillInfo.manaCost)
            {
                if (DeniedUseSkillEventHandler != null)
                    DeniedUseSkillEventHandler.Invoke(EDeniedUseSkillCause.NotEnoughMana);
            }
            if (_countDownCo != null)
            {
                if (DeniedUseSkillEventHandler != null)
                    DeniedUseSkillEventHandler.Invoke(EDeniedUseSkillCause.CoolTime);
            }
        }
        return false;
    }
    protected IEnumerator StartCountDownCoolTimeCo(float coolTimeInSec)
    {
        Debug.Assert(IsCanUseSkillByCoolTime == false);
        yield return new WaitForSeconds(coolTimeInSec);
        IsCanUseSkillByCoolTime = true;
        _countDownCo = null;
    }

    protected void InitByESkillType(ESkillType eType)
    {
        InitSkillInfoByTypeAndCoolTime(eType);
        ECurrentSkillSlot = ESkillSlot.Count;
        _isUsingSkill = false;
        IsCanUseSkillByCoolTime = true;
    }

    void InitSkillInfoByTypeAndCoolTime(ESkillType eType)
    {
        _eSkillType = eType;
        _skillInfo = Managers.Data.SkillInfoDict[(int)eType];
        _initCoolTimeInSec = Managers.Data.SkillInfoDict[(int)eType].coolTime;
        SkillCoolTimeInSec = _initCoolTimeInSec;
    }
}
