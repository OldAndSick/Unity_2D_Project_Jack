
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] private float mAtk = 1;

    private float currentMaxHP;
    private float currentHP;
    private float currentAtk;

    private EnemyMove enemyMove;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
        
        currentAtk = mAtk;
    }

    public void EnemyStat(float waveHP, float waveAtk, float waveSpeed)
    {
        currentMaxHP = waveHP;
        currentHP = currentMaxHP;
        currentAtk = waveAtk;

        if(enemyMove != null)
        {
            enemyMove.speed = waveSpeed;
        }
    }

    public void TakeDamage(float towerDamage) //몬스터가 받는 데미지
    {
        if (currentHP > 0)
        {
            Debug.Log("때려요잉");
            currentHP -= towerDamage;
        }

        if(currentHP <=0)
        {
            
            Die();
        }
    }
    public void Die()
    {
        float deathLength = 0.35f;
        EnemyManager manager = FindAnyObjectByType<EnemyManager>();// (런타임 종속성 검색) 싱글톤 관리자 찾을 때 유용 씬 전체에서 탐색한다고 함
        if (animator != null)
        {
            Debug.Log("아이고 나 죽네");
            animator.SetTrigger("Death");
             
        }
 
        if (manager != null)
        {
            manager.OnEnemyDestroyed();
        }
        Destroy(gameObject, deathLength);
    }

    public float EnemyAttack()
    {
        return currentAtk;
    }
}
