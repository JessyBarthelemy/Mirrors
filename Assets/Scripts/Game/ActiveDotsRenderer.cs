using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.Advertisements;

public class ActiveDotsRenderer : DotsRenderer
{
    public MirroredDotsRenderer MirroredRenderer;
    // starting point -> orientation
    public Dictionary<int, int> Tutorial { get; set; }
    private Animator winAnimator;
    private Animator endAnimator;
    private Animator moveLeftAnimator;
    public GameObject winPanel;
    public GameObject endGamePanel;
    public Image winStarImage;
    public Image endStarImage;
    public GameObject tutorialPanel;
    public GameObject hintPanel;
    public Text hintText;
    public Text noButtonText;
    public Text yesButtonText;
    private Animator hintAnimator;
    public Image stars;
    public Text moveLeftText;
    private int moveLeft;
    private float teleportWidth;
    private int lastVibrationId;
    private bool adsShown;

    void Awake()
    {
        Tutorial = new Dictionary<int, int>();
        winAnimator = winPanel.GetComponent<Animator>();   
        endAnimator = endGamePanel.GetComponent<Animator>();
        hintAnimator = hintPanel.GetComponent<Animator>();
        MirroredRenderer = GameObject.Find("BottomGrid").GetComponent<MirroredDotsRenderer>();
        moveLeft = LevelLoader.currentLevel.moveOneStar;
        moveLeftAnimator = moveLeftText.GetComponent<Animator>();
    }

