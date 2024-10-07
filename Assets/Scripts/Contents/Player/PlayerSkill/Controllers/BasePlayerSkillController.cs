using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;



public abstract class BasePlayerSkillController : MonoBehaviour
{
    [SerializeField] protected UI_PlayerCoolTimer _uiCoolTimerImg;

    protected PlayerController _pc;
    protected ESkillType _eSkillType;

    protected float _initCoolTime;
    public float SkillCoolTimeInSec { get; protected set; }
    public bool IsCanUseSkill { get; protected set; }
    public abstract void Init();

    private void Awake()
    {
        Init();
        _pc = transform.parent.GetComponent<PlayerController>();
    }

    protected bool IsValidStateToUseSkill()
    {
        if (IsCanUseSkill && (_pc.ECurrentState == EPlayerState.IDLE || _pc.ECurrentState == EPlayerState.RUN))
        {
            return true;
        }
        return false;
    }
    
    protected void UseSkill(ESkillType eType)
    {
        switch (eType)
        {
            case ESkillType.Roll:
                {
                    _pc.ChangeState(EPlayerState.ROLL);
                    ProcessSkillLogic();
                }
                break;
            case ESkillType.Spawn_Reaper:
                {
                    _pc.ChangeState(EPlayerState.CAST_SPAWN);
                    ProcessSkillLogic();
                }
                break;
            case ESkillType.Spawn_Panda:
                {
                    _pc.ChangeState(EPlayerState.CAST_LAUNCH);
                    ProcessSkillLogic();
                }
                break;
        }
    }

    private void ProcessSkillLogic()
    {
        _uiCoolTimerImg.StartCoolTime(SkillCoolTimeInSec);
        IsCanUseSkill = false;
        StartCoroutine(AfterGivenCoolTimePossibleDoSkillCo(SkillCoolTimeInSec));
    }

    protected IEnumerator AfterGivenCoolTimePossibleDoSkillCo(float coolTimeInSec)
    {
        Debug.Assert(IsCanUseSkill == false);
        yield return new WaitForSeconds(coolTimeInSec);
        IsCanUseSkill = true;
    }
}
