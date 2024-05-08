using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckTutorialEvent : EventBoxCollider
{
    private MonsterStat _zKeyMonsterStatOrNull = null;
    protected override void OnPlayerEnter()
    {
        if (_isFirstPlayerEnterFlag)
            return;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector2 playerPosition = playerTransform.position;
        GameObject warden = Managers.MonsterPool.Get(define.EMonsterNames.Warden, new Vector2(playerPosition.x + 7f, playerPosition.y + 1f));
        _zKeyMonsterStatOrNull =  warden.GetComponent<MonsterStat>();
        Debug.Assert(_zKeyMonsterStatOrNull != null);
        _zKeyMonsterStatOrNull.SetHPForTutorial(50);
        Managers.UIKeyTutorial.ActiveAttackKeyTutorial();
        _isFirstPlayerEnterFlag = true;
    }

    private void Update()
    {
        if (_zKeyMonsterStatOrNull != null && _zKeyMonsterStatOrNull.HP <= 0)
        {
            Managers.UIKeyTutorial.UnactiveAttackKeyTutorial();
            gameObject.SetActive(false);
        }
    }
}
