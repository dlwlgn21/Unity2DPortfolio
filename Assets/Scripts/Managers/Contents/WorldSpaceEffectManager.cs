using define;
using UnityEngine;

public class WorldSpaceEffectManager
{
    PlayerMovementEffectController _playerMovementEffect;
    public void Init()
    {
        if (_playerMovementEffect == null)
        {
            GameObject prefab = Managers.Resources.Load<GameObject>("Prefabs/Player/MovementEffect");
            GameObject go = Object.Instantiate(prefab);
            _playerMovementEffect = go.GetComponent<PlayerMovementEffectController>();
            Object.DontDestroyOnLoad(go);
        }
    }
    public void PlayPlayerMovementEffect(EPlayerMovementEffect eType, Vector2 pos, ECharacterLookDir eLookDir)
    {
        _playerMovementEffect.PlayEffect(eType, pos, eLookDir);
    }
}
