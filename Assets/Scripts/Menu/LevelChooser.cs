using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class LevelChooser : MonoBehaviour
{
    public GameObject canvas;
    public GameObject parametersPanel;
    private List<Level> levels;
    private GameObject gamePrefab;
    private GameObject levelPrefab;
    private GameObject levelDotPrefab;

    void Awake()
    {
        gamePrefab = (GameObject)Resources.Load("Prefabs/GameScene");
        levelPrefab = (GameObject) Resources.Load("Prefabs/Level");
        levelDotPrefab = (GameObject)Resources.Load("Prefabs/LevelDot");
    }
	
    void Start()
    {
        ParameterReader.Instance.LockOrientation(true);
        int currentPlayerWorld = 0;
        if (LevelLoader.currentLevel != null)
            currentPlayerWorld = LevelLoader.currentLevel.world;

        levels = new List<Level>();
        Sprite unlockedSprite = Resources.Load<Sprite>("Images/Game/dot_top_normal");
        using (var connection = new SqliteConnection(StartScreen.GetDatabasePath()))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, topDots, bottomDots, unlocked, world, moveOneStar, moveTwoStars, moveThreeStars, tutorial, row_count, column_count, mode FROM level order by world";
 
                using (IDataReader reader = command.ExecuteReader())
                {
                    Vector3 startPosition = new Vector3(0, 0, 0);
                    int currentWorld = 0;
                    List<Transform> worlds = new List<Transform>();
                    while (reader.Read())
                    {
                        if(currentWorld < reader.GetInt32(4))
                        {
                            Transform world = Instantiate(levelPrefab.transform, startPosition, Quaternion.identity);
                            world.Find("LevelNumber").GetComponent<Text>().text = LocalizedText.Translate("world", reader.GetInt32(4).ToString());
                            world.SetParent(this.transform, false);
                            worlds.Add(world);
                        }

                        Level level = new Level
                        {
                            level = reader.GetInt32(0),
                            topLevelText = reader.GetString(1),
                            bottomLevelText = reader.GetString(2),
                            unlocked = reader.GetInt32(3),
                            world = reader.GetInt32(4),
                            moveOneStar = reader.GetInt32(5),
                            moveTwoStars = reader.GetInt32(6),
                            moveThreeStars = reader.GetInt32(7),
                            tutorial = reader.GetInt32(8),
                            rowCount = reader.GetInt32(9),
                            columnCount = reader.GetInt32(10),
                            mode = reader.GetInt32(11)
                        };

                        levels.Add(level);
                        
                        Transform levelDot = Instantiate(levelDotPrefab.transform, startPosition, Quaternion.identity);
                        levelDot.SetParent(worlds.Last().Find("Wrapper/GridLevel").transform, false);
                        currentWorld = level.world;
     
                        levelDot.Find("LevelText").GetComponent<Text>().text = level.level.ToString();
                        levelDot.GetComponent<Dot>().Id = level.level;

                        if (level.unlocked != -1)
                        {
                            if(LevelLoader.currentLevel == null)
                                currentPlayerWorld = level.world;

                            levelDot.GetComponent<Image>().sprite = unlockedSprite;
                            levelDot.GetComponent<Button>().onClick.AddListener(() => LoadLevel(level));

                            Image starImage = levelDot.Find("Stars").GetComponent<Image>();
                           
                            if(level.unlocked == 3){
                                starImage.sprite = Resources.Load<Sprite>("Images/3_stars");
                                starImage.gameObject.SetActive(true);
                            }
                            else if(level.unlocked == 2){
                                starImage.sprite = Resources.Load<Sprite>("Images/2_stars");
                                starImage.gameObject.SetActive(true);
                            }
                            else if(level.unlocked == 1)
                            {
                                starImage.sprite = Resources.Load<Sprite>("Images/1_stars");
                                starImage.gameObject.SetActive(true);
                            }
                        }

                        LevelLoader.maxLevel = level.level;
                    }

                    reader.Close();
                }
            }
            connection.Close();
        }
        ScrollSnapRect scroll = canvas.GetComponent<ScrollSnapRect>();
        scroll.startingPage = currentPlayerWorld-1;
        scroll.Init();
    }

    public void LoadLevel(Level level)
    {
        bool unlocked = false;
        for(int i = 0; i < levels.Count && !unlocked; i++)
        {
            if (levels[i].level == level.level + 1 && levels[i].unlocked > -1)
                unlocked = true;
        }

        Audio.Instance.PlaySound(Sound.Button);
        LevelLoader.SetCurrentLevel(level, unlocked);
        SceneManager.LoadSceneAsync("Game");
    }

    public void LoadLevel(string level)
    {
        Audio.Instance.PlaySound(Sound.Button);
        SceneManager.LoadSceneAsync(level);
    }

    public void ShowParameters()
    {
        Audio.Instance.PlaySound(Sound.Button);
        parametersPanel.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (parametersPanel.activeSelf)
            {
                Audio.Instance.PlaySound(Sound.Button);
                parametersPanel.SetActive(false);
            }
            else
                LoadLevel("Start");
    }
}
