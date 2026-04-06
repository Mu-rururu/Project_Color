using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool IsPendingRemove { get; private set; }

    public void Initialize()
    {
        IsPendingRemove = false;
    }

    public void Tick()
    {
        if (IsPendingRemove) return;
        if (!GameManager.Instance.IsSessionRunning) return;
    }

    public void OnHit()
    {
        if (IsPendingRemove) return;

        IsPendingRemove = true;
        NPCManager.Instance.RequestRemove(this);
    }
}