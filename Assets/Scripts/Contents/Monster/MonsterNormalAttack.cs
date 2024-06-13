using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNormalAttack : MonoBehaviour
{
    private MonsterStat _stat;
    private BaseMonsterController _controller;
    private void Awake()
    {
        _controller = transform.parent.GetComponent<BaseMonsterController>();
        Debug.Assert(_controller != null);
        _stat = _controller.Stat;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().OnHitted(_stat.Attack, _controller);
        }
    }
}
