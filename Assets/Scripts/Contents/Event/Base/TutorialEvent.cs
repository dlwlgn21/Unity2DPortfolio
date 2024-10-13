using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialSequence : MonoBehaviour
{
    protected bool _isTutorialStart = false;
    protected NormalMonsterController _spawnendMonsterController;
    protected PlayerController _pc;
    protected int _playerOriginalAttackDamage;
    protected TutorialCameraManager _camManager;
    protected TutorialManager _tutorialManager;
    public abstract void OnDialogEnd();
    protected void Init()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _pc = player.GetComponent<PlayerController>();
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
    }

    protected void SpawnMonster()
    {
        Vector2 playerPos = _pc.gameObject.transform.position;
        _spawnendMonsterController = Managers.MonsterPool.Get(define.EMonsterNames.Warden, new Vector2(playerPos.x + 5f, playerPos.y + 1f)).GetComponent<NormalMonsterController>();
        _spawnendMonsterController.Stat.InitStatForTutorial();
        _spawnendMonsterController.HealthBar.SetFullHpBarRatio();
        _spawnendMonsterController.ChangeState(ENormalMonsterState.Idle);
    }
    protected void SetPlayerAttackToZero()
    {
        _playerOriginalAttackDamage = _pc.Stat.Attack;
        _pc.Stat.Attack = 0;
    }
    protected void RollBackPlayerAttack()
    {
        _pc.Stat.Attack = _playerOriginalAttackDamage;
    }

    protected void SwitchCamToMain()
    {
        Managers.CamSwitch.SwitchCameraToMain();
    }
}
