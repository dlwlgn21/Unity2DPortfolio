using UnityEngine;

public class RollTutorialEvent : EventBoxCollider
{
    private int _rollKeyDownCount = 0;
    private MonsterStat _monStat;
    protected override void OnPlayerEnter()
    {
        if (_isFirstPlayerEnterFlag)
            return;
        Managers.UIKeyTutorial.ActiveRollKeyTutorial();
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        GameObject monster = Managers.MonsterPool.Get(define.EMonsterNames.Warden, new Vector2(playerPos.x + 5f, playerPos.y + 1f));
        _monStat = monster.GetComponent<MonsterStat>();
        _monStat.SetHPForTutorial(50);
        _isFirstPlayerEnterFlag = true;
    }

    private void Update()
    {
        if (_isFirstPlayerEnterFlag)
        {
            if (Input.GetKeyDown(KeyCode.C))
                ++_rollKeyDownCount;
            if (_monStat.HP <= 0 && _rollKeyDownCount >= 3)
            {
                Managers.UIKeyTutorial.UnactiveRollKeyTutorial();
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
