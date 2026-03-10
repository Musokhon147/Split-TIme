using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public static event Action OnReset;

    [Header("References")]
    public PlayerController player;
    public Transform spawnPoint;
    public GameObject clonePrefab;
    public Sprite cloneSprite;

    [Header("Settings")]
    public int maxClones = 5;
    [HideInInspector]
    public float levelTime;

    private List<List<RecordedFrame>> savedRecordings = new List<List<RecordedFrame>>();
    private List<GameObject> activeClones = new List<GameObject>();
    private Vector3 spawnPosition;
    public bool levelComplete { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Initialize()
    {
        levelComplete = false;
        spawnPosition = spawnPoint != null ? spawnPoint.position : player.transform.position;

        savedRecordings.Clear();
        DestroyAllClones();

        player.ResetToSpawn(spawnPosition);
        player.StartRecording();
        player.SetControl(true);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCloneCount(0, maxClones);
            UIManager.Instance.HideLevelComplete();
        }

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.StartTimer(levelTime);
    }

    public void SplitTime()
    {
        if (levelComplete) return;
        if (savedRecordings.Count >= maxClones) return;

        List<RecordedFrame> recording = player.StopRecording();

        if (recording.Count < 2)
        {
            player.StartRecording();
            return;
        }

        savedRecordings.Add(recording);

        OnReset?.Invoke();
        DestroyAllClones();
        SpawnAllClones();

        player.ResetToSpawn(spawnPosition);
        player.StartRecording();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCloneCount(savedRecordings.Count, maxClones);
        }
    }

    public void ResetCurrentAttempt()
    {
        if (levelComplete) return;

        player.StopRecording();

        OnReset?.Invoke();
        DestroyAllClones();
        SpawnAllClones();

        player.ResetToSpawn(spawnPosition);
        player.StartRecording();
    }

    public void FullReset()
    {
        levelComplete = false;

        savedRecordings.Clear();
        DestroyAllClones();

        OnReset?.Invoke();

        player.ResetToSpawn(spawnPosition);
        player.StartRecording();
        player.SetControl(true);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCloneCount(0, maxClones);
            UIManager.Instance.HideLevelComplete();
        }

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetTimer();
    }

    public void OnLevelComplete()
    {
        if (levelComplete) return;

        Debug.Log("OnLevelComplete called!");
        levelComplete = true;
        player.SetControl(false);
        player.StopRecording();

        int levelScore = 0;
        int timeBonus = 0;
        if (ScoreManager.Instance != null)
        {
            timeBonus = ScoreManager.Instance.GetTimeBonus();
            levelScore = ScoreManager.Instance.CompleteLevel();
        }

        bool isLastLevel = LevelInitializer.Instance != null && LevelInitializer.Instance.IsLastLevel();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowLevelComplete(savedRecordings.Count, levelScore, timeBonus, isLastLevel);
        }
    }

    void SpawnAllClones()
    {
        for (int i = 0; i < savedRecordings.Count; i++)
        {
            GameObject cloneObj = Instantiate(clonePrefab);
            cloneObj.name = "Clone_" + i;
            cloneObj.tag = "Clone";

            CloneController cc = cloneObj.GetComponent<CloneController>();
            cc.Init(savedRecordings[i], cloneSprite);

            activeClones.Add(cloneObj);
        }
    }

    void DestroyAllClones()
    {
        for (int i = 0; i < activeClones.Count; i++)
        {
            if (activeClones[i] != null)
            {
                DestroyImmediate(activeClones[i]);
            }
        }
        activeClones.Clear();
    }
}
