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
        InitByESkillType(ESkillType.Cast_BlackFlame_LV1);
        if (_blackFlame == null)
        {
            _blackFlame = Managers.Resources.Instantiate<Skill_BlackFlameObject>(Managers.Data.SkillInfoDict[(int)_eSkillType].objectPrefabPath);
            DontDestroyOnLoad(_blackFlame.gameObject);
            _blackFlame.gameObject.name = Managers.PlayerSkill.GetSkillObjectName(ESkillType.Cast_BlackFlame_LV1);

        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerCastAnimValidTiming;
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerCastAnimValidTiming;
    }
    public override bool TryUseSkill()
    {
        if (IsValidStateToUseSkill())
        {
            _pc.ChangeState(EPlayerState.SKILL_CAST);
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

    public override void LevelUpSkill(ESkillType eType)
    {
        base.LevelUpSkill(eType);
        _blackFlame.ESkillType = eType;
    }
}
