using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private float sessionDuration = 60f;
    public float RemainingTime { get; private set; }

    public event Action<float> OnTimeChanged;

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
        RemainingTime = sessionDuration;
        OnTimeChanged?.Invoke(RemainingTime);
    }

    public void Tick(float deltaTime)
    {
        if (!GameManager.Instance.IsSessionRunning) return;

        RemainingTime -= deltaTime;

        if (RemainingTime < 0f)
            RemainingTime = 0f;

        OnTimeChanged?.Invoke(RemainingTime);
    }

    public bool IsTimeOver()
    {
        return RemainingTime <= 0f;
    }
}