using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    public override void ActiateAbility()
    {
        base.ActiateAbility();
        int partyID = OwningAbilityComponent.GetPartyID();
        List<BattleCharacter> targets = GameMode.MainGameMode.BattleManager.GetTargetsForTeam(partyID, true);
        foreach(BattleCharacter battleCharacter in targets)
        {
            Debug.Log($"Found target: {battleCharacter.gameObject.name}");
        }
    }
}
