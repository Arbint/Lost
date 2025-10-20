using System;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [SerializeField] GameObject mTurnIndicator;
    public float CooldownDuration => 1f / Speed;
    public float CooldownTimeRemaining { get; private set; }

    public event Action<BattleCharacter> onTurnStarted;

    public event Action OnTurnFinished;

    void Awake()
    {
        CooldownTimeRemaining = CooldownDuration;
        mTurnIndicator.SetActive(false);
    }

    public void TakeTurn()
    {
        Invoke("FinishTurn", 1);
        mTurnIndicator.SetActive(true);
        onTurnStarted?.Invoke(this);
        CooldownTimeRemaining = CooldownDuration;
    }

    public void FinishTurn()
    {
        mTurnIndicator.SetActive(false);
        OnTurnFinished?.Invoke();
    }

    internal void AdvanceCooldown(float advanceTime)
    {
        CooldownTimeRemaining -= advanceTime;
    }
}
