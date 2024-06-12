using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private bool _isTimeScaling;

    const float ATTACK_SUCCESS_SLOW_TIME = 0.3f;
    const float HITTED_BY_MONSTER_SLOW_TIME = 0.2f;
    const float ATTACK_SUCCESS_TIME_SCALE_VALUE = 0.5f;
    const float HITTED_BY_MONSTER_TIME_SCALE_VALUE = 0.5f;
    public void Init()
    {
        _isTimeScaling = false;
    }

    public void OnPlayerNormalAttackSuccess()
    {
        if (_isTimeScaling)
        {
            return;
        }
        StartCoroutine(StartTimeScaleOnPlayerNormalAttackSuccess());
    }

    public void OnPlayerHittedByMonster()
    {
        if (_isTimeScaling)
        {
            return;
        }
        StartCoroutine(StartTimeScaleOnPlayerHittedByMonster());
    }

    private IEnumerator StartTimeScaleOnPlayerNormalAttackSuccess()
    {
        _isTimeScaling = true;
        Time.timeScale = ATTACK_SUCCESS_TIME_SCALE_VALUE;
        yield return new WaitForSeconds(ATTACK_SUCCESS_SLOW_TIME);
        TimeScalingEnd();
    }

    private IEnumerator StartTimeScaleOnPlayerHittedByMonster()
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
