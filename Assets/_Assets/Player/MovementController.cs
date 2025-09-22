using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    [SerializeField] float mJumpSpeed = 15f;
    [SerializeField] float mMaxMoveSpeed = 5f;
    [SerializeField] float mMoveSpeedAcceleration = 5f;
    [SerializeField] float mMaxFallSpeed = 50f;
    private PlayerInputActions mPlayerInputActions;
    private CharacterController mCharacterController;

    private Vector3 mVerticalVelocity;
    private Vector3 mHorizontalVelocity;
    private Vector2 mMoveInput;

    void Awake()
    {
        mPlayerInputActions = new PlayerInputActions();
        mPlayerInputActions.Gameplay.Jump.performed += PerformJump;

        mPlayerInputActions.Gameplay.Move.performed += HandleMoveInput;
        mPlayerInputActions.Gameplay.Move.canceled += HandleMoveInput;

        mCharacterController = GetComponent<CharacterController>();
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        mMoveInput = context.ReadValue<Vector2>();
        Debug.Log($"move input is: {mMoveInput}");
    }

    private void PerformJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jumping!");
        if (mCharacterController.isGrounded)
        {
            mVerticalVelocity.y = mJumpSpeed;
        }
    }

    void OnEnable()
    {
        mPlayerInputActions.Enable();
    }

    void OnDisable()
    {
        mPlayerInputActions.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mVerticalVelocity.y > -mMaxFallSpeed)
        {
            mVerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        mHorizontalVelocity += mMoveInput.x * Camera.main.transform.right * mMoveSpeedAcceleration * Time.deltaTime;
        mHorizontalVelocity = Vector3.ClampMagnitude(mHorizontalVelocity, mMaxMoveSpeed);

        mCharacterController.Move((mHorizontalVelocity + mVerticalVelocity) * Time.deltaTime);
    }
}
