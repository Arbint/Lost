using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    Button mButton;
    [SerializeField] TextMeshProUGUI mAbilityNameText;
    void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(ActivateAbility);
    }
    Ability mAbility;
    public void SetAbility(Ability ability)
    {
        mAbility = ability;
        mAbilityNameText.SetText(ability.AbilityName);
    }

    void ActivateAbility()
    {
        mAbility.ActiateAbility();
    }
}
