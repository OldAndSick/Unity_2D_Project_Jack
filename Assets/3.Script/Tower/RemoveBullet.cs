using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    [SerializeField] private float bulletRemove = 0.2f;

    private void Start()
    {
        Destroy(gameObject, bulletRemove);
    }
}
