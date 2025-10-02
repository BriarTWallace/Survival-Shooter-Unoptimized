using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static int score;


    [SerializeField] Text text;

    [System.Serializable]
    public class ScoreChangedEvent : UnityEvent<int> { }

    public ScoreChangedEvent OnScoreChanged;

    void Awake()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }
}
