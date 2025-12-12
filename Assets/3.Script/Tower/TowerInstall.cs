using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInstall : MonoBehaviour
{
    public TowerType Type { get; private set; }
    public int Tier { get; private set; }
    public TowerSO Data { get; private set; }
    private TowerAttack towerAttack;

  
    public void Initialize(TowerSO data)
    {
        Data = data;
        Type = data.type;
        Tier = data.tier;

        gameObject.name = $"{data.towerName} (Tier{data.tier})";
        Debug.Log("나 타워 설치");
        if (towerAttack != null)
        {
            towerAttack.Initialize(data);
        }
    }
    public void Remove()
    {
        if (Gamemanager.instance != null && Gamemanager.instance.TowerManager != null)
        {
            Gamemanager.instance.TowerManager.RemoveInstalledTower(this);
        }
        Destroy(gameObject);
    }
    private void Awake()
    {
        towerAttack = GetComponent<TowerAttack>();
    }
    private void OnMouseDown()
    {
        Debug.Log($"[판매 디버그] 타워 '{gameObject.name}' 위에서 마우스 클릭 감지.");
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log(" [판매 디버그] Q 키 입력 확인! TowerManager에 판매 요청.");
            if (Gamemanager.instance != null && Gamemanager.instance.TowerManager != null)
            {
                Gamemanager.instance.TowerManager.SellTower(this);
            }
            else
            {
                Debug.LogError(" [판매 디버그] GameManager 또는 TowerManager가 null입니다. 초기화 확인 필요.");
            }
        }
        else
        {
            // 4. Q 키 안 눌림 확인
            Debug.LogWarning(" [판매 디버그] Q 키가 눌리지 않았습니다. 판매 실패.");
        }
    }
}
