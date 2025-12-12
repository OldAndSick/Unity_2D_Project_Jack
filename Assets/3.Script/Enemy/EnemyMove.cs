using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private EnemyWay pathfinder;
    private List<Node> pathNodes;

    [SerializeField] public float speed = 1;

    private EnemyManager enemyManager;
    private PlayerHealth playerHealth;
    private float monsterDamage;

    private int wayPoint = 0;
    public void Initialize(EnemyManager manager, PlayerHealth health, float damage)
    {
        enemyManager = manager;
        playerHealth = health;
        monsterDamage = damage;
    }
    private void Start()
    {
        pathfinder = FindAnyObjectByType<EnemyWay>();
        if(pathfinder != null)
        {
            pathNodes = pathfinder.Complete_Node;

            if(pathNodes == null || pathNodes.Count <2)
            {
                Destroy(gameObject);
                return;
            }
        }
        SetPosition();
    }
    void SetPosition ()
    {
        Node startNode = pathNodes[0];
        Vector3 startPos = new Vector3(startNode.X, startNode.Y, transform.position.z);
        transform.position = startPos;

        wayPoint = 1;
    }
    private void Update()
    {
        if (pathNodes == null || wayPoint >= pathNodes.Count)
        {
            ReachAction();
            return;
        }
        Node targetNode = pathNodes[wayPoint];

        Vector3 targetPos = new Vector3(targetNode.X, targetNode.Y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            wayPoint++;
        }
    }
    void ReachAction()
    {
        

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(monsterDamage);
            Debug.Log($"[시스템] 몬스터 도착! 플레이어 체력 감소: {monsterDamage}");
        }

        // 2. EnemyManager에게 몬스터가 사라졌음을 알려 남은 수를 줄입니다.
        if (enemyManager != null)
        {
            enemyManager.OnEnemyDestroyed();
        }

        // .자기 자신을 파괴합니다.
        Destroy(gameObject);
    }
}