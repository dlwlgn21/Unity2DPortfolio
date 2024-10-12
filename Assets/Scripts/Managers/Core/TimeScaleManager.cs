using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeScaleManager : MonoBehaviour
{
    private bool _isTimeScaling = false;

    const float ATTACK_SUCCESS_SLOW_TIME = 0.3f;
    const float HITTED_BY_MONSTER_SLOW_TIME = 0.2f;
    const float ATTACK_SUCCESS_TIME_SCALE_VALUE = 0.5f;
    const float HITTED_BY_MONSTER_TIME_SCALE_VALUE = 0.5f;

    public void Init()
    {
        _isTimeScaling = false;
        gameObject.SetActive(true);
        // TODO : 수월한 테스트 위해서 일단 이거 꺼놓음
        //BaseMonsterController.BigAttackEventHandler -= OnSuccessBigAttack;
        //BaseMonsterController.BigAttackEventHandler += OnSuccessBigAttack;
    }


    public void OnSuccessBigAttack()
    {
        if (_isTimeScaling)
        {
            return;
        }
        Debug.Assert(this != null);
        StartCoroutine(StartTimeScaleOnPlayerNormalAttackSuccessCo());
    }

    public void OnPlayerHittedByMonster()
    {
        // TODO : 수월한 테스트 위해서 일단 이거 꺼놓음
        //if (_isTimeScaling)
        //{
        //    return;
        //}
        //Debug.Assert(this != null);
        //StartCoroutine(StartTimeScaleOnPlayerHittedByMonsterCo());
    }

    private IEnumerator StartTimeScaleOnPlayerNormalAttackSuccessCo()
    {
        _isTimeScaling = true;
        Time.timeScale = ATTACK_SUCCESS_TIME_SCALE_VALUE;
        yield return new WaitForSeconds(ATTACK_SUCCESS_SLOW_TIME);
        TimeScalingEnd();
    }

    private IEnumerator StartTimeScaleOnPlayerHittedByMonsterCo()
    {
        _isTimeScaling = true;
        Time.timeScale = HITTED_BY_MONSTER_TIME_SCALE_VALUE;
        yield return new WaitForSeconds(HITTED_BY_MONSTER_SLOW_TIME);
        TimeScalingEnd();
    }

    private void TimeScalingEnd()
    {
        _isTimeScaling = false;
        Time.timeScale = 1f;
    }
}
