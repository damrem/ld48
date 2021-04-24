using Cinemachine;
using Damrem.Procedural;
using UnityEngine;

[RequireComponent(typeof(ExitSystem))]
[RequireComponent(typeof(PlayerPickCoinSystem))]
[RequireComponent(typeof(PlayerMovementSystem))]
[RequireComponent(typeof(PlayerGravitySystem))]
public class GameManager : MonoBehaviour {
    public int Seed = 0;
    public Player PlayerPrefab;
    public Block BlockPrefab;
    public Exit ExitPrefab;
    public Coin CoinPrefab;
    public Purse Purse;
    public Color[] Colors;
    public LevelDef[] LevelDefs;

    PRNG PRNG;
    int CurrentLevelIndex = 0;
    Level CurrentLevel;
    Player Player;

    void Start() {
        Init();
    }

    Level CreateLevel(int index) {
        var entity = new GameObject($"Level-{index}");
        entity.transform.SetParent(transform);

        var level = entity.AddComponent<Level>()
        .Init(LevelDefs[index], BlockPrefab, ExitPrefab, CoinPrefab, Seed, Colors);

        return level;
    }

    Player CreatePlayer(Cell cell) {
        var player = Instantiate(PlayerPrefab, transform).Init(cell);
        player.name = "Player";
        return player;
    }

    void NextLevel() {
        Clear();

        CurrentLevel = CreateLevel(CurrentLevelIndex++);
        Player = CreatePlayer(new Cell(PRNG.Int(CurrentLevel.Def.Width), 0));
        GetComponent<PlayerMovementSystem>().Init(CurrentLevel, Player);
        GetComponent<PlayerGravitySystem>().Init(CurrentLevel, Player);
        GetComponent<PlayerPickCoinSystem>().Init(CurrentLevel, Player, PickUpCoin);
        GetComponent<ExitSystem>().Init(CurrentLevel, Player, NextLevel);

        SetupCamera(Player.transform, CurrentLevel.Def.Width);
    }

    void PickUpCoin() {
        Debug.Log("PickUpCoin");
        Purse.Increment();
    }

    void SetupCamera(Transform target, int size) {
        var camera = GetComponentInChildren<CinemachineVirtualCamera>();
        camera.Follow = camera.LookAt = target;
        camera.m_Lens.OrthographicSize = size;
    }

    void Clear() {
        CurrentLevel?.Clear();
        Player?.Clear();
    }

    void Init() {
        PRNG = new PRNG(Seed);
        NextLevel();
    }
}
