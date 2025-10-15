using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    List<BattleSite> mBattleSites;
    List<BattleCharacter> mBattleCharacters = new List<BattleCharacter>();

    public void StartBattle(BattlePartyComponent playerParty, BattlePartyComponent enemyParty)
    {
        mBattleCharacters.Clear();
        if (mBattleSites == null)
        {
            mBattleSites = new List<BattleSite>();
            mBattleSites.AddRange(GameObject.FindObjectsByType<BattleSite>(FindObjectsSortMode.None));
        }

        PrepParty(playerParty);
        PrepParty(enemyParty);
        StartCoroutine(StartTurns());
    }

    IEnumerator StartTurns()
    {
        //TODO: refactor to not hard code the delay
        yield return new WaitForSeconds(2);
        NextTurn();
    }

    void NextTurn()
    {

    }

    private void PrepParty(BattlePartyComponent party)
    {
        BattleSite partyBattleSite = mBattleSites.Find((battleSite) => { return !battleSite.IsPlayerSite; });
        if (party.gameObject.CompareTag("Player"))
        {
            partyBattleSite = mBattleSites.Find((battleSite) => { return battleSite.IsPlayerSite; });
        }

        int i = 0;
        foreach (BattleCharacter partyBattleCharacter in party.GetBattleCharacters())
        {
            partyBattleCharacter.transform.position = partyBattleSite.GetPositionForUnit(i);
            partyBattleCharacter.transform.rotation = partyBattleSite.transform.rotation;
            partyBattleCharacter.OnTurnFinished += NextTurn;
            mBattleCharacters.Add(partyBattleCharacter);
            i++;
        }

        party.FinishPrep();
    }
}
