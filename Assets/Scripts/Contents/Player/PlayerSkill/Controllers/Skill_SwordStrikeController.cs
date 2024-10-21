using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Skill_SwordStrikeController : Skill_BaseController
{
    private Skill_SwordStrikeObject _swordStrike;
    public override void Init()
    {
        _eSkillType = EActiveSkillType.Cast_SwordStrike;
        InitByEActiveSkillType();
        if (_swordStrike == null)
        {
            _swordStrike = Managers.Resources.Instantiate<Skill_SwordStrikeObject>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Cast_SwordStrike][0].objectPrefabPath);
            DontDestroyOnLoad(_swordStrike.gameObject);
            _swordStrike.gameObject.name = Managers.PlayerSkill.GetSkillObjectName(EActiveSkillType.Cast_SwordStrike);

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
            _swordStrike.UseSkill(_pc.CastSwordStrikePoint.position, _pc.ELookDir);
            _isUsingSkill = false;
        }
    }
}
