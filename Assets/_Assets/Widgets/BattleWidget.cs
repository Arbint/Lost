using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleWidget : MonoBehaviour
{
    [SerializeField] CharacterControlWidget mCharacterControlWidget;
    [SerializeField] LayoutGroup mAbilityListLayoutGroup;
    [SerializeField] AbilityWidget mAbilityWidgetPrefab;

    public void SetCharacterControlTarget(BattleCharacter battleCharacter)
    {
        foreach(Transform existingEntries in mAbilityListLayoutGroup.transform)
        {
            Destroy(existingEntries);
            Debug.Log($"may be not destory this??");
        }

        mCharacterControlWidget.gameObject.SetActive(true);
        mCharacterControlWidget.SetBattleCharacter(battleCharacter);
        AbilityComponent abilityComponent = battleCharacter.GetAbilityComponent();
        if(abilityComponent)
        {
            foreach(Ability ability in abilityComponent.GetAbilities())
            {
                AddAbilityToAbilityList(ability);
            }
        }
    }

    private void AddAbilityToAbilityList(Ability ability)
    {
        AbilityWidget newAbilityWidget = Instantiate(mAbilityWidgetPrefab, mAbilityListLayoutGroup.transform);
        newAbilityWidget.SetAbility(ability);
    }
}
