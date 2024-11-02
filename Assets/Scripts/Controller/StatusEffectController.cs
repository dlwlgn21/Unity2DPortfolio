using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class StatusEffectController : MonoBehaviour
{
    static public UnityAction<EAttackStatusEffect, float> PlayerStatusEffectEventHandler;
    const float BURN_TIME_IN_SEC = 3f;
    int _lastBurnedDamage;
    PlayerController _pc;
    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
        MonsterProjectileController.MonsterProjectileHitPlayerEventHandelr -= OnHittedByMonsterAttack;
        MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr -= OnHittedByMonsterAttack;
        MonsterProjectileController.MonsterProjectileHitPlayerEventHandelr += OnHittedByMonsterAttack;
        MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr += OnHittedByMonsterAttack;
    }
    void OnHittedByMonsterAttack(BaseMonsterController mc)
    {
        // TODO : BlockSuccess 일 때 무시하게 바꿔야 함.
        if (!_pc.IsValidStateToChangeHitState())
            return;

        switch (mc.Stat.EStatusEffectType)
        {
            case EAttackStatusEffect.None:
                break;
            case EAttackStatusEffect.Knockback:
                Vector2 knockbackForce = mc.Stat.KnockbackForce;
                Vector2 dir = mc.transform.position - transform.position;
                if (dir.x > 0)
                    _pc.RigidBody.AddForce(new Vector2(-knockbackForce.x, knockbackForce.y), ForceMode2D.Impulse);
                else
                    _pc.RigidBody.AddForce(knockbackForce, ForceMode2D.Impulse);
                break;
            case EAttackStatusEffect.Blind:
                Managers.FullScreenEffect.StartFullScreenEffect(EFullScreenEffectType.MONSTER_BLIND_EFFECT);
                break;
            case EAttackStatusEffect.Burn:
                if (!_pc.IsBurned)
                {
                    _lastBurnedDamage = mc.Stat.Attack;
                    StartCoroutine(BurnPlayerCo());
                    PlayerStatusEffectEventHandler?.Invoke(EAttackStatusEffect.Burn, BURN_TIME_IN_SEC);
                }
                break;
            case EAttackStatusEffect.Slow:
                if (!_pc.IsSlowState)
                {
                    StartCoroutine(StartSlowStateCountdownCo(mc.Stat.SlowTimeInSec));
                    PlayerStatusEffectEventHandler?.Invoke(EAttackStatusEffect.Slow, mc.Stat.SlowTimeInSec);
                }
                break;
            case EAttackStatusEffect.Parallysis:
                _pc.ChangeState(EPlayerState.HitByStatusParallysis);
                break;
            default:
                Debug.DebugBreak();
                break;
        }
    }

    IEnumerator StartSlowStateCountdownCo(float slowTimeInSec)
    {
        _pc.IsSlowState = true;
        yield return new WaitForSeconds(slowTimeInSec);
        _pc.IsSlowState = false;
    }
    IEnumerator BurnPlayerCo()
    {
        _pc.IsBurned = true;
        yield return new WaitForSeconds(1f);
        _pc.ActualDamgedFromMonsterAttack(Mathf.Max((int)(_lastBurnedDamage * 0.5f), 1));
        Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_1_PATH);
        yield return new WaitForSeconds(1f);
        _pc.ActualDamgedFromMonsterAttack(Mathf.Max((int)(_lastBurnedDamage * 0.5f), 1));
        Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_2_PATH);
        yield return new WaitForSeconds(1f);
        _pc.ActualDamgedFromMonsterAttack(Mathf.Max((int)(_lastBurnedDamage * 0.5f), 1));
        Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_1_PATH);
        _pc.IsBurned = false;
    }
}
