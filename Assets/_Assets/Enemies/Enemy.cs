using System;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    GameObject mTarget;
    GameObject Target
    {
        get { return mTarget; }
        set
        {
            mTarget = value;
        }
    }

    [SerializeField] float mEyeHeight = 1.5f;
    [SerializeField] float mSightDistance = 5f;
    [SerializeField] float mViewAngle = 30f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPerception();
    }

    private void UpdatePlayerPerception()
    {
        Debug.Log($"Checking Perception");
        Player player = GameMode.MainGameMode.mPlayer;
        if (!player)
        {
            Debug.Log($"Player Do not exist");
            Target = null;
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) > mSightDistance)
        {
            Target = null;
            Debug.Log($"Player too far");
            return;
        }

        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(playerDir, transform.forward) > mViewAngle)
        {
            Target = null;
            Debug.Log($"Player out of angle");
            return;
        }

        Vector3 eyeViewPoint = transform.position + Vector3.up * mEyeHeight;
        if (Physics.Raycast(eyeViewPoint, playerDir, out RaycastHit hitInfo, mSightDistance))
        {
            if (hitInfo.collider.gameObject != player.gameObject)
            {
                Target = null;
                Debug.Log($"Player blocked by: {hitInfo.collider.gameObject.name}");
                return;
            }
        }

        Target = player.gameObject;
    }

    void OnDrawGizmos()
    {
        Vector3 eyeViewPoint = transform.position + Vector3.up * mEyeHeight;
        Gizmos.DrawWireSphere(eyeViewPoint, mSightDistance);

        Vector3 leftLineDir = Quaternion.AngleAxis(mViewAngle, Vector3.up) * transform.forward;
        Vector3 rightLineDir = Quaternion.AngleAxis(-mViewAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(eyeViewPoint, eyeViewPoint + leftLineDir * mSightDistance);
        Gizmos.DrawLine(eyeViewPoint, eyeViewPoint + rightLineDir * mSightDistance);

        if (Target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Target.transform.position);
            Gizmos.DrawWireSphere(Target.transform.position, 0.5f);
        }
    }
}