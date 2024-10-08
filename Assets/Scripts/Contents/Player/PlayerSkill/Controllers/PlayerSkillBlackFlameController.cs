using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillBlackFlameController : BasePlayerSkillController
{
    private PlayerSkillBlackFlameObject _blackFlame;
    public override void Init()
    {
        InitByESkillType(ESkillType.Cast_BlackFlame);
        if (_blackFlame == null)
        {
            _blackFlame = Managers.Resources.Instantiate<PlayerSkillBlackFlameObject>("Prefabs/Player/Skills/SkillBlackFlameObject");
            DontDestroyOnLoad(_blackFlame.gameObject);
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
            _blackFlame.CastBlackFlame(_pc.CastBlackFlamePoint.position, _pc.ELookDir);
            _pc.AddOppositeForceByLookDir(new Vector2(2f, 3f));
            _isUsingSkill = false;
        }
    }
}
