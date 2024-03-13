using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MySingleton<GameManager>
{
    // �ٸ� GameManager ���� ���� �� �޼��� �߰� ����

    //����â ���� ������
    public enum GameState
    {
        MainMenu,
        InGame,
        GameOver
        // ��Ÿ �߰� ���� ����
    }

    public GameState CurrentGameState { get; private set; }

    // ���� ���� ���� �޼���
    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;

        // ���¿� ���� ó��
        switch (newState)
        {
            case GameState.MainMenu:
                // ���� �޴� �ʱ�ȭ
                break;

            case GameState.InGame:
                // ���� ���� �ʱ�ȭ
                break;

            case GameState.GameOver:
                // ���� ���� �ʱ�ȭ
                break;
        }
    }

    // ���� ���� �޼���
    public void StartGame()
    {
        SetGameState(GameState.InGame);
        // �ʿ��� �ʱ�ȭ �� ���� ���� �۾� ����
    }

    // ���� ���� �޼���
    public void EndGame()
    {
        SetGameState(GameState.GameOver);
        // ���� ���� ���� �۾� ����
    }

    // �� �ε� �޼���
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}