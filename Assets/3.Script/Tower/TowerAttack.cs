using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    private TowerSO data;
    private Transform target;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("투사체")]
    [SerializeField] private TowerBulletSO towerBulletData;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void Initialize(TowerSO data)
    {
        this.data = data;
        StopAllCoroutines();
        Debug.Log("코루틴 갑니다잉"); 
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if(target == null || Vector3.Distance(transform.position,target.position) > data.attackRange)
            {
                target = FindTarget();
                
            }
            if(target != null)
            {
                Debug.Log("적 발견 공격 준비");
                LookAtTarget(target.position);
                if(animator != null)
                {
                    animator.SetTrigger("Attack");
                }
                SpawnTowerBullet();
                yield return new WaitForSeconds(1f / data.attackSpeed);
            }
        }

    }
    private void SpawnTowerBullet()
    {
        GameObject bulletInstance = Instantiate(towerBulletData.bulletSkin, transform.position, Quaternion.identity);
        TowerBullet towerBulletComponent = bulletInstance.GetComponent<TowerBullet>();

        if (towerBulletComponent != null)
        {
            towerBulletComponent.BulletShoot(towerBulletData, target, data.towerDamage);
        }
    }
    private Transform FindTarget()
    {
        Collider2D[] targetOn = Physics2D.OverlapCircleAll(transform.position, data.attackRange, LayerMask.GetMask("Enemy"));

        if(targetOn.Length > 0)
        {
            return targetOn[0].transform;
        }
        return null;
    }
    private void LookAtTarget(Vector3 targetPosition)
    {
        if (spriteRenderer == null)
            return;

        float directionX = targetPosition.x - transform.position.x;

        if(directionX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(directionX < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
