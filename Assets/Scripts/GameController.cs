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

    public void SetState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.PLAYING:
                GridManager.Instance.Init();
                deathPanel.SetActive(false);
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
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    SetState(GameState.PLAYING);
                }
                
                break;
        }
    }
}
