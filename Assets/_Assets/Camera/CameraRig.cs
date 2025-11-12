using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] float mHeightOffset = 0.5f;
    [SerializeField] float mFollowLerpRate = 20f;

    [SerializeField] Transform mYawTransform;
    [SerializeField] Transform mPitchTransform;
    [SerializeField] float mRotationRate;
    [SerializeField] float mPitchMin = -89f;
    [SerializeField] float mPitchMax = 89f;
    Transform FollowTransform
    {
        get
        {
            if (mFollowTransforms.Count > 0)
            {
                return mFollowTransforms.Last();
            }

            return null;
        }
    }

    List<Transform> mFollowTransforms = new List<Transform>();

    Vector2 mLookInput;

    float mPitch;

    public void SetLookInput(Vector2 lookInput)
    {
        mLookInput = lookInput;
    }

    public void PushFollowTransform(Transform followTransform)
    {
        mFollowTransforms.Add(followTransform);
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, FollowTransform.position + mHeightOffset * Vector3.up, mFollowLerpRate * Time.deltaTime);

        mYawTransform.rotation *= Quaternion.AngleAxis(mLookInput.x * mRotationRate * Time.deltaTime, Vector3.up);

        mPitch = mPitch + mRotationRate * Time.deltaTime * mLookInput.y;
        mPitch = Mathf.Clamp(mPitch, mPitchMin, mPitchMax);
        mPitchTransform.localEulerAngles = new Vector3(mPitch, 0f, 0f);
    }

    internal void ResetViewAngle()
    {
        mPitch = 0f;
        mYawTransform.localRotation = Quaternion.identity;
    }

    internal void PopFollowTransform(Transform viewTarget)
    {
        mFollowTransforms.RemoveAll(target=>viewTarget == target);
    }
}
