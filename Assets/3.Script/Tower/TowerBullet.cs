using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    private TowerBulletSO data;
    private Transform target;
    private float damageApply;

    public void BulletShoot(TowerBulletSO bulletData, Transform targetEnemy, float towerDamage)
    {
        data = bulletData;
        target = targetEnemy;
        damageApply = towerDamage;

        Destroy(gameObject, data.lifeTime);

    }
    private void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;

        }
        Vector3 targetPostion = target.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPostion, data.speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, targetPostion) <0.1f)
        {
            HitTarget();
        }
    }
    private void HitTarget()
    {
        EnemyData enemyData = target.GetComponent<EnemyData>();
        if(enemyData != null)
        {
            enemyData.TakeDamage(damageApply);
        }
        if(data.hitSkin != null)
        {
            Instantiate(data.hitSkin, transform.position, Quaternion.identity);

        }
        Destroy(gameObject);
    }

}
