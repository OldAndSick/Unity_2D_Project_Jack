using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //타일 설치를 위해 연동

public class MapData : MonoBehaviour
{
    [Header("바닥과 벽")] //타일과 장애물 설치 구분
    [SerializeField] public Tilemap mapTile;
    [SerializeField] public Tilemap Wall;

    [Header("설치 타일과 전용 타일")] //설치할 타일과 장애물 설치 구분
    [SerializeField] private TileBase unitInstall;
    [SerializeField] public Tilemap installTile;

    private BoundsInt mapBound;

    private void Start()
    {
        if (Wall != null) //맵 범위 지정
        {
            mapBound = Wall.cellBounds;
        }
    }

    public Vector3Int CellPosition(Vector3 worldPosition)
    {
        if (mapTile == null)
            return Vector3Int.zero;
        return mapTile.WorldToCell(worldPosition); //return 연달아 쓰기 가능, worldtocell 쓰면 위치 주소 변환 가능
        
    }

    public bool CanInstall(Vector3Int cellPos) //장애물과 타일이 이미 설치되어 있는지 확인
    {
        if (!mapBound.Contains(cellPos))
        {
            Debug.Log("범위 밖");
            return false;
        }

        if (Wall.GetTile(cellPos) != null)
        {
            Debug.Log("장애물 위");
            return false;
        }
       // if (mapTile.GetTile(cellPos) != null)
       // {
       //     Debug.Log("그라운드 위에만 설치 가능");
       //     return false;
       //             오류 떠서 제거
        if(installTile.GetTile(cellPos) != null)
        {
            Debug.Log("이미 설치된 타일 위");
            return false;
        }
        return true;
    }

    public void TileInstall(Vector3Int cellPos) 
    {
        if (installTile != null && unitInstall != null) //맵 범위 안에 있고 타일이 설치되어 있지 않은 경우에 타일 설치
        {
            installTile.SetTile(cellPos, unitInstall);
        }
    }
}

