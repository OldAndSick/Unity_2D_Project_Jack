using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("타워 데이터")]
    public List<TowerSO> allTower;

    [Header("시스템 연결")]  
    [SerializeField] private LayerMask installLayer;
    [SerializeField] private Gamemanager gameManager;
    [SerializeField] private float tileCheck = 0.1f;

    [Header("확률")]
    [Range(0f, 1f)] public float tier1Probability = 0.65f; //슬라이드바 형식

    [Range(0f, 1f)] public float tier2Probability = 0.30f;
    [Range(0f, 1f)] public float tier3Probability = 0.40f;
    [Range(0f, 1f)] public float tier4Probability = 0.10f;
    //[Range(0f, 1f)] public float tier3Probability = 
    [Header("타일 관리")]
    private Dictionary<Vector3, bool> tileOccupancy = new Dictionary<Vector3, bool>();


    public bool CanSpawn = true;
    private Camera mainCamera;
    private List<TowerInstall> installTower = new List<TowerInstall>();

    private void Awake()
    {
        mainCamera = Camera.main;
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (CanSpawn)
            {
                TowerInstall();
            }
            else
            {
                Debug.Log(" [설치 불가] 현재 몬스터 웨이브 중입니다!");
            }
        }
        
    }
    private void TowerInstall()
    {
        TowerSO putTower = InstallRandomTower();
        

        if(putTower == null)
        {
            return;
        }
        Vector3 installPosition = MouseToWorldPostion();
        bool towerToInstall = InstallTower(putTower, installPosition);
        if(!towerToInstall)
        {
            Debug.Log("설치 실패(오류)");
        }

    }
    private Vector3 MouseToWorldPostion()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.x = Mathf.Round(worldPosition.x); //round 소수점 반올림
        worldPosition.y = Mathf.Round(worldPosition.y);
        worldPosition.z = 0;
        return worldPosition;
    }
    public TowerSO InstallRandomTower()
    {
        int selectTier = 1;
        float rnd = UnityEngine.Random.value; //0.0이상 1.0 이하 랜덤

        if(rnd < tier1Probability) //누적 확률이랍니다
        {
            selectTier = 1;
        }
        else if(rnd < tier1Probability + tier2Probability)
        {
            selectTier = 2;
        }
        else if(rnd < tier1Probability + tier2Probability + tier3Probability)
        {
            selectTier = 3;
        }
        else
        {
            selectTier = 4;
        }
        List<TowerSO> tierTower = allTower.Where(so => so.tier == selectTier).ToList(); //linq where 조건 맞는 애들만
        int manaCost = 10;
        if(gameManager != null && gameManager.CurrentMana < manaCost)
        {
            Debug.Log("노mp");
            return null;
        }
        if(gameManager != null)
        {
            gameManager.UseMana(manaCost);
        }

        int rndTower = UnityEngine.Random.Range(0, tierTower.Count);

        TowerSO selectedTower = tierTower[rndTower]; // 뽑힌 타워

        // 여기서 4티어인지 확인하고 컷신 발사!
        if (selectedTower.tier == 4)
        {
            if (CutsceneManager.instance != null)
            {
                CutsceneManager.instance.PlayCutscene(selectedTower);
            }
        }
        return selectedTower;
    }

    public bool InstallTower(TowerSO towerToInstall, Vector3 installPosition)
    {
        if (!CanInstall(installPosition))
        {
            return false;
        }
        if (towerToInstall.prefabs == null)
        {
            return false;
        }
        GameObject newTowerObject = Instantiate(towerToInstall.prefabs, installPosition, Quaternion.identity);
        TowerInstall towerInstall = newTowerObject.GetComponent<TowerInstall>();

        if (towerInstall != null)
        {
            towerInstall.Initialize(towerToInstall);
            installTower.Add(towerInstall);

            SetTileOccupancy(installPosition, true);

            return true;
        }
        
        Destroy(newTowerObject);
        return false;
    }

    public bool CanInstall(Vector3 position)
    {

        Vector3 gridKey = ConvertWorldToGridKey(position);
        //if (tileOccupancy.ContainsKey(gridKey) && tileOccupancy[gridKey] == true)

            if (tileOccupancy.ContainsKey(gridKey))
        {
            Debug.Log($"설치 불가: 타일 ({gridKey})이 이미 점유됨.");
            return false;

        }
           
            

        Collider2D hit = Physics2D.OverlapCircle(position, tileCheck, installLayer);

        if(hit == null)
        {
            return false;
        }

        installTower.RemoveAll(tower => tower == null);

        foreach (TowerInstall tower in installTower)
        {
            if(Vector3.Distance(tower.transform.position, position) < 0.5f)
            {
                return false;
            }
        }
        return true;
    }
    public void RemoveInstalledTower(TowerInstall tower)
    {
        if (installTower.Contains(tower))
        {
            installTower.Remove(tower);
            Debug.Log($"타워 목록에서 제거됨: {tower.gameObject.name}");
        }
    }
    public void SellTower(TowerInstall towerToSell)
    {
        if(towerToSell == null || towerToSell.Data == null)
        {
            Debug.Log("판매가 안되는뎁쇼");
            return;
        }
        //판매
        const int manaCost = 10;
        int sellPrice = manaCost / 2;

        if(gameManager != null)
        {
            gameManager.AddMana(sellPrice);
            Debug.Log("판매 완");
        }

        UnoccupyTile(towerToSell.transform.position);

        //  목록에서 제거 및 오브젝트 파괴
        RemoveInstalledTower(towerToSell); // 목록에서 타워 제거
        Destroy(towerToSell.gameObject);  // 실제 오브젝트 파괴
    }

    //이 아래부터 악마와 계약
    public void UnoccupyTile(Vector3 worldPosition)
    {
        SetTileOccupancy(worldPosition, false);
    }
    private void SetTileOccupancy(Vector3 worldPosition, bool occupied)
    {
        Vector3 gridKey = ConvertWorldToGridKey(worldPosition);
        if (occupied)
        {
            // 설치: 이미 키가 없을 때만 추가 (안전장치)
            if (!tileOccupancy.ContainsKey(gridKey))
            {
                tileOccupancy.Add(gridKey, true);
                Debug.Log($"[타일 점유] {gridKey} 위치 등록됨");
            }
        }
        else
        {
            // 판매: 키가 있으면 삭제 (아예 없애버림)
            if (tileOccupancy.ContainsKey(gridKey))
            {
                tileOccupancy.Remove(gridKey);
                Debug.Log($"[타일 해제] {gridKey} 위치 비워짐 (삭제완료)");
            }
        }
    }
    private Vector3 ConvertWorldToGridKey(Vector3 worldPosition)
    {
        Vector3 gridKey = worldPosition;
        gridKey.x = Mathf.Round(worldPosition.x);
        gridKey.y = Mathf.Round(worldPosition.y);
        gridKey.z = 0;
        return gridKey;
    }
    public void SellTile(GameObject tileToSell)
    {
        Vector3 tilePos = tileToSell.transform.position;
        Vector3 gridKey = ConvertWorldToGridKey(tilePos);

        // 1. 타워 존재 여부 확인 (핵심!)
        // tileOccupancy에 키가 있다는 건, 그 위에 타워가 있다는 뜻입니다.
        if (tileOccupancy.ContainsKey(gridKey))
        {
            Debug.LogWarning($" [판매 불가] 타일 위에 타워가 있어서 판매할 수 없습니다! 위치: {gridKey}");
            return; // 판매 중단
        }

        // 2. 판매 보상 (타일 판매 가격은 0원이거나 소량으로 설정)
        // 형님이 말씀하신 "10개 무료, 이후 유료"는 설치 비용 로직이므로
        // 판매 시에는 보통 적은 양의 마나를 돌려주거나 안 돌려줍니다.
        // 여기서는 예시로 1마나를 돌려줍니다.
        if (gameManager != null)
        {
            gameManager.AddMana(1);
        }

        // 3. 타일 파괴
        Destroy(tileToSell);
        Debug.Log($" [타일 판매] {gridKey} 위치의 타일이 제거되었습니다.");
    }
    public bool IsPositionOccupied(Vector3 worldPosition)
    {
        Vector3 gridKey = ConvertWorldToGridKey(worldPosition);
        // 딕셔너리에 키가 있다는 건 타워가 있다는 뜻!
        return tileOccupancy.ContainsKey(gridKey);
    }
    
}


