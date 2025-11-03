using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BattleWidget : MonoBehaviour
{
    [SerializeField] CharacterControlWidget mCharacterControlWidget;
    [SerializeField] LayoutGroup mAbilityListLayoutGroup;
    [SerializeField] AbilityWidget mAbilityWidgetPrefab;

    List<AbilityWidget> mAbilityWidgets = new List<AbilityWidget>();

    public void SetCharacterControlTarget(BattleCharacter battleCharacter)
    {
        foreach(Transform existingEntries in mAbilityListLayoutGroup.transform)
        {
            Destroy(existingEntries.gameObject);
        }

        mAbilityWidgets.Clear();

        mCharacterControlWidget.gameObject.SetActive(true);
        mCharacterControlWidget.SetBattleCharacter(battleCharacter);
        AbilityComponent abilityComponent = battleCharacter.GetAbilityComponent();
        if (abilityComponent)
        {
            foreach (Ability ability in abilityComponent.GetAbilities())
            {
                AddAbilityToAbilityList(ability);
            }
        }

        EventSystem.current.SetSelectedGameObject(mAbilityWidgets[0].gameObject); 
    }

    private void AddAbilityToAbilityList(Ability ability)
    {
        AbilityWidget newAbilityWidget = Instantiate(mAbilityWidgetPrefab, mAbilityListLayoutGroup.transform);
        mAbilityWidgets.Add(newAbilityWidget);
        newAbilityWidget.SetAbility(ability);
    }
}
