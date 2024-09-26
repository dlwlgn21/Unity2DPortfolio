using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EPlayerSkill
{
    ROLL,
    SPAWN_REAPER,
    SPAWN_SHOOTER,
    COUNT
}
public abstract class BasePlayerSkillController : MonoBehaviour
{
    [SerializeField] protected UIPlayerCoolTimer _uiCoolTimerImg;

    protected PlayerController _pc;
    protected EPlayerSkill _eSkillType;

    protected float _initCoolTime;
    public float SkillCoolTimeInSec { get; protected set; }
    public bool IsPossibleDoSkill { get; protected set; }
    public abstract void Init();

    private void Awake()
    {
        Init();
        _pc = transform.parent.GetComponent<PlayerController>();
    }

    protected bool IsPosibbleValidStateToDoSkill()
    {
        if (IsPossibleDoSkill && (_pc.ECurrentState == EPlayerState.IDLE || _pc.ECurrentState == EPlayerState.RUN))
        {
            return true;
        }
        return false;
    }
    
    protected void DoSkill(EPlayerSkill eType)
    {
        switch (eType)
        {
            case EPlayerSkill.ROLL:
                {
                    _pc.ChangeState(EPlayerState.ROLL);
                    ProcessSkillLogic();
                }
                break;
            case EPlayerSkill.SPAWN_REAPER:
                {
                    _pc.ChangeState(EPlayerState.CAST_SPAWN);
                    ProcessSkillLogic();
                }
                break;
            case EPlayerSkill.SPAWN_SHOOTER:
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
        IsPossibleDoSkill = false;
        StartCoroutine(AfterGivenCoolTimePossibleDoSkillCo(SkillCoolTimeInSec));
    }

    protected IEnumerator AfterGivenCoolTimePossibleDoSkillCo(float coolTimeInSec)
    {
        Debug.Assert(IsPossibleDoSkill == false);
        yield return new WaitForSeconds(coolTimeInSec);
        IsPossibleDoSkill = true;
    }
}
