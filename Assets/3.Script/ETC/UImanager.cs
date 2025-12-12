using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UImanager : MonoBehaviour
{
    [Header("데이터 출처")]
    [SerializeField] private EnemyManager enemyManager; // EnemyManager 연결
    [SerializeField] private Gamemanager gameManager;   // GameManager 연결
    [SerializeField] private PlayerHealth playerHealth;   // 플레이어 체력 스크립트 연결 (가정)

    [Header("UI 텍스트 요소")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI keyInfoText; // 키 설명은 고정값이므로 Update에서 제외

    private void Start()
    {
        // 팝업이 닫힌 후 UIManager가 활성화될 때 한 번 초기화합니다.
        // keyInfoText는 정적 정보이므로 Start에서만 처리합니다.

        // 형님이 직접 KeyInfoText에 내용을 채웠다면 이 코드는 필요 없습니다.
        // 만약 코드에서 설정하고 싶다면 아래와 같이 씁니다.
        // keyInfoText.text = "1키: 타일 설치\n2키: 타워 설치\nS키: 웨이브 시작";

        // 만약 Start에서 팝업이 닫히지 않고 TimeScale=0이라면, 팝업 닫는 로직 후에
        // UIManager를 활성화시켜야 합니다.
    }

    private void Update()
    {
        // 매 프레임마다 데이터 출처에서 값을 가져와서 UI를 업데이트합니다.
        UpdateDynamicUI();
    }

    private void UpdateDynamicUI()
    {
        // 1. 웨이브 업데이트
        if (enemyManager != null && waveText != null)
        {
            // EnemyManager.CurrentWave (혹은 GetCurrentWave())를 사용합니다.
            waveText.text = $"<color=#FFFF00>웨이브:</color> {enemyManager.currentWave}";
        }

        // 2. 마나 업데이트
        if (gameManager != null && manaText != null)
        {
            // GameManager.CurrentMana 속성을 사용한다고 가정합니다.
            manaText.text = $"<color=#00CCFF>마나:</color> {gameManager.CurrentMana}";
        }

        //3. 체력 업데이트
        if (playerHealth != null && hpText != null)
        {
            // PlayerStats.CurrentHP 속성을 사용한다고 가정합니다.
            float healthPercentage = playerHealth.CurrentHealth;
            hpText.text = $"<color=#FF0000>체력:</color> {healthPercentage:F0}"; // 정수만 표시
        }
    }
}
