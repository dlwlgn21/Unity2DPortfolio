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

public abstract class PlayerSkillController : MonoBehaviour
{
    [SerializeField] protected UIPlayerCoolTimer _uiCoolTimerImg;

    protected PlayerController _pc;
    protected EPlayerSkill _eSkillType;

    protected float _initCoolTime;
    public float SkillCoolTime { get; protected set; }
    public bool IsPossibleDoSkill { get; protected set; }
    public abstract void Init();

    private void Awake()
    {
        Init();
        _pc = transform.parent.GetComponent<PlayerController>();
    }

    protected IEnumerator AfterGivenCoolTimePossibleDoSkillCo(float coolTimeInSec)
    {
        Debug.Assert(IsPossibleDoSkill == false);
        yield return new WaitForSeconds(coolTimeInSec);
        IsPossibleDoSkill = true;
    }
}
