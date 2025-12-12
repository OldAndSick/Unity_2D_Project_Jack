using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    public float Speed = 2f;
    private Vector3 target;

    private SpriteRenderer Renderer;
    void Start()
    {
        target = transform.position;

        Renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

        }
        MovePlayer(target,Speed);


    }

    public void MovePlayer(Vector3 target,float speed)
    {
        MoveFace();
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    //public void Coroutine_move(Vector3 targetPostion)
    //{
    //    StartCoroutine(Move_co(targetPostion));
    //}
    public IEnumerator Move_co(Vector3 target_position)
    {
        target_position = new Vector3(target_position.x, target_position.y, 0f);
        target = target_position;
        while (Vector3.Distance(transform.position,target_position)>0.05f)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target_position, (Speed * Time.deltaTime)*0.5f);
            MovePlayer(target_position,Speed*0.5f);
            yield return null;//한프레임
        }
        transform.position = target_position;

    }

    private void MoveFace()
    {
        if (Renderer == null)
            return;

        float directionX = target.x - transform.position.x;

        if(directionX > 0.01f)
        {
            Renderer.flipX = false;
        }
        else if(directionX < -0.01f)
        {
            Renderer.flipX = true;
        }
    }
}
