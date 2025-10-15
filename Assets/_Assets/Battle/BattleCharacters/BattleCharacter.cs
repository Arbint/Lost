using System;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [SerializeField] GameObject mTurnIndicator;
    public float CooldownDuration => 1f / Speed;
    public float CooldownTimeRemaining { get; private set; }

    public event Action OnTurnFinished;

    void Awake()
    {
        CooldownTimeRemaining = CooldownDuration;
        mTurnIndicator.SetActive(false);
    }

    public void TakeTurn()
    {
        Invoke("FinshTurn", 1);
        mTurnIndicator.SetActive(true);
    }

    public void FinishTurn()
    {
        mTurnIndicator.SetActive(false);
        OnTurnFinished?.Invoke();
    }
}
