using UnityEngine;
using System;

public enum PlayerState
{
    Black,
    Red
}

public class PlayerStateController : MonoBehaviour
{
    public static PlayerStateController Instance { get; private set; }

    public PlayerState CurrentState { get; private set; } = PlayerState.Black;
    public bool HasEnteredRedThisSession { get; private set; }

    [SerializeField] private int redThreshold = 30;

    public event Action<PlayerState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Initialize()
    {
        CurrentState = PlayerState.Black;
        HasEnteredRedThisSession = false;
        OnStateChanged?.Invoke(CurrentState);
    }

    public void EvaluateTransition(int currentScore)
    {
        if (!GameManager.Instance.IsSessionRunning) return;
        if (CurrentState == PlayerState.Red) return;

        if (currentScore >= redThreshold)
        {
            EnterRedState();
        }
    }

    private void EnterRedState()
    {
        CurrentState = PlayerState.Red;
        HasEnteredRedThisSession = true;
        OnStateChanged?.Invoke(CurrentState);
    }

    public bool CanReceivePlayerInput()
    {
        return CurrentState == PlayerState.Black && GameManager.Instance.IsSessionRunning;
    }

    public bool IsRedState()
    {
        return CurrentState == PlayerState.Red;
    }
}