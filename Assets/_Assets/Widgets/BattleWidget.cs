using UnityEngine;

public class BattleWidget : MonoBehaviour
{
    [SerializeField] CharacterControlWidget mCharacterControlWidget;

    public void SetCharacterControlTarget(BattleCharacter battleCharacter)
    {
        mCharacterControlWidget.gameObject.SetActive(true);
        mCharacterControlWidget.SetBattleCharacter(battleCharacter);
    }
}
