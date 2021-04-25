using Damrem.Procedural;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ExitSystem))]
[RequireComponent(typeof(PickerSystem))]
[RequireComponent(typeof(PlayerMovementSystem))]
[RequireComponent(typeof(PlayerGravitySystem))]
public class GameManager : MonoBehaviour/* , IPointerClickHandler */ {
    public int Seed = 0;
    public Player PlayerPrefab;
    public Block BlockPrefab;
    public Exit ExitPrefab;
    public Coin CoinPrefab;
    public Gem GemPrefab;
    public Purse Purse;
    public EnergyBar EnergyBar;
    public LevelTitle LevelTitle;
    public Overlay WelcomeScreen;
    public GameOverScreen GameOverScreen;
    public Canvas HUD;
    public int CoinGain = 10;
    public int MaxEnergy = 5;
    public int EnergyRefill = 10;
    public int EnergyWalkCost = 1;
    public int EnergyDigCost = 3;
    [Range(0, 1)] public float CoinDensity = .25f;
    [Range(0, 1)] public float GemDensity = .125f;
    public Color[] Colors;
    // public LevelDef[] LevelDefs;

    PRNG PRNG;
    int CurrentLevelIndex = 0;
    Level CurrentLevel;
    Player Player;

    void Start() {
        Init();
    }

    void Init() {
        WelcomeScreen.Init(InitGame);
        GotoWelcomeScreen();

        LevelTitle.GetComponent<Overlay>().Init(NextLevel);
    }

    void GotoWelcomeScreen() {
        Debug.Log("GotoWelcomeScreen");
        WelcomeScreen.Show();
        CurrentLevelIndex = 0;
    }

    void InitGame() {
        PRNG = new PRNG();
        InitHUD();
        PreNextLevel();
    }

    void Clear() {
        GetComponent<PlayerMovementSystem>().enabled = false;

        CurrentLevel?.Clear();
        CurrentLevel = null;

        Player?.Clear();
        Player = null;
    }

    void InitHUD() {
        HUD.gameObject.SetActive(true);
        Purse.Init();
        EnergyBar.Init(MaxEnergy, GotoGameOverScreen);
    }

    Level CreateLevel(int index) {
        var entity = new GameObject($"Level-{index}");
        entity.transform.SetParent(transform);

        var level = entity.AddComponent<Level>()
        // .Init(LevelDefs[index], BlockPrefab, ExitPrefab, CoinPrefab, GemPrefab, Seed, Colors);
        .Init(LevelDef.CreateLevelDef(index, CoinDensity, GemDensity), BlockPrefab, ExitPrefab, CoinPrefab, GemPrefab, Seed, Colors);

        level.OnGroupDestroyed += blockCount => {
            Purse.Increment(blockCount * blockCount);
        };

        return level;
    }

    Player CreatePlayer(Cell cell) {
        var player = Instantiate(PlayerPrefab, transform).Init(cell);
        player.name = "Player";
        return player;
    }

    void PreNextLevel() {
        Clear();

        HUD.gameObject.SetActive(true);
        LevelTitle.SetLevelIndex(CurrentLevelIndex);
        LevelTitle.GetComponent<Overlay>().Show();
    }

    // public void OnPointerClick(PointerEventData data) {
    //     if (!LevelTitle.enabled) return;

    //     LevelTitle.GetComponent<Overlay>().Hide();
    //     NextLevel();
    // }

    void NextLevel() {
        GetComponent<PlayerMovementSystem>().enabled = true;

        CurrentLevel = CreateLevel(CurrentLevelIndex);

        var playerInitialX = PRNG.Int(CurrentLevel.Def.Width);
        Player = CreatePlayer(new Cell(playerInitialX, 0));
        CurrentLevel.AddBlockUnderPlayer(playerInitialX);

        var picker = Player.GetComponent<Picker>();
        picker.OnTouchedPickable += PickItem;

        GetComponent<PlayerMovementSystem>().Init(CurrentLevel, Player, SpendEnergy);
        GetComponent<PlayerGravitySystem>().Init(CurrentLevel, Player);
        GetComponent<PickerSystem>().Init(CurrentLevel, picker);
        GetComponent<ExitSystem>().Init(CurrentLevel, Player, PreNextLevel);

        SetupCamera(Player.transform, CurrentLevel);
        CurrentLevelIndex++;
    }

    void PickItem(Pickable pickable) {
        switch (pickable.Type) {
            case PickableType.Coin: PickUpCoin(); break;
            case PickableType.Gem: PickUpGem(); break;
            default: break;
        }
    }

    void PickUpCoin() {
        Purse.Increment(CoinGain);
    }

    void PickUpGem() {
        EnergyBar.Increment(EnergyRefill);
    }

    void SpendEnergy(EnergySpendingType spendingType) {
        int value;
        switch (spendingType) {
            case EnergySpendingType.Dig: value = EnergyDigCost; break;
            case EnergySpendingType.Walk: value = EnergyWalkCost; break;
            default: value = 1; break;
        }
        EnergyBar.Decrement(value);
    }

    void GotoGameOverScreen() {
        Debug.Log("GotoGameOverScreen");
        Clear();
        HUD.gameObject.SetActive(false);
        GameOverScreen.Init(CurrentLevelIndex, Purse.Value, GotoWelcomeScreen);
        GameOverScreen.Overlay.Show();
    }

    void SetupCamera(Transform target, Level level) {
        Debug.Log("SetupCamera " + target);
        Camera.main.GetComponent<CameraMan>().Init(target, level);
        // var camera = GetComponentInChildren<CinemachineVirtualCamera>();
        // camera.Follow = camera.LookAt = target;
        // camera.m_Lens.OrthographicSize = size;
    }

    // void Update() {
    //     if (LevelTitle.IsVisible && Input.anyKey) NextLevel();
    // }
}
