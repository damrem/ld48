using Cinemachine;
using Damrem.Procedural;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ExitSystem))]
[RequireComponent(typeof(CoinPickSystem))]
[RequireComponent(typeof(GemPickSystem))]
[RequireComponent(typeof(PlayerMovementSystem))]
[RequireComponent(typeof(PlayerGravitySystem))]
public class GameManager : MonoBehaviour, IPointerClickHandler {
    public int Seed = 0;
    public Player PlayerPrefab;
    public Block BlockPrefab;
    public Exit ExitPrefab;
    public Coin CoinPrefab;
    public Gem GemPrefab;
    public Purse Purse;
    public EnergyBar EnergyBar;
    public LevelTitle LevelTitle;
    public int EnergyRefill = 10;
    public Color[] Colors;
    public LevelDef[] LevelDefs;

    PRNG PRNG;
    int CurrentLevelIndex = 0;
    Level CurrentLevel;
    Player Player;

    void Start() {
        Init();
    }

    void Init() {
        PRNG = new PRNG(Seed);
        InitHUD();
        PreNextLevel();
    }

    void Clear() {
        CurrentLevel?.Clear();
        Player?.Clear();
    }

    void InitHUD() {
        Purse.Init();
        EnergyBar.Init(100);
    }

    Level CreateLevel(int index) {
        var entity = new GameObject($"Level-{index}");
        entity.transform.SetParent(transform);

        var level = entity.AddComponent<Level>()
        .Init(LevelDefs[index], BlockPrefab, ExitPrefab, CoinPrefab, GemPrefab, Seed, Colors);

        return level;
    }

    Player CreatePlayer(Cell cell) {
        var player = Instantiate(PlayerPrefab, transform).Init(cell);
        player.name = "Player";
        return player;
    }

    void PreNextLevel() {

        Clear();

        CurrentLevelIndex++;
        LevelTitle.SetLevelNumber(CurrentLevelIndex);
        LevelTitle.Show();
    }

    public void OnPointerClick(PointerEventData data) {
        if (!LevelTitle.enabled) return;

        LevelTitle.Hide();
        NextLevel();
    }

    void NextLevel() {
        CurrentLevel = CreateLevel(CurrentLevelIndex);
        Player = CreatePlayer(new Cell(PRNG.Int(CurrentLevel.Def.Width), 0));
        GetComponent<PlayerMovementSystem>().Init(CurrentLevel, Player, SpendEnergy);
        GetComponent<PlayerGravitySystem>().Init(CurrentLevel, Player);
        GetComponent<CoinPickSystem>().Init(CurrentLevel, Player, PickUpCoin);
        GetComponent<GemPickSystem>().Init(CurrentLevel, Player, PickUpGem);
        GetComponent<ExitSystem>().Init(CurrentLevel, Player, PreNextLevel);

        SetupCamera(Player.transform, CurrentLevel.Def.Width);
    }

    void PickUpCoin() {
        Purse.Increment();
    }

    void PickUpGem() {
        EnergyBar.Increment(EnergyRefill);
    }

    void SpendEnergy(EnergySpendingType spendingType) {
        int value;
        switch (spendingType) {
            case EnergySpendingType.Dig: value = 5; break;
            case EnergySpendingType.Walk: value = 1; break;
            default: value = 1; break;
        }
        EnergyBar.Decrement(value);
    }

    void SetupCamera(Transform target, int size) {
        var camera = GetComponentInChildren<CinemachineVirtualCamera>();
        camera.Follow = camera.LookAt = target;
        camera.m_Lens.OrthographicSize = size;
    }
}
