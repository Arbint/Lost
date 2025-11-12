using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }
    public AbilityComponent OwningAbilityComponent { get; private set; }
    public event Action onAbilityEnded;
    public event Action onAbilityActivated;

    internal void Init(AbilityComponent newAbility)
    {
        OwningAbilityComponent = newAbility; 
    }

    public virtual void ActiateAbility()
    {
        Debug.Log($"Activating ability");
        onAbilityActivated?.Invoke();
    }

    public virtual void EndAbility()
    {
        Debug.Log($"End Ability");
        onAbilityEnded?.Invoke();
    }
}
