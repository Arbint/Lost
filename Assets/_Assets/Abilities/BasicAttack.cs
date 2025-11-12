using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    BattleCharacter mTarget;
    [SerializeField] float mDamageAmt = 20f;
    public override void ActiateAbility()
    {
        base.ActiateAbility();
        OwningAbilityComponent.StartTargeting(true);

        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;

        OwningAbilityComponent.onTargetPicked += TargetPicked;
        OwningAbilityComponent.onTargetCancelled += TargetCancelled;
    }

    private void TargetCancelled()
    {
        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;
        EndAbility();
    }

    private void TargetPicked(BattleCharacter character)
    {
        mTarget = character;
        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;

        Debug.Log($"atacking: {character.gameObject.name}");

        OwningAbilityComponent.MoveToTarget(character.transform.position);

        OwningAbilityComponent.onMoveToTargetFinished -= MovedToTarget;
        OwningAbilityComponent.onMoveToTargetFinished += MovedToTarget;
    }

    private void MovedToTarget()
    {
        OwningAbilityComponent.onMoveToTargetFinished -= MovedToTarget;
        OwningAbilityComponent.GetComponent<Animator>().SetTrigger("Attack");
        OwningAbilityComponent.onGameplayEventReceived += HandleGameplayEvent;
    }

    private void HandleGameplayEvent(string eventTag)
    {
        if (eventTag == "ApplyDamage")
        {
            mTarget.TakeDamage(mDamageAmt);
            return;
        }

        if(eventTag == "AttackFinished")
        {
            OwningAbilityComponent.MoveBackToPartySpot(); 
        }
    }
}
