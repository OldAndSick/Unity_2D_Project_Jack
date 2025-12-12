using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMent : MonoBehaviour
{
    public float MoveSpeed = 0f;

    [SerializeField] private Vector3 MoveDirection = Vector3.zero;

    public void MoveTo(Vector3 Direction)
    {
        MoveDirection = Direction;
    }

    private void Update()
    {
        transform.position += MoveDirection * MoveSpeed * Time.deltaTime;
    }
}
