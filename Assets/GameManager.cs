using Cinemachine;
using Damrem.Procedural;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int Seed = 0;
    public Player PlayerPrefab;
    public Block BlockPrefab;
    public Exit ExitPrefab;
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
        var level = entity.AddComponent<Level>().Init(index, LevelDefs[index], BlockPrefab, ExitPrefab, Seed, Colors);
        return level;
    }

    Player CreatePlayer(Cell cell) {
        var player = Instantiate(PlayerPrefab).Init(cell);
        player.transform.SetParent(transform);
        return player;
    }

    void NextLevel() {
        if (CurrentLevel) CurrentLevel.Clear();

        CurrentLevel = CreateLevel(CurrentLevelIndex++);
        Player = CreatePlayer(new Cell(PRNG.Int(CurrentLevel.Def.Width), 0));
        GetComponentInChildren<PlayerMovementSystem>().Init(CurrentLevel, Player);
        GetComponentInChildren<PlayerGravitySystem>().Init(CurrentLevel, Player);
        GetComponentInChildren<ExitSystem>().Init(CurrentLevel, Player, NextLevel);

        CameraFollow(Player.transform);
    }

    void CameraFollow(Transform target) {
        var camera = GetComponentInChildren<CinemachineVirtualCamera>();
        camera.Follow = camera.LookAt = target;
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
