using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LevelUpText : MonoBehaviour
{
    TextMeshProUGUI _text;

    private const float WAIT_TIME_IN_SEC = 3f; 
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.enabled = false;
        PlayerStat.LevelUpEventHandler -= OnLevelUp;
        PlayerStat.LevelUpEventHandler += OnLevelUp;
    }

    void OnLevelUp(int levelUpCount)
    {
        _text.enabled = true;
        _text.transform.localScale = new Vector3(1f, 1f, 1f);
        Managers.Tween.StartUIDoPunchPos(_text.transform);
        StartCoroutine(SetEnabledToFalseAfterSeconds());
    }

    IEnumerator SetEnabledToFalseAfterSeconds()
    {
        yield return new WaitForSeconds(WAIT_TIME_IN_SEC);
        Managers.Tween.StartUIScaleTW(_text.transform, 0f, () => { _text.enabled = false; }, 0.5f);
    }
}
