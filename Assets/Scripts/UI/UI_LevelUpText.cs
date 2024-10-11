using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LevelUpText : MonoBehaviour
{
    TextMeshProUGUI _text;

    private const float WAIT_TIME_IN_SEC = 2f; 
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.enabled = false;
        PlayerStat.OnLevelUpEventHandler -= OnLevelUp;
        PlayerStat.OnLevelUpEventHandler += OnLevelUp;
    }

    void OnLevelUp(int levelUpCount)
    {
        _text.enabled = true;
        Managers.Tween.StartUIDoPunchPos(_text.transform);
        StartCoroutine(SetEnabledToFalseAfterSeconds());
    }

    IEnumerator SetEnabledToFalseAfterSeconds()
    {
        yield return new WaitForSeconds(WAIT_TIME_IN_SEC);
        _text.enabled = false;
    }
}