    void Update()
    {
        if (isDrawing && (!IsWon || !MirroredRenderer.IsWon))
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 point = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), 1);
                point.z = 100;
                lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, point);
                Collider2D hit = Physics2D.OverlapPoint(lineRenderers.Last().GetPosition(lineRenderers.Last().positionCount - 1));

                if (hit != null && hit.gameObject != null)
                {
                    OnDotCollision(hit.transform.gameObject.GetComponent<Dot>());
                }
                else
                {
                    lastVibrationId = 0;
                }
            }
            else
            {
                Audio.Instance.PlaySound(Sound.ExitDot);
                isDrawing = false;               
                lineRenderers.Last().positionCount--;
            }
        }
    }
   
    protected void OnDotCollision(Dot dot)
    {
        if(dot.State == Dot.STATE_START)
            return;

        if (CanAddDotPoint(dot) && moveLeft > 0 && MirroredRenderer.OnDotCollision(dot.Order))
        {
            //avoid vibration
            lastVibrationId = dot.Id;
            AddDotPoint(dot, false, true);
            if (IsWon && MirroredRenderer.IsWon)
            {
                Audio.Instance.PlaySound(Sound.Won);

                GameObject panel = null;
                Animator animator = null;
                Image image = null;
                if(LevelLoader.maxLevel == LevelLoader.currentLevel.level){
                    panel = endGamePanel;
                    animator = endAnimator;
                    image = endStarImage;
                }else{
                    panel = winPanel;
                    animator = winAnimator;
                    image = winStarImage;
                }

                panel.SetActive(true);
                animator.SetBool("ShowWin", true);
                if (!adsShown && moveLeft >= (LevelLoader.currentLevel.moveOneStar - LevelLoader.currentLevel.moveThreeStars))
                {
                    LevelLoader.currentLevelUnlocked = 3;
                    image.sprite = Resources.Load<Sprite>("Images/3_stars");
                }
                else if (!adsShown &&  moveLeft >= (LevelLoader.currentLevel.moveOneStar - LevelLoader.currentLevel.moveTwoStars))
                {
                    LevelLoader.currentLevelUnlocked = 2;
                    image.sprite = Resources.Load<Sprite>("Images/2_stars");
                }
                else
                {
                    LevelLoader.currentLevelUnlocked = 1;
                    image.sprite = Resources.Load<Sprite>("Images/1_stars");
                }

                using (var connection = new SqliteConnection(StartScreen.GetDatabasePath()))
                {
                    connection.Open();
                    if (LevelLoader.currentLevelUnlocked > LevelLoader.currentLevel.unlocked)
                        UpdateLevel(connection, LevelLoader.currentLevel.level, LevelLoader.currentLevelUnlocked);

                    UpdateLevel(connection, LevelLoader.currentLevel.level + 1, 0);
                    connection.Close();
                }
            }
        }
        else if (lastVibrationId != dot.Id && current.Last() != dot.Id)
        {
            lastVibrationId = dot.Id;
            Audio.Instance.PlaySound(Sound.Error);
            if(ParameterReader.Instance.IsVibrationActivatedParam)
                Vibration.Vibrate(200);
            
        }
    }

    public override void OnDotClick(Dot dot)
    {
        if ((IsWon && MirroredRenderer.IsWon) || current.Last() != dot.Id)
            return;

        Audio.Instance.PlaySound(Sound.EnterDot);
        //avoid vibration
        lastVibrationId = dot.Id;
        
        lineRenderers.Last().positionCount++;
        RectTransform rectTransform = dot.GetComponent<RectTransform>();
        Vector3 point = rectTransform.TransformPoint(rectTransform.rect.center);
        point.z = 100;
        lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount-1, point);

        isDrawing = true;
       
        MirroredRenderer.OnDotClick();
    }

    public override void ResetLevel()
    {
        Audio.Instance.PlaySound(Sound.Button);

        if(winPanel.activeSelf)
            winAnimator.SetBool("ShowWin", false);

        if(endGamePanel.activeSelf)
            endAnimator.SetBool("ShowWin", false);     
        
        isDrawing = false;
        base.ResetLevel();
        MirroredRenderer.ResetLevel();
        moveLeft = LevelLoader.currentLevel.moveOneStar;
        UpdateMoveLeft();
    }

    private void UpdateMoveLeft()
    {
        moveLeftAnimator.SetBool("NoMoreMove", moveLeft == 0);
        moveLeftText.text = moveLeft.ToString();

        if (!adsShown && moveLeft >= (LevelLoader.currentLevel.moveOneStar - LevelLoader.currentLevel.moveThreeStars))
            stars.sprite = Resources.Load<Sprite>("Images/3_stars");
        else if (!adsShown && moveLeft >= (LevelLoader.currentLevel.moveOneStar - LevelLoader.currentLevel.moveTwoStars))
            stars.sprite = Resources.Load<Sprite>("Images/2_stars");
        else
            stars.sprite = Resources.Load<Sprite>("Images/1_stars");
    }

    public override void AddDotPoint(Dot addedDot, bool isMirrored, bool checkTeleport)
    {
        Audio.Instance.PlaySound(Sound.EnterDot);
        if(!checkTeleport || addedDot.State != Dot.STATE_TELEPORT)
            moveLeft--;
        UpdateMoveLeft();
        base.AddDotPoint(addedDot, isMirrored, checkTeleport);
        ShowTutorial(addedDot);
    }

    private void UpdateLevel(SqliteConnection connection, int level, int unlocked){
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE level SET unlocked = @unlocked WHERE id = @level AND unlocked < @unlocked ";
            //ne pas debloquer si level deja debloque
            var param = command.CreateParameter();
            param.ParameterName = "@unlocked";
            param.Value = unlocked;
            param.DbType = DbType.Int32;
            command.Parameters.Add(param);

            var paramLevel = command.CreateParameter();
            paramLevel.ParameterName = "@level";
            paramLevel.Value = level;
            paramLevel.DbType = DbType.Int32;
            command.Parameters.Add(paramLevel);

            command.ExecuteNonQuery();
        }
    }

    protected override bool ShouldDeleteLineRenderer()
    {
        return lineRenderers.Count > 1 && lineRenderers.Last().positionCount == 1;
    }

    protected override void SetStartPoint(){
        base.SetStartPoint();
        ShowTutorial(dots[startIndex]);
    }

    void ShowTutorial(Dot dot){
        if(Tutorial != null && Tutorial.ContainsKey(dot.Id)){
            if(teleportWidth == 0)
                teleportWidth = tutorialPanel.GetComponent<RectTransform>().rect.width;

            RectTransform rectTransform = dot.GetComponent<RectTransform>();

            float x = rectTransform.rect.center.x;
            float y = rectTransform.rect.center.y;

            switch (Tutorial[dot.Id])
            {
                //North
                case 1:
                    y += teleportWidth;
                    tutorialPanel.transform.rotation =  Quaternion.Euler(0, 0, 180);
                    break;
                //Est
                case 2:
                    x += teleportWidth;
                    tutorialPanel.transform.rotation =  Quaternion.Euler(0, 0, 90);
                    break;
                //South
                case 3:
                    y -= teleportWidth;
                    tutorialPanel.transform.rotation =  Quaternion.Euler(0, 0, 0);
                    break;
                //West
                case 4:
                    x -= teleportWidth;
                    tutorialPanel.transform.rotation =  Quaternion.Euler(0, 0, 270);
                    break;
                //North Est
                case 5:
                    y += teleportWidth;
                    x += teleportWidth;
                    tutorialPanel.transform.rotation = Quaternion.Euler(0, 0, 135);
                    break;
                //South Est
                case 6:
                    x += teleportWidth;
                    y -= teleportWidth;
                    tutorialPanel.transform.rotation = Quaternion.Euler(0, 0, 45);
                    break;
                //South West
                case 7:
                    y -= teleportWidth;
                    x -= teleportWidth;
                    tutorialPanel.transform.rotation = Quaternion.Euler(0, 0, -45);
                    break;
                //North West
                case 8:
                    x -= teleportWidth;
                    y += teleportWidth;
                    tutorialPanel.transform.rotation = Quaternion.Euler(0, 0, -135);
                    break;
            }

            Vector3 pos = rectTransform.TransformPoint(x, y, 0);
            tutorialPanel.transform.position = pos;

            Vector3 localPosition = tutorialPanel.transform.localPosition;
            localPosition.z = -21;
            tutorialPanel.transform.localPosition = localPosition;

            tutorialPanel.SetActive(true);
        }
    }

    public void UndoMove()
    {
        int last = current.Last();
        if (dots[last].State != Dot.STATE_START)
        {
            Audio.Instance.PlaySound(Sound.ExitDot);
            RemoveLastDot();
            moveLeft++;
            UpdateMoveLeft();
            MirroredRenderer.RemoveLastDot();
            lastVibrationId = last;
        }
    }

    public void ShowHintPanel()
    {
        #if UNITY_ANDROID || UNITY_EDITOR
        Audio.Instance.PlaySound(Sound.Button);
        hintText.text = LocalizedText.Translate("hint_text");
        noButtonText.transform.parent.gameObject.SetActive(true);
        noButtonText.text = LocalizedText.Translate("no");
        yesButtonText.text = LocalizedText.Translate("yes");

        Button yesButton = yesButtonText.transform.parent.GetComponent<Button>();
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => ShowRewardedAd());

        hintPanel.SetActive(true);
        hintAnimator.SetBool("ShowWin", true);
        #endif
    }
    
    #if UNITY_ANDROID || UNITY_EDITOR
    public void ShowRewardedAd()
    {
        Audio.Instance.PlaySound(Sound.Button);
        CloseHintPanel();
        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show("rewardedVideo", options);
    }

    public void CloseHintPanel(bool sound = false)
    {
        if(sound)
            Audio.Instance.PlaySound(Sound.Button);
        if (hintPanel.activeSelf)
            hintAnimator.SetBool("ShowWin", false);
    }

    private void HandleShowResult(ShowResult result)
    {
        noButtonText.transform.parent.gameObject.SetActive(false);
        yesButtonText.text = LocalizedText.Translate("ok");

        Button yesButton = yesButtonText.transform.parent.GetComponent<Button>();
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => CloseHintPanel(true));

        switch (result)
        {
            case ShowResult.Finished:
                hintText.text = LocalizedText.Translate("ads_success");
                hintPanel.SetActive(true);
                hintAnimator.SetBool("ShowWin", true);

                moveLeft += 5;
                adsShown = true;
                UpdateMoveLeft();
                break;
            case ShowResult.Skipped:
                hintText.text = LocalizedText.Translate("ads_not_finished");
                hintPanel.SetActive(true);
                hintAnimator.SetBool("ShowWin", true);
                break;
            case ShowResult.Failed:
                hintText.text = LocalizedText.Translate("ads_error");
                hintPanel.SetActive(true);
                hintAnimator.SetBool("ShowWin", true);
                break;
        }
    }
    #endif
}
