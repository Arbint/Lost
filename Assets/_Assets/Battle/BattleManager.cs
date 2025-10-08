using UnityEngine;

public class BattleManager 
{
    public void StartBattle(BattlePartyComponent partyOne, BattlePartyComponent partyTwo)
    {
        Debug.Log($"Staring Battle between: {partyOne.gameObject.name} and {partyTwo.gameObject.name}");
    }
}
