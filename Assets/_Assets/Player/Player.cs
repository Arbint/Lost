using System;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour
{
    [SerializeField] CameraRig mCameraRigPefab;

    private PlayerInputActions mPlayerInputActions;
    private MovementController mMovementController;

    private BattleState mBattleState;

    CameraRig mCameraRig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mCameraRig = Instantiate(mCameraRigPefab);
        mCameraRig.SetFollowTransform(transform);

        mMovementController = GetComponent<MovementController>();

        mPlayerInputActions = new PlayerInputActions();


        mPlayerInputActions.Gameplay.Jump.performed += mMovementController.PerformJump;

        mPlayerInputActions.Gameplay.Move.performed += mMovementController.HandleMoveInput;
        mPlayerInputActions.Gameplay.Move.canceled += mMovementController.HandleMoveInput;

        mPlayerInputActions.Gameplay.Look.performed += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());
        mPlayerInputActions.Gameplay.Look.canceled += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());
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
        }
    }

    private bool IsInBattle()
    {
        return mBattleState == BattleState.InBattle;
    }
}
