using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    [SerializeField] float mJumpSpeed = 15f;
    [SerializeField] float mMaxMoveSpeed = 5f;
    [SerializeField] float mGroundMoveSpeedAcceleration = 40f;
    [SerializeField] float mAirMoveSpeedAcceleration = 5f;

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

        UpdateHorizontalVelocity();
        mCharacterController.Move((mHorizontalVelocity + mVerticalVelocity) * Time.deltaTime);
    }

    void UpdateHorizontalVelocity()
    {
        Vector3 moveDir = PlayerInputToWorldDir(mMoveInput);

        float acceleration = mCharacterController.isGrounded ? mGroundMoveSpeedAcceleration : mAirMoveSpeedAcceleration;

        if (moveDir.sqrMagnitude > 0)
        {
            mHorizontalVelocity += moveDir * acceleration * Time.deltaTime;
            mHorizontalVelocity = Vector3.ClampMagnitude(mHorizontalVelocity, mMaxMoveSpeed);
        }
        else
        {
            if (mHorizontalVelocity.sqrMagnitude > 0)
            {
                mHorizontalVelocity -= mHorizontalVelocity.normalized * acceleration * Time.deltaTime;
                if (mHorizontalVelocity.sqrMagnitude < 0.1)
                {
                    mHorizontalVelocity = Vector3.zero;
                }
            }
        }
    }

    Vector3 PlayerInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = Camera.main.transform.right;
        Vector3 fwdDir = Vector3.Cross(rightDir, Vector3.up);

        return rightDir * inputVal.x + fwdDir * inputVal.y; 
    }
}
