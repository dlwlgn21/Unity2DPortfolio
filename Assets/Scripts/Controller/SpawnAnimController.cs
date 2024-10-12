using UnityEngine;

public class SpawnAnimController : WorldSpaceAnimController
{
    private void Awake()
    {
        AssignComponents();
        SetComponentsEnabled(false);
    }
    private void Update()
    {
        FixPosition();
    }

    private void OnEnable()
    {
        MonsterPoolManager.MonsterSpawnEventHandler -= OnSpawnMonster;
        MonsterPoolManager.MonsterSpawnEventHandler += OnSpawnMonster;
    }

    private void OnDisable()
    {
        MonsterPoolManager.MonsterSpawnEventHandler -= OnSpawnMonster;
    }

    public void OnSpawnMonster(BaseMonsterController mc, Vector2 worldPos)
    {
        if (mc.gameObject.activeSelf)
        {
            PlaySpawnEffect(worldPos);
        }
    }

    private void PlaySpawnEffect(Vector2 worldPos)
    {
        if (_animator == null)
        {
            AssignComponents();
        }
        SetComponentsEnabled(true);
        _animator.Play("SpawnEffect", -1, 0f);
        _fixedWorldPos = worldPos;
    }

    public void OnSpawnEffectAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }
}
