using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class TestMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    private Rigidbody2D mRb;

    private Vector2 mMoveSpeed;

    private void Awake()
    {
        mRb = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(float x)
    {
        Vector2 velocity = mRb.velocity;
        mMoveSpeed.x = x * _moveSpeed;
        velocity.x = mMoveSpeed.x;
        mRb.velocity = velocity;
    }
}
