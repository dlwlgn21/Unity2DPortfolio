using UnityEngine;

public class SpawnEffectController : WorldSpaceEffectController
{
    private void Awake()
    {
        AssignComponents();
    }

    public void PlaySpawnEffect(Vector2 worldPos)
    {
        if (_animator == null)
        {
            AssignComponents();
        }
        _animator.Play("SpawnEffect", -1, 0f);
        _fixedWorldPos = worldPos;
    }

    public void OnSpawnEffectAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }
}
