using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MySingleton<GameManager>
{
    // 다른 GameManager 관련 변수 및 메서드 추가 가능

    //게임창 상태 열거형
    public enum GameState
    {
        MainMenu,
        InGame,
        GameOver
        // 기타 추가 상태 가능
    }

    public GameState CurrentGameState { get; private set; }

    // 게임 상태 설정 메서드
    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;

        // 상태에 따른 처리
        switch (newState)
        {
            case GameState.MainMenu:
                // 메인 메뉴 초기화
                break;

            case GameState.InGame:
                // 게임 시작 초기화
                break;

            case GameState.GameOver:
                // 게임 종료 초기화
                break;
        }
    }

    // 게임 시작 메서드
    public void StartGame()
    {
        SetGameState(GameState.InGame);
        // 필요한 초기화 및 게임 시작 작업 수행
    }

    // 게임 종료 메서드
    public void EndGame()
    {
        SetGameState(GameState.GameOver);
        // 게임 종료 관련 작업 수행
    }

    // 씬 로딩 메서드
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}