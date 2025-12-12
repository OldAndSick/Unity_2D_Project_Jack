using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] Transform playerTransform;
    [SerializeField] float installRange = 2f;
    [SerializeField] MoveToMouse moveTo;
    [SerializeField] EnemyWay enemyWay;

    public bool CanSpawn = true;

    private void Start()
    {
        moveTo = GetComponent<MoveToMouse>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (CanSpawn)
            {
                TileInstall();
            }
            else
            {
                Debug.Log(" [설치 불가] 현재 몬스터 웨이브 중입니다!");
            }
        }
    }
    private void TileInstall() //타일 설치
    {
        Vector3 clickToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
        Vector3Int clickPos = mapData.CellPosition(clickToWorld);


        if (!mapData.CanInstall(clickPos))
        {
            Debug.Log("설치 불가");
            return;
        }

        if (PlayerRange(clickPos))
        {
            mapData.TileInstall(clickPos);
            if(enemyWay != null)
            {
                enemyWay.PathFinding();
            }
        }
        else
        {
            Debug.Log("설치 범위 밖");
            StartCoroutine(MoveAndInstall(clickPos));
        }
    }

    private IEnumerator MoveAndInstall(Vector3Int target) //범위 밖일 때 이동하고 설치하는 코루틴
    {
        yield return  StartCoroutine(moveTo.Move_co(target));
        yield return null;//한프레임
        mapData.TileInstall((Vector3Int)target);
        if(enemyWay != null)
        {
            enemyWay.PathFinding();
        }

    }


    private bool PlayerRange(Vector3Int cellPos) //범위 밖이면 설치되지 않는다
    {
        if(playerTransform == null)
        {
            return false;
        }
    
        Vector3 worldCenter = mapData.mapTile.GetCellCenterWorld(cellPos); //타일 위치 월드좌표 중앙으로 

        float distance = Vector3.Distance(playerTransform.position, worldCenter);
        return distance <= installRange;
        
    }
    public void SetCanSpawn(bool state)
    {
        CanSpawn = state;
        Debug.Log($"[시스템] 타워 설치 가능 상태가 {(state ? "활성화" : "비활성화")}되었습니다.");
    }
}
