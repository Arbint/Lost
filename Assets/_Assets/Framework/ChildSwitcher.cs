using System.Collections.Generic;
using UnityEngine;

public class ChildSwitcher : MonoBehaviour
{
    List<GameObject> mChildGameObjects = new List<GameObject>();
    int mCurrentActiveChildIndex = 0;
    void Awake()
    {
        foreach (Transform childTransform in transform)
        {
            mChildGameObjects.Add(childTransform.gameObject);
        }

        SetActiveChildByIndex(mCurrentActiveChildIndex);
    }

    public void SetActiveChild(GameObject childToSwitchTo)
    {
        int childIndex = mChildGameObjects.FindIndex((x)=> { return childToSwitchTo == x; });
        SetActiveChildByIndex(childIndex);
    }

    public void SetActiveChildByIndex(int newActiveChildIndex)
    {
        if (newActiveChildIndex < 0 || newActiveChildIndex >= mChildGameObjects.Count)
        {
            return;
        }

        foreach (GameObject childGameObject in mChildGameObjects)
        {
            childGameObject.SetActive(false);
        }

        mCurrentActiveChildIndex = newActiveChildIndex;
        mChildGameObjects[mCurrentActiveChildIndex].SetActive(true);
    }
}
