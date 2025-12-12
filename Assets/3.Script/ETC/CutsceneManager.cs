using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    [Header("연결할 UI")]
    [SerializeField] private GameObject cutscenePanel; // 컷신 전체 패널
    [SerializeField] private Image towerDisplayImage;  // 타워 그림 보여줄 이미지
    [SerializeField] private float cutsceneDuration = 2.0f; // 컷신 지속 시간

    private void Awake()
    {
        instance = this;
    }

    // 4티어 타워가 뜨면 이 함수를 호출할 겁니다
    public void PlayCutscene(TowerSO towerData)
    {
        StartCoroutine(CutsceneRoutine(towerData));
    }

    private IEnumerator CutsceneRoutine(TowerSO towerData)
    {
        // 1. 게임 일시정지 (몬스터도 멈춤, 뽕맛 UP)
        Time.timeScale = 0f;

        // 2. 타워 이미지 교체 및 패널 켜기
        // TowerSO에 'illust'나 'sprite'가 있다고 가정합니다. 
        // 만약 TowerSO에 일러스트용 변수가 없다면 prefabs의 스프라이트를 가져옵니다.
        Sprite towerSprite = towerData.prefabs.GetComponent<SpriteRenderer>().sprite;

        towerDisplayImage.sprite = towerSprite;
        
        cutscenePanel.SetActive(true);

        // 3. 리얼타임으로 대기 (Time.timeScale이 0이라도 이건 흐름)
        yield return new WaitForSecondsRealtime(cutsceneDuration);

        // 4. 컷신 끄고 게임 재개
        cutscenePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
