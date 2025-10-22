using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] mInitialAbilities;
    List<Ability> mAbilities = new List<Ability>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       foreach(Ability initialAbility in mInitialAbilities)
       {
            GiveAbility(initialAbility);
       }
    }

    private void GiveAbility(Ability abiltyDefaultObject)
    {
        Ability newAbility = Instantiate(abiltyDefaultObject);
        newAbility.Init(this);
        mAbilities.Add(newAbility);
    }
}
