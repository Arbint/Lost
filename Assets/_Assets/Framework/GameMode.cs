using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] Player mPlayerGameObjectPrefab;

    Player mPlayerGameObject;

    public Player mPlayer => mPlayerGameObject;
    public static GameMode MainGameMode;

    public BattleManager BattleManager { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnDestroy()
    {
        if (MainGameMode == this)
        {
            MainGameMode = null;
        }
    }

    void Awake()
    {
        if (MainGameMode != null)
        {
            Destroy(gameObject);
        }

        MainGameMode = this;

        BattleManager = new BattleManager();

        PlayerStart playerStart = FindFirstObjectByType<PlayerStart>();
        if (!playerStart)
        {
            throw new System.Exception("Need a player start in the scene for the player spawn location and rotation");
        }

        mPlayerGameObject = Instantiate(mPlayerGameObjectPrefab, playerStart.transform.position, playerStart.transform.rotation);
    }
}
