using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TestThrow : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    public void LauchBomb(define.ECharacterLookDir eLookDir, Vector3 pos)
    {
        GameObject go;
        go = Managers.SkillPool.Get(pos);
        go.GetComponent<TestProjectile>().Launch(eLookDir);
    }
}
