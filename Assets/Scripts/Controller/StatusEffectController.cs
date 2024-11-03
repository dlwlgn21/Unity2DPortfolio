using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class StatusEffectController : MonoBehaviour
{
    static public UnityAction<EAttackStatusEffect, float> PlayerStatusEffectEventHandler;
    int _lastBurnedDamage;
    PlayerController _pc;
    Coroutine _burnCoOrNull;
    Coroutine _slowCoOrNull;

    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
        PlayerController.PlayerStatusEffectEventHandler -= OnHittedByMonsterAttack;
        PlayerController.PlayerStatusEffectEventHandler += OnHittedByMonsterAttack;
    }
    void OnHittedByMonsterAttack(BaseMonsterController mc)
    {
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
                if (!_pc.IsBurned && _burnCoOrNull == null)
                {
                    _lastBurnedDamage = mc.Stat.Attack;
                    _burnCoOrNull = StartCoroutine(BurnPlayerCo((int)mc.Stat.BurnTimeInSec));
                    if (PlayerStatusEffectEventHandler != null)
                        PlayerStatusEffectEventHandler.Invoke(EAttackStatusEffect.Burn, mc.Stat.BurnTimeInSec);
                }
                break;
            case EAttackStatusEffect.Slow:
                if (!_pc.IsSlowState && _slowCoOrNull == null)
                {
                    _slowCoOrNull = StartCoroutine(StartSlowStateCountdownCo(mc.Stat.SlowTimeInSec));
                    if (PlayerStatusEffectEventHandler != null)
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
        _slowCoOrNull = null;
    }
    IEnumerator BurnPlayerCo(int burnTimeInSec)
    {
        _pc.IsBurned = true;
        for (int timeInSec = 0; timeInSec < burnTimeInSec; ++timeInSec)
        {
            yield return new WaitForSeconds(1f);
            _pc.ActualDamgedFromMonsterAttack(Mathf.Max((int)(_lastBurnedDamage * 0.5f), 1));
            int randIdx = UnityEngine.Random.Range(0, 1);
            if (randIdx % 2 == 0)
                Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_1_PATH);
            else
                Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_2_PATH);
        }
        _pc.IsBurned = false;
        _burnCoOrNull = null;
    }
}
