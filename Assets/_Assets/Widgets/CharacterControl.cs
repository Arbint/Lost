using System;
using UnityEngine;
using TMPro;

public class CharacterControlWidget : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mCharacterNameText;
    internal void SetBattleCharacter(BattleCharacter battleCharacter)
    {
        Debug.Log($"Setting Battle Character name to: {battleCharacter.gameObject.name}");
        mCharacterNameText.SetText(battleCharacter.Name);
    }
}
