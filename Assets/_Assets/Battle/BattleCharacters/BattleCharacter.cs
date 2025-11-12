using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AbilityComponent))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [field: SerializeField] public string Name { get; private set; } = "BattleCharacter";
    [SerializeField] GameObject mTurnIndicator;
    public float CooldownDuration => 1f / Speed;
    public float CooldownTimeRemaining { get; private set; }
    public event Action<BattleCharacter> onTurnStarted;
    public event Action OnTurnFinished;
    AbilityComponent mAbilityComponent;

    public int PartyID { get; private set; }

    NavMeshAgent mNavMeshAgent;

    Animator mAnimator;

    public void Init(int partyID, IViewClient viewClient)
    {
        PartyID = partyID;
        if(mAbilityComponent == null)
        {
            mAbilityComponent = GetComponent<AbilityComponent>();
        }

        if(mAbilityComponent)
        {
            mAbilityComponent.SetViewClient(viewClient);
        }
    }

    public AbilityComponent GetAbilityComponent()
    {
        return mAbilityComponent;
    }

    void Awake()
    {
        CooldownTimeRemaining = CooldownDuration;
        mTurnIndicator.SetActive(false);

        mAbilityComponent = GetComponent<AbilityComponent>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        mAnimator.SetFloat("Speed", mNavMeshAgent.velocity.magnitude);
    }

    public void SetHighLighted(bool highted)
    {
        mTurnIndicator.SetActive(highted);
    }

    public void TakeTurn()
    {
        SetHighLighted(true);
        onTurnStarted?.Invoke(this);
        CooldownTimeRemaining = CooldownDuration;
    }

    public void FinishTurn()
    {
        SetHighLighted(false);
        OnTurnFinished?.Invoke();
    }

    internal void AdvanceCooldown(float advanceTime)
    {
        CooldownTimeRemaining -= advanceTime;
    }

    internal void WarpNavPositionTo(Vector3 position)
    {
        mNavMeshAgent.Warp(position);
    }

    internal void TakeDamage(float mDamageAmt)
    {
        Debug.Log($"{gameObject.name} Taking damage: {mDamageAmt}");
    }
}
