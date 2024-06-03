using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int health;
    public int money;

    [Header("Components")]
    public TextMeshProUGUI healthAndMoneyText;
    public EnemyPath enemyPath;
    public TowerPlacement towerPlacement;
    public EndScreenUI endScreen;
    public WaveSpawner waveSpawner;

    [Header("Events")]
    public UnityEvent onEnemyDestroyed;
    public UnityEvent onMoneyChanged;

    //Singleton
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateHealthAndMoneyText();
    }
    void UpdateHealthAndMoneyText()
    {
        healthAndMoneyText.text = $"Health: {health}\nMoney: ${money}";
    }

    public void AddMoney(int amount)
    {
        money += amount;

        UpdateHealthAndMoneyText();
        onMoneyChanged.Invoke();
    }

    public void TakeMoney(int amount)
    {
        money -= amount;

        UpdateHealthAndMoneyText();
        onMoneyChanged.Invoke();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        UpdateHealthAndMoneyText();

        if (health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        endScreen.gameObject.SetActive(true);
        endScreen.SetEndScreen(false, waveSpawner.curWave);

    }

    void WinGame()
    {
        endScreen.gameObject.SetActive(true);
        endScreen.SetEndScreen(true, waveSpawner.curWave);
    }

    public void OnEnemyDestroyed()
    {
        if (waveSpawner.remainingEnemies == 0 && waveSpawner.curWave == waveSpawner.waves.Length)
        {
            WinGame();
        }
    }
}
