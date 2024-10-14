using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
using System.Runtime.InteropServices;
using data;

public abstract class Skill_BaseController : MonoBehaviour
{
    protected PlayerController _pc;
    protected ESkillType _eSkillType;
    public ESkillSlot ECurrentSkillSlot { get; set; }
    protected float _initCoolTimeInSec;
    public float SkillCoolTimeInSec { get; private set; }
    public bool IsCanUseSkillByCoolTime { get; protected set; }
    protected bool _isUsingSkill;

    data.SkillInfo _skillInfo;
    public abstract void Init();

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
        StartCoroutine(StartCountDownCoolTimeCo(SkillCoolTimeInSec));
    }
    protected bool IsValidStateAndManaToUseSkill()
    {
        if (IsCanUseSkillByCoolTime &&
            (_pc.ECurrentState == EPlayerState.Idle || _pc.ECurrentState == EPlayerState.Run) &&
             _pc.Stat.Mana >= _skillInfo.manaCost)
        {
            _pc.Stat.Mana -= _skillInfo.manaCost;
            return true;
        }
        return false;
    }
    protected IEnumerator StartCountDownCoolTimeCo(float coolTimeInSec)
    {
        Debug.Assert(IsCanUseSkillByCoolTime == false);
        yield return new WaitForSeconds(coolTimeInSec);
        IsCanUseSkillByCoolTime = true;
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
