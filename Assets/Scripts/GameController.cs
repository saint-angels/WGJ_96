using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PLAYING,
    PLAYER_DEAD
}

public class GameController : SingletonComponent<GameController>
{
    public GameState State { get; private set; }


    [SerializeField] private GameObject deathPanel;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    private int Score
    {
        set
        {
            score = value;
            scoreText.text = score.ToString("0000");
        }
        get
        {
            return score;
        }
    }

    int score = 0;



    public void AddScore(int blocksDestroyed)
    {
        //DOTween.To(() => scoreText.transform.localScale, (newScale) => scoreText.transform.localScale = newScale, 1.5f, .5f);
        
        Score += blocksDestroyed;
    }


    public void SetState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.PLAYING:
                GridManager.Instance.Init();
                deathPanel.SetActive(false);
                Score = 0;
                break;
            case GameState.PLAYER_DEAD:
                print("player gun died");
                GridManager.Instance.StopAllCoroutines();
                deathPanel.SetActive(true);
                break;
        }
    }

    void Start()
    {
        SetState(GameState.PLAYING);
        GridManager.Instance.OnPlayerDeath += () => SetState(GameState.PLAYER_DEAD);
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameState.PLAYING:
                break;
            case GameState.PLAYER_DEAD:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetState(GameState.PLAYING);
                }
                
                break;
        }
    }
}
