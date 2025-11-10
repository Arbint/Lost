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

    int mCurrentlySelectedTargetIndex = -1;

    public event Action<BattleCharacter> onTargetPicked;
    public event Action onTargetCancelled;

    public void SetTargetService(ITargetService targetService)
    {
        mTargetService = targetService;
    }

    public void StartTargeting(int partyId, bool hostile)
    {
        mBattleInputActions.Enable();
        mTargets.Clear();
        mTargets = mTargetService.GetTargetsForTeam(partyId, hostile);
        SetCurrentlySelectedTargetIndex(0);
    }

    void Awake()
    {
        mBattleInputActions = new BattleInputActions();
        mBattleInputActions.Battle.Navigation.performed += HandleTargetNavigation;
        mBattleInputActions.Battle.Navigation.canceled += HandleTargetNavigation;
        mBattleInputActions.Battle.Cancel.performed += CancelTargeting;
        mBattleInputActions.Battle.Confirm.performed += ConfirmTarget;
        mBattleInputActions.Disable();
    }

    private void ConfirmTarget(InputAction.CallbackContext context)
    {
        mBattleInputActions.Disable();
    }

    private void CancelTargeting(InputAction.CallbackContext context)
    {
        BattleCharacter battleCharacter = GetCurrentlySelectedTarget();
        if (battleCharacter)
        {
            battleCharacter.SetHighLighted(false);
        }

        mBattleInputActions.Disable();
        onTargetCancelled?.Invoke();
    }

    BattleCharacter GetCurrentlySelectedTarget()
    {
        if (mCurrentlySelectedTargetIndex >= 0 && mCurrentlySelectedTargetIndex < mTargets.Count)
        {
            return mTargets[mCurrentlySelectedTargetIndex];
        }

        return null;
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
            NavigateToNextTarget(mNavigationInput.x > 0 ? true : false);
        }

        if (mNavigationInput.sqrMagnitude < 0.25)
        {
            mNavigationReset = true;
        }
    }

    void NavigateToNextTarget(bool increment)
    {
        int newIndex = mCurrentlySelectedTargetIndex + (increment ? 1 : -1);
        if (newIndex < 0)
        {
            newIndex = mTargets.Count - 1;
        }

        if (newIndex >= mTargets.Count)
        {
            newIndex = 0;
        }

        SetCurrentlySelectedTargetIndex(newIndex);
    }

    void SetCurrentlySelectedTargetIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= mTargets.Count)
            return;

        if (mCurrentlySelectedTargetIndex >= 0 && mCurrentlySelectedTargetIndex < mTargets.Count)
        {
            mTargets[mCurrentlySelectedTargetIndex].SetHighLighted(false);
        }

        mCurrentlySelectedTargetIndex = newIndex;
        mTargets[mCurrentlySelectedTargetIndex].SetHighLighted(true);
    }
}
