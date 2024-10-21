using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Skill_BlackFlameController : Skill_BaseController
{
    private Skill_BlackFlameObject _blackFlame;
    public override void Init()
    {
        _eSkillType = EActiveSkillType.Cast_BlackFlame;
        InitByEActiveSkillType();
        if (_blackFlame == null)
        {
            _blackFlame = Managers.Resources.Instantiate<Skill_BlackFlameObject>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Cast_BlackFlame][0].objectPrefabPath);
            DontDestroyOnLoad(_blackFlame.gameObject);
            _blackFlame.gameObject.name = Managers.PlayerSkill.GetSkillObjectName(EActiveSkillType.Cast_BlackFlame);

        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerCastAnimValidTiming;
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerCastAnimValidTiming;
    }
    public override bool TryUseSkill()
    {
        if (IsValidStateAndManaToUseSkill())
        {
            _pc.ChangeState(EPlayerState.SkillCast);
            StartCountdownCoolTime();
            _isUsingSkill = true;
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerCastAnimValidTiming;
    }
    void OnPlayerCastAnimValidTiming()
    {
        if (_isUsingSkill)
        {
            _blackFlame.UseSkill(_pc.CastBlackFlamePoint.position, _pc.ELookDir);
            _pc.AddOppositeForceByLookDir(new Vector2(2f, 3f));
            _isUsingSkill = false;
        }
    }
}
