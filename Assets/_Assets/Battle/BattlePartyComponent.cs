using System;
using System.Collections.Generic;
using UnityEngine;

public class BattlePartyComponent : MonoBehaviour
{
    [SerializeField] BattleCharacter[] mBattleCharactersPrefabs;

    List<BattleCharacter> mBattleCharacters;

    IViewClient mOwnerViewClient;

    public event Action<BattleCharacter> onBAttleCharacterTakeTurn;

    [field: SerializeField] public int PartyID { get; private set; } = 0;

    void Awake()
    {
        mOwnerViewClient = GetComponent<IViewClient>();
    }

    public void FinishPrep()
    {
    }

    public void UpdateView()
    {
        if(mOwnerViewClient is not null)
        {
            mOwnerViewClient.SetViewTarget(mBattleCharacters[0].transform);
            mOwnerViewClient.ResetViewAngle();
        }
    }

    public List<BattleCharacter> GetBattleCharacters()
    {
        if (mBattleCharacters == null)
        {
            mBattleCharacters = new List<BattleCharacter>();
            foreach (BattleCharacter battleCharacter in mBattleCharactersPrefabs)
            {
                BattleCharacter newBattleCharacter = Instantiate(battleCharacter);
                newBattleCharacter.Init(PartyID);
                newBattleCharacter.onTurnStarted += CharacterInTurn;
                mBattleCharacters.Add(newBattleCharacter);
            }
        }

        return mBattleCharacters;
    }

    private void CharacterInTurn(BattleCharacter character)
    {
        onBAttleCharacterTakeTurn?.Invoke(character);
        if(mOwnerViewClient is not null && character)
        {
            mOwnerViewClient.SetViewTarget(character.transform);
        }
    }
}

