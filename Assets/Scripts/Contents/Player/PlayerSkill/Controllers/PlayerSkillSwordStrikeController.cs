using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSwordStrikeController : BasePlayerSkillController
{
    private PlayerSkillSwordStrikeObject _swordStrike;
    public override void Init()
    {
        InitByESkillType(ESkillType.Cast_SwordStrike);
        if (_swordStrike == null)
        {
            _swordStrike = Managers.Resources.Instantiate<PlayerSkillSwordStrikeObject>("Prefabs/Player/Skills/SkillSwordStikeObject");
            DontDestroyOnLoad(_swordStrike.gameObject);
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
            _swordStrike.CastSwordStrike(_pc.CastSwordStrikePoint.position, _pc.ELookDir);
            _isUsingSkill = false;
        }
    }
}
