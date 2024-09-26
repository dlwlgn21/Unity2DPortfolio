using define;
using UnityEngine;

public abstract class BaseProjectileController : MonoBehaviour
{
    protected const float LIFE_TIME_IN_SEC = 3f;
    protected SpriteRenderer _sr;
    protected Animator _animator;
    protected Rigidbody2D _rb;
    protected ECharacterLookDir _eLaunchDir;
    protected bool _isHit = false;

    protected void AssginCommonComponents()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
}
