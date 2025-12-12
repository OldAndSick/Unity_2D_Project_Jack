using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private float maxHealth = 100f; // 최대 체력
    [SerializeField] private float currentHealth;    // 현재 체력

    [Header("시스템 연결")]
    // 게임 오버 시 필요한 경우 GameManager 등을 연결합니다.
    // [SerializeField] private Gamemanager gameManager; 

    // UIManager가 값을 읽을 수 있도록 public 속성으로 노출
    public float CurrentHealth => currentHealth;

    private void Start()
    {
        // 게임 시작 시 최대 체력으로 초기화
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 외부에서 데미지를 줄 때 호출하는 함수입니다.
   
    /// <param name="damageAmount">받은 데미지 양</param>
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0) return; // 이미 죽었으면 처리 안 함

        // 데미지를 감소시킵니다.
        currentHealth -= damageAmount;

        // 체력이 0보다 작아지지 않도록 처리합니다.
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"[플레이어] 데미지 {damageAmount} 받음. 남은 체력: {currentHealth}");

        // 체력이 0 이하가 되면 게임 오버 처리
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(" 게임 오버! 플레이어가 사망했습니다.");

        // 1. 플레이어 오브젝트 비활성화 또는 파괴
        gameObject.SetActive(false);

        // 2. 게임 오버 씬 로드 (선택 사항)
         SceneManager.LoadScene("GameOver");

        // 3. (옵션) GameManager에 게임 오버 상태를 알림
        // if (gameManager != null) gameManager.SetGameOver(true);
    }
}
