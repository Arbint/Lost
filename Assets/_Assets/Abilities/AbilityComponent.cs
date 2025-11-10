using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] mInitialAbilities;
    [SerializeField] Transform mTargettingFollowTransform;
    List<Ability> mAbilities = new List<Ability>();

    IViewClient mOwnerViewClient;

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
        if(mOwnerViewClient is not null)
        {
            mOwnerViewClient.PushViewTarget(mTargettingFollowTransform);
        }

        TargetingComponent targetingComponent = GameMode.MainGameMode.BattleManager.GetTargetingComponent();
        targetingComponent.onTargetCancelled -= CancelTargeting;
        targetingComponent.onTargetCancelled += CancelTargeting;
        targetingComponent.StartTargeting(GetPartyID(), hostile);
    }

    private void CancelTargeting()
    {
        if(mOwnerViewClient is not null)
        {
            mOwnerViewClient.PopViewTarget(mTargettingFollowTransform);
        }
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
}
