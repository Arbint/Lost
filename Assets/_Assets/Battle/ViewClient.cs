using UnityEngine;

public interface IViewClient
{
    public void PushViewTarget(Transform viewTarget);
    public void PopViewTarget(Transform viewTarget);
    public void ResetViewAngle();
}
