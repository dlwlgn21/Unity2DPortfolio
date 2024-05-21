using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestThrow : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Shoot();
        }
    }


    private void Shoot()
    {
        Debug.Log("Shoot!!!");
        Instantiate(_projectile, transform.position + Vector3.right * 3f, Quaternion.identity);
    }


}
