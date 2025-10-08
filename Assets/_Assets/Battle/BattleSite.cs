using UnityEngine;
using UnityEngine.PlayerLoop;

public class BattleSite : MonoBehaviour
{
    [SerializeField] float mSiteRadius;
    [SerializeField, Range(0, 5)] int mSiteCapacity;
    [SerializeField] bool mIsPlayerSite = false;

    public bool IsPlayerSite => mIsPlayerSite;

    // zero indexed, first unit shouuld have a index of 0
    public Vector3 GetPositionForUnit(int index)
    {
        //Edge case
        if (mSiteCapacity <= 1)
        {
            return transform.position;
        }

        float gap = mSiteRadius * 2 / (mSiteCapacity - 1);
        Vector3 startingPoint = transform.position - transform.right * mSiteRadius;

        return startingPoint + index * gap * transform.right;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = mIsPlayerSite ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, mSiteRadius);
        for (int i = 0; i < mSiteCapacity; ++i)
        {
            Gizmos.DrawSphere(GetPositionForUnit(i), 0.5f);
        }
    }
}
