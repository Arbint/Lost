using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] mInitialAbilities;
    [SerializeField] Transform mTargettingFollowTransform;
    List<Ability> mAbilities = new List<Ability>();

    IViewClient mOwnerViewClient;

    public event Action onTargetCancelled;
    public event Action<BattleCharacter> onTargetPicked;

    NavMeshAgent mNavMeshAgent;

    void Awake()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public int GetPartyID()
    {
        return GetComponent<BattleCharacter>().PartyID;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Ability initialAbility in mInitialAbilities)
        {
            GiveAbility(initialAbility);
        }
    }

    public void StartTargeting(bool hostile)
    {
        if (mOwnerViewClient is not null)
        {
            mOwnerViewClient.PushViewTarget(mTargettingFollowTransform);
        }

        TargetingComponent targetingComponent = GameMode.MainGameMode.BattleManager.GetTargetingComponent();
        SubscribeToTargetingDelegates();
        targetingComponent.StartTargeting(GetPartyID(), hostile);
    }

    void SubscribeToTargetingDelegates()
    {
        UnSubscribeToTargetingDelegates();
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetCancelled += CancelTargeting;
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetPicked += TargetPicked;
    }

    void UnSubscribeToTargetingDelegates()
    {
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetCancelled -= CancelTargeting;
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetPicked -= TargetPicked;
    }

    private void TargetPicked(BattleCharacter character)
    {
        UnSubscribeToTargetingDelegates();
        if (mOwnerViewClient is not null)
        {
            mOwnerViewClient.PopViewTarget(mTargettingFollowTransform);
        }
        onTargetPicked?.Invoke(character);
    }

    private void CancelTargeting()
    {
        UnSubscribeToTargetingDelegates();
        if (mOwnerViewClient is not null)
        {
            mOwnerViewClient.PopViewTarget(mTargettingFollowTransform);
        }

        onTargetCancelled?.Invoke();
    }

    private void GiveAbility(Ability abiltyDefaultObject)
    {
        Ability newAbility = Instantiate(abiltyDefaultObject);
        newAbility.Init(this);
        mAbilities.Add(newAbility);
    }

    internal IEnumerable<Ability> GetAbilities()
    {
        return mAbilities;
    }

    internal void SetViewClient(IViewClient viewClient)
    {
        mOwnerViewClient = viewClient;
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        mNavMeshAgent.SetDestination(targetPosition);
    }
}
