using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
using System.Runtime.InteropServices;

public abstract class BasePlayerSkillController : MonoBehaviour
{
    protected PlayerController _pc;
    protected ESkillType _eSkillType;

    protected float _initCoolTimeInSec;
    public float SkillCoolTimeInSec { get; private set; }
    public bool IsCanUseSkill { get; protected set; }
    public abstract void Init();

    private void Awake()
    {
        Init();
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Assert(_pc != null);
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

    public abstract bool TryUseSkill();
    protected void ProcessSkillLogic()
    {
        IsCanUseSkill = false;
        StartCoroutine(StartCountDownCoolTimeCo(SkillCoolTimeInSec));
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
        _initCoolTimeInSec = Managers.Data.SkillInfoDict[(int)eType].coolTime;
        SkillCoolTimeInSec = _initCoolTimeInSec;
        IsCanUseSkill = true;
    }
}
