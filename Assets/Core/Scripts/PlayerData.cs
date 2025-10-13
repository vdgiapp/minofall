using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Minofall.Data
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance
        { get; private set; }

        //public static string SaveVersion = "0.1";
        public static string SaveFile = "player_data.json";
        public static string SavePath => Path.Combine(Application.persistentDataPath, SaveFile);

        private Wrapper _dataWrapper = new()
        {
            settings = new Settings().Reset(),
            highScores = new HighScores().Reset(),
            progress = new Progress().Reset()
        };

        public Settings Settings => _dataWrapper.settings;
        public Progress Progress => _dataWrapper.progress;
        public HighScores HighScores => _dataWrapper.highScores;

        private void Awake()
        {
            // Singleton init
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void SaveAll()
        {
            string json = JsonUtility.ToJson(_dataWrapper);
            File.WriteAllText(SavePath, json);
            Debug.Log("PlayerData: Saved all data");
        }

        public async UniTask SaveAllAsync()
        {
            string json = JsonUtility.ToJson(_dataWrapper);
            await File.WriteAllTextAsync(SavePath, json);
            Debug.Log("PlayerData: Saved all data");
        }

        public void LoadAll()
        {
            // First time in game, create new save file
            if (!File.Exists(SavePath))
            {
                CreateSaveFile();
                return;
            }

            // Found save file => save
            string json = File.ReadAllText(SavePath);
            _dataWrapper = JsonUtility.FromJson<Wrapper>(json);
            Debug.Log("PlayerData: Loaded all data");
        }

        public async UniTask LoadAllAsync()
        {
            if (!File.Exists(SavePath))
            {
                await CreateSaveFileAsync();
                return;
            }

            // Found save file => save
            string json = await File.ReadAllTextAsync(SavePath);
            _dataWrapper = JsonUtility.FromJson<Wrapper>(json);
            Debug.Log("PlayerData: Loaded all data");
        }

        private void CreateSaveFile()
        {
            Debug.Log("PlayerData: No save file found, created new save file");
            SaveAll();
        }

        private async UniTask CreateSaveFileAsync()
        {
            Debug.Log("PlayerData: No save file found, created new save file");
            await SaveAllAsync();
        }
    }

    [Serializable]
    public class Wrapper
    {
        public Settings settings;
        public Progress progress;
        public HighScores highScores;
    }

    [Serializable]
    public class Settings
    {
        public int controlType;

        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        public Settings Reset()
        {
            controlType = 0;
            masterVolume = 1.0f;
            musicVolume = 1.0f;
            sfxVolume = 1.0f;
            return this;
        }
    }

    [Serializable]
    public class HighScores
    {
        public int[] scores;
        public int lastScore;

        public int GetBestScore() => scores[0];
        public HighScores Reset()
        {
            scores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            lastScore = 0;
            return this;
        }

        public HighScores AddScore(int newScore)
        {
            scores[^1] = newScore;
            Array.Sort(scores, (a, b) => b.CompareTo(a));
            return this;
        }
    }

    [Serializable]
    public class Progress
    {
        // Last game save
        public bool isAvailable;
        public Cell[] cells;
        public Piece currentPiece;
        public Vector2Int ghostPosition;
        public List<int> nextQueue; // Piece generator
        public List<int> pieceBag; // Piece generator
        public int holdingPiece;
        public bool heldThisTurn;
        public int highestRow;
        public float dropTimer;
        public int level;
        public int score;
        public int lines;
        
        public Progress Reset()
        {
            isAvailable = false;
            cells = new Cell[BoardController.BOARD_SIZE.x * BoardController.BOARD_SIZE.y];
            for (int i = 0; i < cells.Length; i++) cells[i] = new Cell();
            currentPiece = new Piece();
            ghostPosition = Vector2Int.zero;
            nextQueue = new List<int>();
            pieceBag = new List<int>();
            holdingPiece = -1;
            heldThisTurn = false;
            highestRow = 0;
            dropTimer = 0.0f;
            level = 1;
            score = 0;
            lines = 0;
            return this;
        }
    }
}