using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        // 1. W 키를 누른 상태에서 + 마우스 왼쪽 클릭
        if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(0))
        {
            SellTileAtMousePosition();
        }
    }

    private void SellTileAtMousePosition()
    {
        // 2. 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // 3. 월드 좌표를 타일맵의 '셀 좌표(Grid 좌표)'로 변환
        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

        // 4. 그 위치에 타일이 실제로 있는지 확인
        if (tilemap.HasTile(cellPos))
        {
            // 5. 타일의 중심 좌표 가져오기 (타워 매니저 확인용)
            Vector3 cellCenterPos = tilemap.GetCellCenterWorld(cellPos);

            // 6. 그 위에 타워가 있는지 확인 (TowerManager에게 물어봄)
            if (Gamemanager.instance.TowerManager.IsPositionOccupied(cellCenterPos))
            {
                Debug.LogWarning(" [판매 불가] 타일 위에 타워가 있습니다!");
                return;
            }

            // 7. 타일 판매 (삭제) 및 보상
            Debug.Log($" [타일 판매] 좌표 {cellPos} 타일 제거됨.");

            // 보상 지급 (예: 1 마나)
            Gamemanager.instance.AddMana(1);

            //  핵심: 타일맵에서 해당 위치의 타일을 null로 바꿔버림 (삭제)
            tilemap.SetTile(cellPos, null);
        }
    }
}
