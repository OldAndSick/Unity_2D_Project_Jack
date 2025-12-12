using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    [Header("마나 및 웨이브")]
    [SerializeField] private float currentMana = 50;
    [SerializeField] private int waveCount = 0;
    public static Gamemanager instance;
    public TowerManager TowerManager;

    public float CurrentMana => currentMana;
    public int WaveCount => waveCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        if (TowerManager == null)
        {
            Debug.LogError(" [GameManager] TowerManager가 Inspector에 연결되지 않았습니다! 다시 확인하십시오.");
        }
        else
        {
            Debug.Log("[GameManager] TowerManager 연결 확인 완료.");
        }
    }
    public bool UseMana(float amount)
    {
        if(currentMana >= amount)
        {
            currentMana -= amount;
            return true;

        }
        return false;

    }
    public void AddMana(float amount)
    {
        currentMana += amount;

    }
    public void AddWave(int amount)
    {
        waveCount += amount;
    }
}
