using System;
using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        MainMenu,
        InGame,
        GameOver
    }

    GameState gameState;

    public GameObject startPanel;
    public Button startButton;

    public GameObject restartPanel;
    public Button restartButton;

    public TMP_Text scoreText;

    void Start()
    {
        gameState = GameState.MainMenu;

        startPanel.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);

        restartPanel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
        GlobalOberserver.AddListener<GlobalEvent_PlayerDead>(OnPlayerDead);

        Time.timeScale = 0;
    }

    void StartGame()
    {
        gameState = GameState.InGame;

        startPanel.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        restartPanel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        Time.timeScale = 1;
    }
    private void OnPlayerDead(object sender, EventArgs e)
    {
        GlobalEvent_PlayerDead playerDead = (GlobalEvent_PlayerDead)e;

        gameState = GameState.GameOver;

        startPanel.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        restartPanel.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        scoreText.gameObject.SetActive(true);
        scoreText.text = playerDead.distance.ToString();

        Time.timeScale = 0;
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
