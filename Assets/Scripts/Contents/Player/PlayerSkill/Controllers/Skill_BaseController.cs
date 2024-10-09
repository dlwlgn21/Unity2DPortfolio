using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
using System.Runtime.InteropServices;

public abstract class Skill_BaseController : MonoBehaviour
{
    protected PlayerController _pc;
    protected ESkillType _eSkillType;
    public ESkillSlot ECurrentSkillSlot { get; set; }
    protected float _initCoolTimeInSec;
    public float SkillCoolTimeInSec { get; private set; }
    public bool IsCanUseSkill { get; protected set; }
    protected bool _isUsingSkill;
    public abstract void Init();

    private void Awake()
    {
        Init();
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Assert(_pc != null);
    }

    public abstract bool TryUseSkill();
    protected void StartCountdownCoolTime()
    {
        IsCanUseSkill = false;
        StartCoroutine(StartCountDownCoolTimeCo(SkillCoolTimeInSec));
    }
    protected bool IsValidStateToUseSkill()
    {
        if (IsCanUseSkill &&
            (_pc.ECurrentState == EPlayerState.IDLE || _pc.ECurrentState == EPlayerState.RUN))
        {
            return true;
        }
        return false;
    }
    protected IEnumerator StartCountDownCoolTimeCo(float coolTimeInSec)
    {
        Debug.Assert(IsCanUseSkill == false);
        yield return new WaitForSeconds(coolTimeInSec);
        IsCanUseSkill = true;
    }

    protected void InitByESkillType(ESkillType eType)
    {
        _eSkillType = eType;
        ECurrentSkillSlot = ESkillSlot.Count;
        _isUsingSkill = false;
        _initCoolTimeInSec = Managers.Data.SkillInfoDict[(int)eType].coolTime;
        SkillCoolTimeInSec = _initCoolTimeInSec;
        IsCanUseSkill = true;
    }

}
