using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    Light2D _light;
    public float TurnOffGraduallyLightTimeInSec { get; set; }
    float _initialIntencity;
    Coroutine _turnOffLightCoOrNull = null;

    private void Awake()
    {
        AssignComponent();
        _initialIntencity = _light.intensity;
        SetLightIntencityToZero();
    }

    protected virtual void SetLightIntencityToZero()
    {
        AssignComponent();
        _light.intensity = 0f;
    }

    public void TurnOnLight()
    {
        Debug.Assert(TurnOffGraduallyLightTimeInSec > Mathf.Epsilon);
        ForceToStopCoroutineAndTurnOffLight();
        AssignComponent();
        SetActiveTrue();
        if (!_light.enabled)
            _light.enabled = true;
        _light.intensity = _initialIntencity;
    }
    
    public void TurnOffLightGradually()
    {
        if (_turnOffLightCoOrNull != null)
        {
            StopCoroutineAndTurnOffLight();
            return;
        }
        SetActiveTrue();
        _turnOffLightCoOrNull = StartCoroutine(StartTurnOffLightCo());
    }

    public void ForceToStopCoroutineAndTurnOffLight()
    {
        if (_turnOffLightCoOrNull != null)
        {
            StopCoroutine(_turnOffLightCoOrNull);
        }
        _turnOffLightCoOrNull = null;
        _light.intensity = 0f;
    }


    IEnumerator StartTurnOffLightCo()
    {
        float elapsedTime = 0f;
        while (elapsedTime < TurnOffGraduallyLightTimeInSec)
        {
            elapsedTime += Time.deltaTime;
            _light.intensity = Mathf.Lerp(_initialIntencity, 0f, elapsedTime / TurnOffGraduallyLightTimeInSec);
            yield return null;
        }
        _turnOffLightCoOrNull = null;
    }
    void SetActiveTrue()
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
    }
    void AssignComponent()
    {
        if (_light == null)
        {
            _light = GetComponent<Light2D>();
        }
    }

    void StopCoroutineAndTurnOffLight()
    {
        StopCoroutine(_turnOffLightCoOrNull);
        _turnOffLightCoOrNull = null;
        _light.intensity = 0f;
    }

}
