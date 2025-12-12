using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallTile : MonoBehaviour
{
    private void OnMouseDown()
    {
        // W 키를 누른 상태에서 클릭하면 타일 판매
        if (Input.GetKey(KeyCode.W))
        {
            if (Gamemanager.instance != null && Gamemanager.instance.TowerManager != null)
            {
                // 타일 게임오브젝트 자체를 매니저에게 넘깁
                Gamemanager.instance.TowerManager.SellTile(this.gameObject);
            }
        }
    }
}
