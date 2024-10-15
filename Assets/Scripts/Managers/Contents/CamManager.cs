using Cinemachine;
using UnityEngine;

public enum ECamShakeType
{
    PLAYER_HITTED_BY_MONSTER,
    PLAYER_BLOCK_SUCCES,
    MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK,
    MONSTER_HITTED_BY_KNOCKBACK_BOMB,
    MONSTER_HITTED_BY_REAPER_ATTACK,
}
public class CamManager
{
    private GameObject _camShakeManager;
    private CinemachineImpulseSource _playerImpulseSource;
    private CinemachineImpulseSource _monsterImpulseSource;

    private const float PLAYER_HITTED_BY_MONSTER_FORCE = 0.7f;
    private const float PLAYER_BLOCK_SUCCES_FORCE = 0.8f;
    private const float MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_FORCE = 1f;
    private const float MONSTER_HITTED_BY_KNOCKBACK_BOMB_FORCE = 0.9f;
    private const float MONSTER_HITTED_BY_REAPER_ATTACK_FORCE = 1.5f;

    public CamFollowObject CamFollow { get; private set; }
    public void Init()
    {
        if (GameObject.Find("@CamManager") == null)
        {
            _camShakeManager = Managers.Resources.Load<GameObject>("Prefabs/Managers/CamManager");
            _camShakeManager.name = "@CamManager";
            _playerImpulseSource = Utill.GetComponentInChildrenOrNull<CinemachineImpulseSource>(_camShakeManager, "PlayerImpulseSource");
            _monsterImpulseSource = Utill.GetComponentInChildrenOrNull<CinemachineImpulseSource>(_camShakeManager, "MonsterImpulseSource");
            Object.DontDestroyOnLoad(_camShakeManager);
            PlayerController.HitEffectEventHandler -= Managers.Cam.OnPlayerHittedByMonsterNormalAttack;
            BaseMonsterController.HittedByNormalAttackNoArgsEventHandler -= Managers.Cam.OnMonsterHittedByPlayerNormalAttack;
            NormalMonsterController.MonsterChangeStateEventHandler -= Managers.Cam.OnMonsterHittedByPlayerSkill;
            PlayerController.HitEffectEventHandler += Managers.Cam.OnPlayerHittedByMonsterNormalAttack;
            BaseMonsterController.HittedByNormalAttackNoArgsEventHandler += Managers.Cam.OnMonsterHittedByPlayerNormalAttack;
            NormalMonsterController.MonsterChangeStateEventHandler += Managers.Cam.OnMonsterHittedByPlayerSkill;
            CamFollow = Managers.Resources.Instantiate<CamFollowObject>("Prefabs/Cam/CamFollowObject");
            CamFollow.gameObject.name = "CamFollowObject";
            Object.DontDestroyOnLoad(CamFollow.gameObject);
        }
    }
    public void Clear()
    {
        PlayerController.HitEffectEventHandler -= Managers.Cam.OnPlayerHittedByMonsterNormalAttack;
        BaseMonsterController.HittedByNormalAttackNoArgsEventHandler -= Managers.Cam.OnMonsterHittedByPlayerNormalAttack;
        NormalMonsterController.MonsterChangeStateEventHandler -= Managers.Cam.OnMonsterHittedByPlayerSkill;
    }

    public void OnMonsterHittedByPlayerNormalAttack()
    {
        CamShake(ECamShakeType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK);
    }

    public void OnPlayerHittedByMonsterNormalAttack(EPlayerState eState)
    {
        switch (eState)
        {
            case EPlayerState.HitByMelleAttack:
                CamShake(ECamShakeType.PLAYER_HITTED_BY_MONSTER);
                break;
            case EPlayerState.BlockSucces:
                CamShake(ECamShakeType.PLAYER_BLOCK_SUCCES);
                break;
        }
        CamShake(ECamShakeType.PLAYER_HITTED_BY_MONSTER);
    }

    public void OnMonsterHittedByPlayerSkill(ENormalMonsterState eState)
    {
        switch (eState)
        {
            case ENormalMonsterState.HitByPlayerBlockSucces:
            case ENormalMonsterState.HitByPlayerSkillKnockbackBoom:
                Managers.Cam.CamShake(ECamShakeType.MONSTER_HITTED_BY_KNOCKBACK_BOMB);
                break;
            case ENormalMonsterState.HitByPlayerSkillParallysis:
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
