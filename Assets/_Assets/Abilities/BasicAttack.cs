using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    public override void ActiateAbility()
    {
        base.ActiateAbility();
        OwningAbilityComponent.StartTargeting(true);
    }
}
