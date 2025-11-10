using System;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour, IViewClient
{
    [SerializeField] CameraRig mCameraRigPefab;
    [SerializeField] GameplayWidget mGameplayWidgetPrefab;

    GameplayWidget mGameplayWidget;

    private PlayerInputActions mPlayerInputActions;
    private MovementController mMovementController;
    private BattlePartyComponent mBattlePartyComponent;

    private BattleState mBattleState;

    CameraRig mCameraRig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mCameraRig = Instantiate(mCameraRigPefab);
        mCameraRig.PushFollowTransform(transform);

        mMovementController = GetComponent<MovementController>();

        mPlayerInputActions = new PlayerInputActions();


        mPlayerInputActions.Gameplay.Jump.performed += mMovementController.PerformJump;

        mPlayerInputActions.Gameplay.Move.performed += mMovementController.HandleMoveInput;
        mPlayerInputActions.Gameplay.Move.canceled += mMovementController.HandleMoveInput;

        mPlayerInputActions.Gameplay.Look.performed += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());
        mPlayerInputActions.Gameplay.Look.canceled += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());

        mBattlePartyComponent = GetComponent<BattlePartyComponent>();
        mBattlePartyComponent.onBAttleCharacterTakeTurn += BattleCharacterTakeTurn;
        mGameplayWidget = Instantiate(mGameplayWidgetPrefab);
    }

    private void BattleCharacterTakeTurn(BattleCharacter character)
    {
        mGameplayWidget.SetFocusedCharacterInBattle(character);
    }

    void OnEnable()
    {
        mPlayerInputActions.Enable();
    }

    void OnDisable()
    {
        mPlayerInputActions.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject)
        {
            return;
        }

        BattlePartyComponent otherBattlePartyComponent = other.GetComponent<BattlePartyComponent>();
        if (otherBattlePartyComponent && !IsInBattle())
        {
            GameMode.MainGameMode.BattleManager.StartBattle(mBattlePartyComponent, otherBattlePartyComponent);
            SwitchToBattleState(BattleState.InBattle);
        }
    }

    private void SwitchToBattleState(BattleState battleState)
    {
        if (battleState == BattleState.InBattle)
        {
            mPlayerInputActions.Gameplay.Disable();
        }

        if (battleState == BattleState.Roaming)
        {
            mPlayerInputActions.Gameplay.Enable();
        }

        mGameplayWidget.DipToBlack(1, 1, DippedToBlack);
    }

    void DippedToBlack()
    {
        Debug.Log($"Dipped To Black Called");
        mBattlePartyComponent.UpdateView();
        mGameplayWidget.SwitchToBattle();
    }

    private bool IsInBattle()
    {
        return mBattleState == BattleState.InBattle;
    }

    public void PushViewTarget(Transform viewTarget)
    {
        mCameraRig.PushFollowTransform(viewTarget);
        mCameraRig.transform.rotation = viewTarget.transform.rotation;
    }

    public void ResetViewAngle()
    {
        mCameraRig.ResetViewAngle();
    }

    public void PopViewTarget(Transform viewTarget)
    {
        mCameraRig.PopFollowTransform(viewTarget);
    }
}
