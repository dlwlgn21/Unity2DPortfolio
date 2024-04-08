using UnityEngine;

public class TestPC : MonoBehaviour
{
    private TestMovement mMovement;

    private void Awake()
    {
        mMovement = GetComponent<TestMovement>();
    }

    private void Update()
    {
        UpdateMove();
    }

    public void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Debug.Log(x);
        mMovement.MoveTo(x);
    }
}
