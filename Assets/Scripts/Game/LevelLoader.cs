using UnityEngine;
using System.Data;
using System;
using Mono.Data.SqliteClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private ActiveDotsRenderer topDotsRenderer;
    private GameObject levelPrefab;
    private string tutorial;

    public static Level currentLevel;
    public static bool isNextLevelUnlocked;
    public static int currentLevelUnlocked;
    public static int maxLevel;
    
    public GameObject parameters;
    public Text levelText;
    public Image nextLevel;
    public Button nextLevelBtn;
    public Image previousLevel;
    public GameObject loadingPanel;
    public GameObject tutorialPanel;
    public Text tutorialDescription;
    public GameObject mode;
    public Text moveLeft;

	void Awake()
    {
        
        topDotsRenderer = GameObject.Find("TopGrid").GetComponent<ActiveDotsRenderer>();
        levelPrefab = (GameObject)Resources.Load("Prefabs/LevelChooserScene");
    }
	
    void Start()
    {
        ParameterReader.Instance.LockOrientation(false);
        InitLevel();
    }

    void InitLevel()
    {
        Debug.Log("Ceci est un log de test");
        levelText.text = "#" + currentLevel.level;
        moveLeft.text = currentLevel.moveOneStar.ToString();

        if(currentLevel.mode == 1)
            mode.SetActive(true);

        tutorial = null;
        string tutorialText = "";
        switch (currentLevel.level)
        {
            case 1:
                tutorial = "base_rule";
                tutorialText = "base_rule_1,base_rule_2,base_rule_3,good_luck";
                break;
            case 20:
                tutorial = "rule_multi_color";
                tutorialText = "rule_multi_color";
                break;
            case 31:
                tutorial = "rule_teleport";
                tutorialText = "rule_teleport";
                break;
            case 46:
                tutorial = "rule_double";
                tutorialText = "rule_double";
                break;  
        }

        if (tutorial != null)
        {
            tutorialDescription.text = LocalizedText.Translate(tutorialText);
            tutorialPanel.SetActive(true);
        }

        if (currentLevel.tutorial > 0)
        {
            using (var connection = new SqliteConnection(StartScreen.GetDatabasePath()))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT starting_point, orientation FROM tutorial WHERE id = @id";

                    var paramLevel = command.CreateParameter();
                    paramLevel.ParameterName = "@id";
                    paramLevel.Value = currentLevel.tutorial;
                    paramLevel.DbType = DbType.Int32;
                    command.Parameters.Add(paramLevel);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topDotsRenderer.Tutorial.Add(reader.GetInt32(0), reader.GetInt32(1));
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
        }

        topDotsRenderer.MirroredRenderer.Init(false, currentLevel.bottomLevelText);
        topDotsRenderer.Init(true, currentLevel.topLevelText, topDotsRenderer.MirroredRenderer.dots);

        if (!isNextLevelUnlocked)
            nextLevel.enabled = false;
            

        if (currentLevel.level == 1)
            previousLevel.enabled = false;  
    }

    public void LoadLevel(bool next)
    {
        Audio.Instance.PlaySound(Sound.Button);
        loadingPanel.SetActive(true);

        using (var connection = new SqliteConnection(StartScreen.GetDatabasePath()))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT l.id, l.topDots, l.bottomDots, l.unlocked, l.world, n.unlocked As nextUnlocked, l.moveOneStar, l.moveTwoStars, l.moveThreeStars, l.tutorial, l.row_count, l.column_count, l.mode FROM level l LEFT JOIN level n On n.id = l.id+1 WHERE l.id = @level";
                var paramLevel = command.CreateParameter();
                paramLevel.ParameterName = "@level";
                
                if(next)
                    paramLevel.Value = currentLevel.level + 1;
                else
                    paramLevel.Value = currentLevel.level - 1;

                paramLevel.DbType = DbType.Int32;
                command.Parameters.Add(paramLevel);
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SetCurrentLevel(new Level
                        {
                            level = reader.GetInt32(0),
                            topLevelText = reader.GetString(1),
                            bottomLevelText = reader.GetString(2),
                            unlocked = reader.GetInt32(3),
                            world = reader.GetInt32(4),
                            moveOneStar = reader.GetInt32(6),
                            moveTwoStars = reader.GetInt32(7),
                            moveThreeStars = reader.GetInt32(8),
                            tutorial = reader.GetInt32(9),
                            rowCount = reader.GetInt32(10),
                            columnCount = reader.GetInt32(11),
                            mode = reader.GetInt32(12)
                        }, reader.GetInt32(5) > -1);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        SceneManager.LoadSceneAsync("Game");
    }

    public void GoToLevelSelection()
    {
        Audio.Instance.PlaySound(Sound.Button);
        loadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync("Menu");
    }

    public void GoToHome()
    {
        Audio.Instance.PlaySound(Sound.Button);
        loadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync("Start");
    }

    public static void SetCurrentLevel(Level _level, bool _isNextLevelUnlocked){
        currentLevel = _level;
        currentLevelUnlocked = 0;
        isNextLevelUnlocked = _isNextLevelUnlocked;
    }

    public void ShowParameters(){
        Audio.Instance.PlaySound(Sound.Button);
        parameters.SetActive(true);
    }

    public void CloseTutorial()
    {
        Audio.Instance.PlaySound(Sound.Button);
        tutorialPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (parameters.activeSelf)
            {
                Audio.Instance.PlaySound(Sound.Button);
                parameters.SetActive(false);
            }
            else
                GoToLevelSelection();
        }     
    }
}
