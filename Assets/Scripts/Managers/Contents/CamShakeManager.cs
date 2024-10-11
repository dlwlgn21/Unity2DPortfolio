using UnityEngine;
using Cinemachine;

public enum ECamShakeType
{
    PLAYER_HITTED_BY_MONSTER,
    PLAYER_BLOCK_SUCCES,
    MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK,
    MONSTER_HITTED_BY_KNOCKBACK_BOMB,
    MONSTER_HITTED_BY_REAPER_ATTACK,
}
public class CamShakeManager
{
    private GameObject _camShakeManager;
    private CinemachineImpulseSource _playerImpulseSource;
    private CinemachineImpulseSource _monsterImpulseSource;


    private const float PLAYER_HITTED_BY_MONSTER_FORCE = 0.7f;
    private const float PLAYER_BLOCK_SUCCES_FORCE = 0.8f;
    private const float MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_FORCE = 1f;
    private const float MONSTER_HITTED_BY_KNOCKBACK_BOMB_FORCE = 0.9f;
    private const float MONSTER_HITTED_BY_REAPER_ATTACK_FORCE = 1.5f;
    public void Init()
    {
        if (GameObject.Find("@CamShakeManager") == null)
        {
            _camShakeManager = Managers.Resources.Load<GameObject>("Prefabs/Managers/CamShakeManager");
            _camShakeManager.name = "@CamShakeManager";
            _playerImpulseSource = Utill.GetComponentInChildrenOrNull<CinemachineImpulseSource>(_camShakeManager, "PlayerImpulseSource");
            _monsterImpulseSource = Utill.GetComponentInChildrenOrNull<CinemachineImpulseSource>(_camShakeManager, "MonsterImpulseSource");
            Object.DontDestroyOnLoad(_camShakeManager);
            PlayerController.HitEffectEventHandler -= Managers.CamShake.OnPlayerHittedByMonsterNormalAttack;
            BaseMonsterController.HittedByNormalAttackNoArgsEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerNormalAttack;
            NormalMonsterController.MonsterChangeStateEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerSkill;
            PlayerController.HitEffectEventHandler += Managers.CamShake.OnPlayerHittedByMonsterNormalAttack;
            BaseMonsterController.HittedByNormalAttackNoArgsEventHandler += Managers.CamShake.OnMonsterHittedByPlayerNormalAttack;
            NormalMonsterController.MonsterChangeStateEventHandler += Managers.CamShake.OnMonsterHittedByPlayerSkill;

        }
    }
    public void Clear()
    {
        PlayerController.HitEffectEventHandler -= Managers.CamShake.OnPlayerHittedByMonsterNormalAttack;
        BaseMonsterController.HittedByNormalAttackNoArgsEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerNormalAttack;
        NormalMonsterController.MonsterChangeStateEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerSkill;
    }

    public void OnMonsterHittedByPlayerNormalAttack()
    {
        CamShake(ECamShakeType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK);
    }

    public void OnPlayerHittedByMonsterNormalAttack(EPlayerState eState)
    {
        switch (eState)
        {
            case EPlayerState.HITTED_MELLE_ATTACK:
                CamShake(ECamShakeType.PLAYER_HITTED_BY_MONSTER);
                break;
            case EPlayerState.BLOCK_SUCESS:
                CamShake(ECamShakeType.PLAYER_BLOCK_SUCCES);
                break;
        }
        CamShake(ECamShakeType.PLAYER_HITTED_BY_MONSTER);
    }

    public void OnMonsterHittedByPlayerSkill(ENormalMonsterState eState)
    {
        switch (eState)
        {
            case ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                Managers.CamShake.CamShake(ECamShakeType.MONSTER_HITTED_BY_KNOCKBACK_BOMB);
                break;
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                CamShake(ECamShakeType.MONSTER_HITTED_BY_REAPER_ATTACK);
                break;
        }
    }

    public void CamShake(ECamShakeType eType)
    {
        switch (eType)
        {
            case ECamShakeType.PLAYER_HITTED_BY_MONSTER:
                _playerImpulseSource.GenerateImpulseWithForce(PLAYER_HITTED_BY_MONSTER_FORCE);
                break;
            case ECamShakeType.PLAYER_BLOCK_SUCCES:
                _monsterImpulseSource.GenerateImpulseWithForce(PLAYER_BLOCK_SUCCES_FORCE);
                break;
            case ECamShakeType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK:
                _monsterImpulseSource.GenerateImpulseWithForce(MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_FORCE);
                break;
            case ECamShakeType.MONSTER_HITTED_BY_KNOCKBACK_BOMB:
                _monsterImpulseSource.GenerateImpulseWithForce(MONSTER_HITTED_BY_KNOCKBACK_BOMB_FORCE);
                break;
            case ECamShakeType.MONSTER_HITTED_BY_REAPER_ATTACK:
                _monsterImpulseSource.GenerateImpulseWithForce(MONSTER_HITTED_BY_REAPER_ATTACK_FORCE);
                break;
        }
    }

}
