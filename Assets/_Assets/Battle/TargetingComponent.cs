using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingComponent : MonoBehaviour
{
    BattleInputActions mBattleInputActions;
    Vector2 mNavigationInput;

    ITargetService mTargetService;

    List<BattleCharacter> mTargets = new List<BattleCharacter>();

    bool mNavigationReset = true;

    public void SetTargetService(ITargetService targetService)
    {
        mTargetService = targetService;
    }

    public void StartTargeting(int partyId, bool hostile)
    {
        mBattleInputActions.Enable();
        mTargets.Clear();
        mTargets = mTargetService.GetTargetsForTeam(partyId, hostile);
        mTargets[0].SetHighLighted(true);
    }

    void Awake()
    {
        mBattleInputActions = new BattleInputActions();
        mBattleInputActions.Battle.Navigation.performed += HandleTargetNavigation;
        mBattleInputActions.Battle.Navigation.canceled += HandleTargetNavigation;
        mBattleInputActions.Disable();
    }

    void OnEnable()
    {
        mBattleInputActions.Enable();
    }

    void OnDisable()
    {
        mBattleInputActions.Disable();
    }

    private void HandleTargetNavigation(InputAction.CallbackContext context)
    {
        mNavigationInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (mNavigationInput.sqrMagnitude > 0.5 && mNavigationReset)
        {
            mNavigationReset = false;
            Debug.Log($"Navigating with input X: {mNavigationInput.x}");
        }

        if(mNavigationInput.sqrMagnitude < 0.25)
        {
            mNavigationReset = true;
        }
    }
}
