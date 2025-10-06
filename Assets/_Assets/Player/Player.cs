using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour
{
    [SerializeField] CameraRig mCameraRigPefab;

    private PlayerInputActions mPlayerInputActions;
    private MovementController mMovementController;

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
}
