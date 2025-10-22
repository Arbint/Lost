using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }
    AbilityComponent mOwningAbilityComponent;

    internal void Init(AbilityComponent newAbility)
    {
        mOwningAbilityComponent = newAbility; 
    }
}
