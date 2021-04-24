using Damrem.Procedural;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int Seed = 0;
    public Block BlockPrefab;
    public Color[] Colors;
    public LevelDef[] LevelDefs;

    PRNG PRNG;
    int CurrentLevelIndex = 0;
    Level CurrentLevel;

    void Start() {
        Init();
    }

    Level CreateLevel(int index) {
        var entity = new GameObject($"Level-{index}");
        entity.transform.SetParent(transform);
        var level = entity.AddComponent<Level>().Init(index, LevelDefs[index], BlockPrefab, Seed, Colors);
        level.OnComplete += NextLevel;
        return level;
    }

    void NextLevel() {
        if (CurrentLevel) Destroy(CurrentLevel.gameObject);
        CurrentLevel = CreateLevel(CurrentLevelIndex++);
        var player = GetComponentInChildren<Player>().Init(new Cell(PRNG.Int(CurrentLevel.Def.Width), 0));
        GetComponentInChildren<PlayerMovementSystem>().Init(CurrentLevel, player);
        GetComponentInChildren<PlayerGravitySystem>().Init(CurrentLevel, player);
    }

    void Init() {
        PRNG = new PRNG(Seed);
        NextLevel();
    }
}
