using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerSkillLaunchBomb : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    public void LauchBomb(Vector3 pos, define.ECharacterLookDir eLookDir)
    {
        GameObject go;
        go = Managers.SkillPool.Get(pos);
        go.GetComponent<KnockbackBoom>().Launch(eLookDir);
    }
}
