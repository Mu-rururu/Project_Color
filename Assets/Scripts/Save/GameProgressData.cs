using UnityEngine;
//진행 기록 저장용 스크립트
public static class GameProgressData
{
    public static bool HasSeenRedEnding { get; private set; }

    public static void MarkRedEndingSeen()
    {
        HasSeenRedEnding = true;
    }

    public static void ResetProgress()
    {
        HasSeenRedEnding = false;
    }
}