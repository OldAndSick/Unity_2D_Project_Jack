using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;

    // 게임 시작 시 팝업을 바로 띄울지 여부 (유니티 인스펙터에서 설정)
    public bool ShowOnStart = true;

    private void Start()
    {
        // 씬 로드 시 바로 띄우기
        if (ShowOnStart)
        {
            OpenPopup();
        }
        else
        {
            // 혹시 몰라 인스펙터에서 팝업이 켜져있다면 꺼줍니다.
            ClosePopup();
        }

        // 팝업이 켜져 있는 동안은 게임을 멈춥니다.
        if (popupPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    // 팝업을 여는 함수
    public void OpenPopup()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f; // 게임 시간 정지 (필수)
    }

    // 팝업을 닫는 함수 (버튼에 연결할 핵심 함수)
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f; // 게임 시간 다시 정상 재생 (필수)
    }
}
