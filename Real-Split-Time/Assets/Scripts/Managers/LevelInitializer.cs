using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    public static LevelInitializer Instance { get; private set; }

    public GameObject[] levelPrefabs;
    public float[] levelTimes;

    private int currentLevelIndex;
    private GameObject currentLevelInstance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;

        // Destroy all clones by tag before loading new level
        GameObject[] clones = GameObject.FindGameObjectsWithTag("Clone");
        for (int i = 0; i < clones.Length; i++)
            DestroyImmediate(clones[i]);

        // DestroyImmediate so the old SpawnPoint tag is gone before we search
        if (currentLevelInstance != null)
            DestroyImmediate(currentLevelInstance);

        currentLevelInstance = Instantiate(levelPrefabs[index]);

        // Set level time on TimeManager
        if (levelTimes != null && index < levelTimes.Length)
            TimeManager.Instance.levelTime = levelTimes[index];

        // Now safe to use global tag search since old level is already destroyed
        GameObject spawnObj = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawnObj != null)
            TimeManager.Instance.spawnPoint = spawnObj.transform;
        else
            Debug.LogWarning("LevelInitializer: No SpawnPoint found in level " + index);

        Debug.Log("LevelInitializer: Loaded level " + index);
        TimeManager.Instance.Initialize();
    }

    public void NextLevel()
    {
        Debug.Log("NextLevel called. Current: " + currentLevelIndex + ", Total: " + levelPrefabs.Length);
        if (currentLevelIndex + 1 < levelPrefabs.Length)
            LoadLevel(currentLevelIndex + 1);
        else
            LoadLevel(0);
    }

    public bool IsLastLevel()
    {
        return currentLevelIndex + 1 >= levelPrefabs.Length;
    }
}
