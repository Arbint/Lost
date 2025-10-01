using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] Player mPlayerGameObjectPrefab;

    Player mPlayerGameObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PlayerStart playerStart = FindFirstObjectByType<PlayerStart>();
        if (!playerStart)
        {
            throw new System.Exception("Need a player start in the scene for the player spawn location and rotation");
        }

        mPlayerGameObject = Instantiate(mPlayerGameObjectPrefab, playerStart.transform.position, playerStart.transform.rotation);
    }
}
