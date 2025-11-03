using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    public override void ActiateAbility()
    {
        base.ActiateAbility();
        int partyID = OwningAbilityComponent.GetPartyID();
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().StartTargeting(partyID, true);
    }
}
