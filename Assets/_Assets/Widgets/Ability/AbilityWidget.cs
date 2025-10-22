using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    Button mButton;
    void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(ActivateAbility);
    }
    Ability mAbility;
    public void SetAbility(Ability ability)
    {
        mAbility = ability;
    }

    void ActivateAbility()
    {
        mAbility.ActiateAbility();
    }
}
