using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("몬스터")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("몬스터 정보")]
    [SerializeField] private float monsterHP = 50f;
    [SerializeField] private float plusHP = 1.25f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damage = 1f;

    [Header("웨이브 설정")]
    [SerializeField] private int enemyCount = 30;
    [SerializeField] private float spawnInterval = 0.25f;
    [SerializeField] private int rewardMana = 50;

    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Gamemanager gameManager;

    public int currentWave = 0;

    private int enemyRemain = 0;
    private int spawnAllEnemy = 0;
    private bool readyToStart = true;
    private bool rewardPaid = false;
    private void Start()
    {
        if(spawnPoint == null) //걍 안전장치 -> 안정 장치를 많이 사용
        {
            return;
        }
    }
    private void Update()
    {
        if(readyToStart && Input.GetKeyDown(KeyCode.S)) //웨이브 시작 웨이브 도중에 시작할 수 없게 bool 사용
        {
            readyToStart = false;
            StartNextWave();
        }
    }
    public void StartNextWave()
    {
        currentWave++; //시작할 때마다 라운드 증가

        spawnAllEnemy = enemyCount; //스폰될 적
        enemyRemain = spawnAllEnemy; //카운트 없으면 스폰 다 되자마자 끝나버린 걸로 됨

        rewardPaid = false;

        float waveHP = WaveStat(monsterHP, plusHP, currentWave); //웨이브에 따라 hp증가
        StartCoroutine(SpawnEnemy_co(waveHP, damage, speed));
    }
    private float WaveStat(float baseStat, float multiplier, int wave)
    {
        return baseStat * Mathf.Pow(multiplier, wave - 1); //pow - 거듭제곱 (웨이브 수만큼 거듭제곱) 몬스터 체력 증가
    }
    private IEnumerator SpawnEnemy_co(float HP, float damage, float speed)
    {
        if (playerControl != null)
        {
            playerControl.SetCanSpawn(false);//몬스터 웨이브 중에 타워 설치 불가
        }

        for (int i = 0; i<spawnAllEnemy; i++)
        {
            SpawnEnemy(HP, damage, speed);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void SpawnEnemy(float HP, float damage, float speed)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyData enemyData = enemy.GetComponent<EnemyData>();
        if (enemyData != null)
        {
            enemyData.EnemyStat(HP, damage, speed);
        }

        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        if (enemyMove != null)
        {
            enemyMove.speed = speed;

            enemyMove.Initialize(this, playerHealth, damage);
        }
    }
    public void OnEnemyDestroyed() //적 사라져잉
    {
        enemyRemain--; 

        if (enemyRemain <= 0)
        {
            if(!rewardPaid)
            {
                GiveMana(currentWave);
                rewardPaid = true;
            }
            readyToStart = true;

            if (playerControl != null)
            {
                playerControl.SetCanSpawn(true);
            }
        }
    }

    private void GiveMana(int waveNumber)
    {
        int finalReward = rewardMana;

        // Gamemanager에 마나 추가 요청
        if (gameManager != null)
        {
            //  Gamemanager에 AddMana 함수가 있어야 합니다.
            gameManager.AddMana(finalReward);
            Debug.Log($" [보상] 웨이브 {waveNumber} 클리어! 마나 {finalReward} 획득.");
        }
    }
}
