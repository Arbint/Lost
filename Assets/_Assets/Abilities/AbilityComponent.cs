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

    public event Action onMoveToTargetFinished;
    public event Action onMoveBackToPartySpotFinished;
    public event Action<string> onGameplayEventReceived;

    NavMeshAgent mNavMeshAgent;

    Vector3 mPartySpotPosition;
    Quaternion mPartySpotRotation;

    bool mHasReachedDestination = true;
    bool mIsMovingBackFinished = true;

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

        mPartySpotPosition = transform.position;
        mPartySpotRotation = transform.rotation;
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
        mHasReachedDestination = false;
        mNavMeshAgent.SetDestination(targetPosition);
    }

    void Update()
    {
        UpdateNavigation(); 
        if(mHasReachedDestination && !mIsMovingBackFinished)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, mPartySpotRotation, Time.deltaTime * 20f);
            if(Quaternion.Angle(transform.rotation, mPartySpotRotation) < 1f)
            {
                transform.rotation = mPartySpotRotation;
                mIsMovingBackFinished = true;
                onMoveBackToPartySpotFinished?.Invoke();
            }
        }
    }

    private void UpdateNavigation()
    {
        if (mHasReachedDestination)
            return;

        if (mNavMeshAgent.pathPending)
            return;

        if (mNavMeshAgent.remainingDistance > mNavMeshAgent.stoppingDistance)
            return;

        if(!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
        {
            mHasReachedDestination = true;
            onMoveToTargetFinished?.Invoke();
        }
    }

    public void HandleGameplayEvent(string eventTag)
    {
        Debug.Log($"handling event with tag: {eventTag}");
        onGameplayEventReceived?.Invoke(eventTag);
    }

    internal void MoveBackToPartySpot()
    {
        Debug.Log($"Moving back!");
        mIsMovingBackFinished = false;
        MoveToTarget(mPartySpotPosition);
    }
}
