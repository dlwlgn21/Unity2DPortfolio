using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    Light2D _light;
    [SerializeField] private float _turnOffGraduallyLightTime;
    private float _initialIntencity;
    private bool _isDoingTurnOffLight = false;
    private void Awake()
    {
        AssignComponents();
        _initialIntencity = _light.intensity;
        Init();
    }
    public virtual void Init()
    {
        AssignComponents();
        _light.intensity = 0f;
    }

    public void SetTurnOffLightTime(float time)
    {
        _turnOffGraduallyLightTime = time;
    }

    public void TurnOnLight()
    {
        _light.enabled = true;
        _light.intensity = _initialIntencity;
    }
    public void TurnOffLightGradually()
    {
        if (_isDoingTurnOffLight)
        {
            return;
        }
        StartCoroutine(StartTurnOffLightCo());
    }

    IEnumerator StartTurnOffLightCo()
    {
        // TODO : 이거 나중에 손보자. 바꿔야함. 끄는중에 켜면, 끄는 값을 덮어 씌어버리는 문제가 생김. 이거 고쳐야함.
        float elapsedTime = 0f;
        _isDoingTurnOffLight = true;
        while (elapsedTime < _turnOffGraduallyLightTime)
        {
            elapsedTime += Time.deltaTime;
            _light.intensity = Mathf.Lerp(_initialIntencity, 0f, elapsedTime / _turnOffGraduallyLightTime);
            yield return null;
        }
        _isDoingTurnOffLight = false;
    }

    private void AssignComponents()
    {
        if (_light == null)
        {
            _light = GetComponent<Light2D>();
        }
    }
}
