using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ObjectAutoScaleEffect;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool BypassVentReq;
    [SerializeField] private bool AvoidPlayerDamage;

    [Header("Debug - Panels")]
    [SerializeField] private float BulletDiagonalAmount;
    [SerializeField] private int ChoiceBulletAmount;
    [SerializeField] private int AttackBulletAmount;
    [SerializeField] private float AnimationTime;
    [SerializeField] private float RightPanelHideDistance;
    [SerializeField] private float RightPanelHideOffset;
    [SerializeField] private float LeftPanelHideDistance;

    [Header("Border Test")]
    [SerializeField] private bool TriggerBorderOpen;
    [SerializeField] private bool TriggerBorderClose;
    [SerializeField] private bool CurrentScreenSize;
    [SerializeField] private bool TriggerBorderCinematize;
    [SerializeField] private GameObject HigherBorder;
    [SerializeField] private GameObject LowerBorder;
    [SerializeField] private float BorderOpenDistance;
    [SerializeField] private float BorderCloseDistance;
    [SerializeField] private float BorderCinematizeDistance;
    [SerializeField] private float BorderAnimationTime;
    [SerializeField] private float BorderOffset;



    //Border Controls
    void BorderOpen()
    {
        aud.PlaySound(aud.SoundFX, aud.s_OpenBorder);
        LeanTween.cancel(HigherBorder);
        LeanTween.cancel(LowerBorder);
        HigherBorder.transform.LeanMoveY(BorderOpenDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint();
        LowerBorder.transform.LeanMoveY(-BorderOpenDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint();
    }

    void BorderClose()
    {
        aud.PlaySound(aud.SoundFX, aud.s_CloseBorder);
        LeanTween.cancel(HigherBorder);
        LeanTween.cancel(LowerBorder);
        HigherBorder.transform.LeanMoveY(-BorderCloseDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint();
        LowerBorder.transform.LeanMoveY(BorderCloseDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint();
    }

    void BorderCinematize()
    {
        aud.PlaySound(aud.SoundFX, aud.s_OpenBorder);
        LeanTween.cancel(HigherBorder);
        LeanTween.cancel(LowerBorder);
        HigherBorder.transform.LeanMoveY(BorderCinematizeDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint();
        LowerBorder.transform.LeanMoveY(-BorderCinematizeDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint();
    }



    [Header("Debug - Cam Controls")]
    [SerializeField] private bool TriggerEnvironmentScene1;
    [SerializeField] private bool TriggerEnvironmentScene2;
    [SerializeField] private bool TriggerFirstPersonView;
    [SerializeField] private bool ManualCinemaView_Prev;
    [SerializeField] private bool ManualCinemaIndex_Next;
    [SerializeField] private int ManualCinemaIndex;

    [Header("Debug - Scene Controls")]
    [SerializeField] private bool RerunIntro;
    [SerializeField] private bool TriggerStopCamMovement;
    [SerializeField] private bool TriggerCinemaView;
    [SerializeField] private bool TriggerLandscapeView;
    [SerializeField] private bool AttackShootTest;
    [SerializeField] private bool AttackShootReload;
    [SerializeField] private bool EnemyMoveTest;
    [SerializeField] private ShaderEffect_BleedingColors ColorBleed;
    [SerializeField] private ShaderEffect_Unsync Unsync;

    [Header("Audio")]
    [SerializeField] private AudioManager aud;

    [Header("Debug - Cam")]
    [SerializeField] private float CinemaViewSpeed;
    [SerializeField] private float LandscapeViewSpeed;
    [SerializeField] private float FirstPersonViewSpeed;
    [SerializeField] private bool doRandom = true;

    [Header("CAMERA PROPERTIES")]
    [SerializeField] private Follow CamFollow;
    [SerializeField] private LookAt CamLookAt;
    [SerializeField] private CameraTransfer CamTransfer;
    [SerializeField] private Transform[] DynamicPositionPoints;
    [SerializeField] private Transform[] StaticPositionPoints;
    [SerializeField] private Transform[] ViewPoints;
    [SerializeField] private int index;
    [SerializeField] private Vector2 delayRange;


    //Data
    private Coroutine c_Intro;
    private Coroutine c_CinemaView;
    private Coroutine c_Parry;
    private Coroutine c_GameOver;
    private Coroutine c_Tooltip;
    private float CylinderRotation=30;
    private float AttackCylinderRotation = 30;
    private bool isIntroSkippable;


    [Header("Stats Panel Properties")]
    [SerializeField] private int Wave = 0;
    [SerializeField] private CanvasGroup WaveCounterOnCinematize;
    [SerializeField] private CanvasGroup ScoreCounterOnCinematize;
    [SerializeField] private GameObject ScoreCounter;
    [SerializeField] private TextMeshProUGUI ScoreCounterText;
    [SerializeField] private TextMeshProUGUI HighScoreCounter;
    [SerializeField] private int Score;
    public StatsSystem PlayerStats;
    [SerializeField] private bool isPlayerAlive = true;
    [SerializeField] private GameObject StatsPanel;
    [SerializeField] private ConsoleText ConsoleText;
    [SerializeField] private ScoreSystem ScoreSystem;
    [SerializeField] private TMP_InputField InputField;
    [SerializeField] private CanvasGroup Tooltips;
    [SerializeField] private bool TooltipsShown;
    [SerializeField] private GameObject StageInfo;
    [SerializeField] private TextMeshProUGUI WaveCounter;
    [SerializeField] private TextMeshProUGUI DifficultyDisplay;
    [SerializeField] private CanvasGroup GameOverCanvas;
    [SerializeField] private CanvasGroup ChoicePanelCanvas;
    [SerializeField] private CanvasGroup AttackPanelCanvas;
    [SerializeField] private GameObject MessagePanel;
    [SerializeField] private TextMeshProUGUI MessagePanelText;

    [Header("Choice Panel Properties")]
    [SerializeField] private GameObject ChoicePanel;
    [SerializeField] private GameObject List;
    [SerializeField] private GameObject Cylinder;
    [SerializeField] private GameObject[] ChoiceButton;
    [SerializeField] private int[] ChoiceButtonPos;
    [SerializeField] private int ChoiceIndex=0;
    [SerializeField] private bool doChoiceControls;
    [SerializeField] private InventoryUI InvUI;
    [SerializeField] private int HeldCalliber;
    [SerializeField] private int HeldCalliberMax = 10;
    [SerializeField] private TextMeshProUGUI VentButtonText;
    [SerializeField] private GameObject StatsPanelHalo;
    [SerializeField] private ObjectAutoScaleEffect AttackHalo;
    [SerializeField] private GameObject ItemUseHalo;

    [Header("Attack Panel Properties")]
    [SerializeField] private GameObject AttackPanel;
    [SerializeField] private GameObject Clip;
    [SerializeField] private GameObject AttackCylinder;
    [SerializeField] private GameObject[] Bullets;
    [SerializeField] private int ClipIndex = 0;
    public bool doShootControls = false;
    [SerializeField] private ShootWithMouse Shooter;
    [SerializeField] private float ClipOffset;
    [SerializeField] private bool isParrying;
    [SerializeField] private GameObject Tonfa;
    [SerializeField] private GameObject ParryEffect;
    [SerializeField] private GameObject DamageEffect;
    [SerializeField] private GameObject DamageEffectParticle;
    [SerializeField] private GameObject SightCursor;
    [SerializeField] private GameObject Crack;
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private int DamageTaken;

    [Header("Stage Properties")]
    [SerializeField] private GameObject[] Spawnpoints;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private float GameOverUIHideDistance;
    [SerializeField] private float GameOverHideUIOffset;

    [Header("Enemy Properties")]
    [SerializeField] private GameObject[] EnemyTypes;
    public List<GameObject> CurEnemies;
    public List<GameObject> AllEnemies;
    public int MovingEnemiesAmount;
    public int MovedEnemiesAmount;
    [SerializeField] private int EnemyIndex;
    [SerializeField] private Vector3 EnemySpawnOffset;

    [Header("Vent Properties")]
    [SerializeField] GameObject VentPanel;
    [SerializeField] Transform VentBar;
    [SerializeField] bool VentModeControls;
    [SerializeField] float VentDamage;
    [SerializeField] GameObject UltExplosion;
    [SerializeField] GameObject UltCharge;

    [Header("Analyze Properties")]
    [SerializeField] CanvasGroup AnalyzePanel;
    [SerializeField] bool AnalyzeControls;
    [SerializeField] TextMeshProUGUI PlayerStatsText;
    [SerializeField] TextMeshProUGUI[] EnemyStatsText;






    public static GameManager Instance { get; internal set; }
    private void OnEnable()
    {
        Instance = this;
    }




    private void Update()
    {
        if (c_Intro!=null && isIntroSkippable)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
            {
                StopCoroutine(c_Intro);
                c_Intro = null;
                StartCoroutine(StartingWave());
            }
        }

        //Choice Panel Controls
        if (doChoiceControls)
        {
            //ListUp
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                ChangeList("up");
            }

            //ListDown
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                ChangeList("down");
            }

            //Shoot
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
            {
                ChoiceShoot();
                RestartTooltip();
                aud.PlaySound(aud.SoundFX, aud.s_CritGunShot);
                switch (ChoiceIndex)
                {
                    case 0:
                        {
                            if (HeldCalliber==HeldCalliberMax|| BypassVentReq)
                            {
                                VentMode();
                                HeldCalliber = 0;
                            }
                            else
                            {
                                DisplayMessage("Not enough Held Calliber! End turn with remaining bullets to obtain some.", false, 2);
                                aud.PlaySound(aud.SoundFX, aud.s_NoBullets);
                            }
                            break;
                        }
                    case 1:
                        {
                            AnalyzeMode(true);
                            break;
                        }
                    case 2:
                        {
                            NegotiateMode(true);
                            break;
                        }
                    case 3:
                        {
                            InventoryMode(true);
                            break;
                        }
                    case 4:
                        {
                            ShootMode(true);
                            break;
                        }
                }
            }
        }


        //Attack Panel Controls
        if (doShootControls)
        {
            if (PlayerStats.HP<=0 && isPlayerAlive)
            {
                StopAllCoroutines();
                c_GameOver ??= StartCoroutine(GameOver());
                doShootControls = false;
                isPlayerAlive = false;
            }
            

            if (MovedEnemiesAmount>=MovingEnemiesAmount)
            {
                MovingEnemiesAmount = 0;
                if (CurEnemies.Count == 0)
                {
                    EndWave();
                }
                else
                {
                    if (isPlayerAlive) EndTurn();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
            {
                RestartTooltip();
                if (ClipIndex>0)
                {
                    AttackShoot(); //Cylinder and Bullets Animation
                    Shooter.Shoot();
                    StartCoroutine(ShootEffect());
                } else
                {
                    aud.PlaySound(aud.SoundFX, aud.s_NoBullets);
                }

            }

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X))
            {
                c_Parry ??= StartCoroutine(Parry());
            }
        }

        //Vent Controls
        if (VentModeControls)
        {
            VentBar.localScale = new(VentDamage/1000, 1, 1);
            ColorBleed.intensity = (VentDamage/1000)*100;
            ColorBleed.shift = Mathf.Lerp(ColorBleed.shift, 0.02f, 0.1f);
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                //Bar and Screen VFXs
                if (VentDamage<1000) VentDamage += Random.Range(10, 30);
                ColorBleed.shift = Random.Range(-0.5f*(VentDamage/1000)*2, 0.5f* (VentDamage / 1000) * 2);

                //Muzzle Effect
                aud.PlaySound(aud.SoundFX, aud.s_UltShot);

                var muzzleflash = Instantiate(MuzzleFlash);
                var cursorTransform = SightCursor.transform;
                muzzleflash.transform.SetParent(cursorTransform.parent, false);

                if (SightCursor.TryGetComponent<RectTransform>(out var cursorRT) &&
                    muzzleflash.TryGetComponent<RectTransform>(out var muzzleRT))
                {
                    muzzleRT.localRotation = Quaternion.identity;
                    muzzleRT.anchoredPosition = cursorRT.anchoredPosition;
                    muzzleRT.SetAsLastSibling();
                }
                else
                {
                    muzzleflash.transform.SetPositionAndRotation(cursorTransform.position, cursorTransform.rotation);
                }
            }
        }

        if (AnalyzeControls)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(3))
            {
                AnalyzeMode(false);
            }
        }

        //Variable Syncing
        ScoreCounterText.text = "Score: " + Score;
        Shooter.doShoot = doShootControls;
        VentButtonText.text = "VENT ("+HeldCalliber+"/"+HeldCalliberMax+")";
    }



    private void FixedUpdate()
    {
        if (VentModeControls)
        {
            if (VentDamage > 0) VentDamage -= Random.Range(1, 3);
        }
    }



    private void Start()
    {
        aud.DoPlayMusic(aud.m_ChromaticRose, aud.m_ChromaticRose_wapred);

        BorderOffset = Screen.height / 2;
        BorderOpenDistance = Screen.height / 2;
        BorderCloseDistance = 0;
        BorderCinematizeDistance = (Screen.height/2)-(Screen.height/7);
        HigherBorder.GetComponent<RectTransform>().sizeDelta = new(Screen.width, Screen.height / 2);
        LowerBorder.GetComponent<RectTransform>().sizeDelta = new(Screen.width, Screen.height / 2);


        c_Intro = StartCoroutine(Intro());
        ColorBleed = Camera.main.gameObject.GetComponent<ShaderEffect_BleedingColors>();
        HighScoreCounter.text = "HIGHSCORE: " + ScoreSystem.GetHighestScoreString();
        MessagePanel.LeanScaleY(0, 0);
    }



    //Panels
    void HideAttackPanel(bool v)
    {
        if (v)
        {
            doShootControls = false;
            AttackPanel.transform.LeanMoveX(Screen.width + (Screen.width / 2), AnimationTime).setEaseOutQuint().setOnComplete(() =>
            {
                AttackPanelCanvas.alpha = 0;
            });
            Cursor.visible = true;
            SightCursor.SetActive(false);
        }
        else
        {
            AttackPanelCanvas.alpha = 1;
            AttackPanel.transform.LeanMoveX(Screen.width - 80, AnimationTime).setEaseOutQuint();
            Cursor.visible = false;
            SightCursor.SetActive(true);
        }
    }

    void HideStatsPanel(bool v)
    {
        if (v)
        {
            StageInfo.transform.LeanMoveX(Screen.width + (Screen.width / 2), AnimationTime).setEaseOutQuint();
            StatsPanel.transform.LeanMoveX(-(Screen.width / 2), AnimationTime).setEaseOutQuint();
            Cursor.visible = false;
        }
        else
        {
            StageInfo.transform.LeanMoveX(Screen.width - 80, AnimationTime).setEaseOutQuint();
            StatsPanel.transform.LeanMoveX(0, AnimationTime).setEaseOutQuint();
            Cursor.visible = true;


            WaveCounter.text = "SCENE : " + ConvertToRomanNumerals(Wave);
            string diff = PlayerPrefs.GetInt("LVL", 2) switch
            {
                1 => "Easy",
                2 => "Normal",
                3 => "Hard",
                _ => "Overclock"
            };
            DifficultyDisplay.text = diff;
            WaveCounter.text = "SCENE : " + ConvertToRomanNumerals(Wave);
        }
    }

    void HideChoicePanel(bool v)
    {
        if (v)
        {
            doChoiceControls = false;
            ChoicePanel.transform.LeanMoveX(Screen.width + (Screen.width / 2), AnimationTime).setEaseOutQuint().setOnComplete(() =>
            {
                ChoicePanelCanvas.alpha = 0;
            });
        }
        else
        {
            AttackHalo.autoScaleType = AutoScaleType.Grow;
            AttackHalo.gameObject.SetActive(true);
            ChoicePanelCanvas.alpha = 1;
            aud.PlaySound(aud.SoundFX, aud.s_ReloadGun);
            ChoicePanel.transform.LeanMoveX(Screen.width - 80, AnimationTime).setEaseOutQuint().setOnComplete(() =>
            {
                doChoiceControls = true;
            });
            StartCoroutine(CheckCurEnemy());
        }
    }

    void HideHighscorePanel(bool v)
    {
        if (v)
        {
            GameOverPanel.LeanMoveY((Screen.height * 2) - (Screen.height / 4), 1f).setEaseOutQuint();
            LeanTween.value(gameObject, 20, 10, 0.5f)
                .setOnUpdate(val =>
                {
                    Unsync.speed = val;
                })
                .setOnComplete(() => {
                    GameOverCanvas.alpha = 0;
                });
        }
        else
        {
            LeanTween.value(gameObject, 10, 20, 0.5f)
                .setOnUpdate(val =>
                {
                    Unsync.speed = val;
                })
                .setOnComplete(() => {
                    GameOverCanvas.alpha = 1;
                    GameOverPanel.LeanMoveY(Screen.height - (Screen.height / 4), 1f).setEaseOutQuint();
                });
        }

    }

    IEnumerator CheckCurEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        if (CurEnemies.Count == 0)
        {
            EndWave();
        }
    }
    


    //Choice Panel Methods
    public void DisplayMessage(string msg, bool good, float time)
    {
        LeanTween.cancel(MessagePanel);
        if (good) aud.PlaySound(aud.SoundFX, aud.s_DisplayGoodMessage);
        else      aud.PlaySound(aud.SoundFX, aud.s_DisplayBadMessage);
        MessagePanelText.text = msg;
        MessagePanel.LeanScaleY(0, 0).setOnComplete(() =>
        {
            MessagePanel.LeanScaleY(1, 0.3f).setEaseOutQuint().setOnComplete(() =>
            {
                MessagePanel.LeanScaleY(0, 0.3f).setEaseOutQuint().setDelay(time);
            });
        });
    }

    public IEnumerator OnItemUsed(ItemClass item)
    {
        ItemUseHalo.SetActive(true);
        HideStatsPanel(false);
        InvUI.ShowInventory(false);
        yield return new WaitForSeconds(0.5f);

        if (item is ItemRemedyClass)
        {
            ItemRemedyClass remedy = item as ItemRemedyClass;
            for (int i=0; i<6; i++)
            {
                DoPlayerView();
                switch (i)
                {
                    case 0:
                        if (remedy.AddedHP == 0) break;
                        PlayerStats.HP += remedy.AddedHP;
                        DisplayMessage("Your HP has increased!", true, 2);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 1:
                        if (remedy.AddedDEF == 0) break;
                        PlayerStats.DEF += remedy.AddedDEF;
                        DisplayMessage("Your DEF has increased!", true, 2);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 2:
                        if (remedy.AddedATK == 0) break;
                        PlayerStats.ATK += remedy.AddedATK;
                        DisplayMessage("Your ATK has increased!", true, 2);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 3:
                        if (remedy.AddedElemATK == 0) break;
                        PlayerStats.ElemATK += remedy.AddedElemATK;
                        DisplayMessage("Your Element ATK has increased!", true, 2);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 4:
                        if (remedy.AddedCritRate == 0) break;
                        PlayerStats.CritRate += remedy.AddedCritRate;
                        DisplayMessage("Your CritRate has increased!", true, 2);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 5:
                        if (remedy.AddedSPEED == 0) break;
                        PlayerStats.Speed += remedy.AddedSPEED;
                        DisplayMessage("Your SPEED has increased!", true, 2);
                        yield return new WaitForSeconds(1.5f);
                        break;
                }
            }
        }


        List<StatsSystem> enemyStats = new();
        foreach (GameObject enemy in CurEnemies) enemyStats.Add(enemy.GetComponentInChildren<StatsSystem>());
        /*
        if (item is ItemInflictorClass)
        {
            ItemInflictorClass inflictor = item as ItemInflictorClass;
            for (int i = 0; i < 6; i++)
            {
                DoPlayerView();
                switch (i)
                {
                    case 0:
                        if (inflictor.AddedHP == 0) break;
                        foreach (var enemy in enemyStats) enemy.HP = inflictor.AddedHP;
                        DisplayMessage("Your HP has increased!", true);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 1:
                        if (inflictor.AddedDEF == 0) break;
                        foreach (var enemy in enemyStats) enemy.DEF = inflictor.AddedDEF;
                        DisplayMessage("Your DEF has increased!", true);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 2:
                        if (inflictor.AddedATK == 0) break;
                        foreach (var enemy in enemyStats) enemy.DEF = inflictor.;
                        PlayerStats.ATK += inflictor.AddedATK;
                        DisplayMessage("Your ATK has increased!", true);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 3:
                        if (inflictor.AddedElemATK == 0) break;
                        PlayerStats.ElemATK += inflictor.AddedElemATK;
                        DisplayMessage("Your Element ATK has increased!", true);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 4:
                        if (inflictor.AddedCritRate == 0) break;
                        PlayerStats.CritRate += inflictor.AddedCritRate;
                        DisplayMessage("Your CritRate has increased!", true);
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 5:
                        if (inflictor.AddedSPEED == 0) break;
                        PlayerStats.Speed += inflictor.AddedSPEED;
                        DisplayMessage("Your SPEED has increased!", true);
                        yield return new WaitForSeconds(1.5f);
                        break;
                }
            }
        }*/

        DoFirstPersonView();
        HideChoicePanel(true);
        HideAttackPanel(false);
        ReloadAttackPanel(0);
        DoFirstPersonView();
        StartEnemyTurn();
    }

    public void ShootMode(bool reloaded)
    {
        HideStatsPanel(false);
        AddScore("ChoseFight");
        DoFirstPersonView();
        HideChoicePanel(true);
        HideAttackPanel(false);
        if (reloaded) ReloadAttackPanel(6);
        else ReloadAttackPanel(0);
        StartEnemyTurn();
        AttackHalo.autoScaleType = AutoScaleType.Shrink;
        AttackHalo.gameObject.SetActive(true);
    }

    public void InventoryMode(bool show)
    {
        if (show)
        {
            InvUI.gameObject.SetActive(true);
            InvUI.ShowInventory(true);
            HideChoicePanel(true);
            HideStatsPanel(true);
        }
        else
        {
            StartCoroutine(delayedInventoryHide());
        }
    }

    IEnumerator delayedInventoryHide()
    {
        InvUI.ShowInventory(false);
        yield return new WaitForSeconds(0.5f);
        HideChoicePanel(false);
        HideStatsPanel(false);
        StartUpChoicePanel(false);
        InvUI.gameObject.SetActive(false);
    }

    public void NegotiateMode(bool show)
    {
        NegotiateUI.Instance.gameObject.SetActive(show);
        NegotiateUI.Instance.ShowNegotiatePanel = show;
        if (show)
        {
            StatsPanelHalo.SetActive(true);
            Cursor.visible = false;
            SightCursor.SetActive(true);
            HideChoicePanel(true);
            HideStatsPanel(true);
            DoNegotiateView();
        }
        else
        {
            StartCoroutine(delayedNegotiateHide());
        }
    }

    IEnumerator delayedNegotiateHide()
    {
        yield return new WaitForSeconds(0.5f);
        Cursor.visible = true;
        SightCursor.SetActive(false);
        HideChoicePanel(false);
        HideStatsPanel(false);
        DoCinemaView();
        StartUpChoicePanel(false);
    }

    void ChangeList(string direction)
    {
        if (direction == "up" && ChoiceIndex < ChoiceBulletAmount)
        {
            ChoiceIndex++;
            aud.PlaySound(aud.SoundFX, aud.s_CylinderTurn);

            CylinderRotation -= 60f;
            LeanTween.cancel(Cylinder);
            Cylinder.transform.LeanRotateZ(CylinderRotation, AnimationTime).setEaseOutQuint();

            for (int i = ChoiceBulletAmount; i >= ChoiceIndex; i--)
            {
                ChoiceButtonPos[i] -= 1;
                LeanTween.cancel(ChoiceButton[i]);
                ChoiceButton[i].transform.LeanMoveLocalX(ChoiceButtonPos[i] * BulletDiagonalAmount, AnimationTime).setEaseOutQuint();
            }
            for (int i = 0; i <= ChoiceIndex - 1; i++)
            {
                ChoiceButtonPos[i] += 1;
                LeanTween.cancel(ChoiceButton[i]);
                ChoiceButton[i].transform.LeanMoveLocalX(ChoiceButtonPos[i] * BulletDiagonalAmount, AnimationTime).setEaseOutQuint();
            }


        }
        else if (direction == "down" && ChoiceIndex > 0)
        {
            ChoiceIndex--;
            aud.PlaySound(aud.SoundFX, aud.s_CylinderTurn);

            CylinderRotation += 60f;
            LeanTween.cancel(Cylinder);
            Cylinder.transform.LeanRotateZ(CylinderRotation, AnimationTime).setEaseOutQuint();

            for (int i = ChoiceBulletAmount; i > ChoiceIndex; i--)
            {
                ChoiceButtonPos[i] += 1;
                LeanTween.cancel(ChoiceButton[i]);
                ChoiceButton[i].transform.LeanMoveLocalX(ChoiceButtonPos[i] * BulletDiagonalAmount, AnimationTime).setEaseOutQuint();
            }
            for (int i = 0; i <= ChoiceIndex; i++)
            {
                ChoiceButtonPos[i] -= 1;
                LeanTween.cancel(ChoiceButton[i]);
                ChoiceButton[i].transform.LeanMoveLocalX(ChoiceButtonPos[i] * BulletDiagonalAmount, AnimationTime).setEaseOutQuint();
            }
        }


        //List Movement
        LeanTween.cancel(List);
        List.transform.LeanMoveLocalY(-240 + (ChoiceIndex * 80), AnimationTime).setEaseOutQuint();


    }

    void ChoiceShoot()
    {
        //Bullet Animation
        LeanTween.cancel(ChoiceButton[ChoiceIndex]);
        ChoiceButton[ChoiceIndex].transform.LeanMoveX(-2000, 0.7f).setEaseOutQuint().setOnComplete(() =>
        {
            ChoiceButton[ChoiceIndex].transform.LeanMoveX(3000, 0);
        });


    }

    void StartUpChoicePanel(bool reload)
    {
        for (int i = 0; i <= ChoiceBulletAmount; i++)
        {
            LeanTween.cancel(ChoiceButton[i]);
            ChoiceButton[i].transform.LeanMoveLocalX(ChoiceButtonPos[i] * BulletDiagonalAmount, AnimationTime).setEaseOutQuint();
        }

        if (reload)
        {
            HeldCalliber += ClipIndex;
            if (HeldCalliber > HeldCalliberMax)
                HeldCalliber = HeldCalliberMax;
        }
    }

    void VentMode()
    {
        AttackHalo.autoScaleType = AutoScaleType.Grow;
        AttackHalo.gameObject.SetActive(true);
        VentDamage = 0;
        ClipIndex = 0;
        VentPanel.SetActive(true);
        HideChoicePanel(true);
        HideStatsPanel(true);
        DoFirstPersonView();
        StartCoroutine(VentTimer());
        VentModeControls = true;
        aud.PlaySound(aud.SoundFX, aud.s_Ult);
        Cursor.visible = false;
        SightCursor.SetActive(true);
    }

    IEnumerator VentTimer()
    {
        List<StatsSystem> enemyStats = new();
        foreach (GameObject enemy in CurEnemies)
        {
            enemyStats.Add(enemy.GetComponentInChildren<StatsSystem>());
        }


        yield return new WaitForSeconds(2f);
        Instantiate(UltCharge, ViewPoints[2].position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(UltCharge, ViewPoints[0].position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);


        VentModeControls = false;
        for (int i = 0; i < 8; i++) Instantiate(UltExplosion, (ViewPoints[0].position) + new Vector3(ViewPoints[0].position.x, Random.Range(-1, 1), Random.Range(-4, 4)), Quaternion.identity);
        aud.PlaySound(aud.SoundFX, aud.s_UltExplosion);
        foreach (StatsSystem stats in enemyStats) stats.TakeDamage((int)VentDamage);
        VentPanel.SetActive(false);
        DoCinemaView();
        ColorBleed.intensity = 100;
        ColorBleed.shift = 0.3f;
        LeanTween.value(gameObject, ColorBleed.shift, 0.02f, 1f)
            .setOnUpdate((float val) => ColorBleed.shift = val);

        LeanTween.value(gameObject, ColorBleed.intensity, 10f, 1f)
            .setOnUpdate((float val) => ColorBleed.intensity = val);
        yield return new WaitForSeconds(1);
        AttackHalo.autoScaleType = AutoScaleType.Grow;
        AttackHalo.gameObject.SetActive(true);
        HideChoicePanel(false);
        HideStatsPanel(false);
        if (CurEnemies.Count == 0)
        {
            EndWave();
        }
        else
        {
            StartUpChoicePanel(true);
        }
    }

    void AnalyzeMode(bool show)
    {
        if (show)
        {
            AnalyzeControls = true;
            DoLandscapeView();
            PlayerStatsText.text = 
                $"HP: {PlayerStats.HP}/1000\r\n" +
                $"ATK: {PlayerStats.ATK}\r\n" +
                $"ElemATK: {PlayerStats.ElemATK}\r\n" +
                $"CritRate: {PlayerStats.CritRate}%\r\n" +
                $"DEF: {PlayerStats.DEF}\r\n" +
                $"SPEED: {PlayerStats.Speed}\r\n";

            int i = 0;
            foreach (GameObject enemy in CurEnemies)
            {
                StatsSystem stats = enemy.GetComponentInChildren<StatsSystem>();
                EnemyStatsText[i].gameObject.SetActive(true);
                EnemyStatsText[i].text =
                $"HP: {stats.HP}\r\n" +
                $"ATK: {stats.ATK}\r\n" +
                $"ElemATK: {stats.ElemATK}\r\n" +
                $"CritRate: {stats.CritRate}%\r\n" +
                $"DEF: {stats.DEF}\r\n" +
                $"SPEED: {stats.Speed}\r\n";
                i++;
            }

            LeanTween.value(gameObject, AnalyzePanel.alpha, 1, 0.2f)
            .setOnUpdate((float val) => AnalyzePanel.alpha = val);
            HideChoicePanel(true);
            HideStatsPanel(true);
        }
        else
        {
            AnalyzeControls = false;
            LeanTween.value(gameObject, AnalyzePanel.alpha, 0, 0.2f)
            .setOnUpdate((float val) => AnalyzePanel.alpha = val).setOnComplete(() =>
            {
                foreach (var text in EnemyStatsText) text.gameObject.SetActive(false);
            });
            StartCoroutine(delayedAnalyzeHide());
        }
    }

    IEnumerator delayedAnalyzeHide()
    {
        yield return new WaitForSeconds(0.5f);
        HideChoicePanel(false);
        HideStatsPanel(false);
        DoCinemaView();
        StartUpChoicePanel(false);
    }



    //Enemy Methods
    public void DamagePlayer(int dmg, bool criticalhit)
    {
        if (isParrying)
        {
            aud.PlaySound(aud.SoundFX, aud.s_Parried);
            Debug.Log("PARRY");
            StartCoroutine(Parried());
        }
        else
        {
            aud.PlaySound(aud.SoundFX, aud.s_EnemyBash);
            Debug.Log("Player took "+dmg+" dmg!!!");
            if (!AvoidPlayerDamage) PlayerStats.TakeDamage(dmg);
            StartCoroutine(Damaged());
            if (criticalhit)
            {
                Crack.SetActive(true);
            }
            DamageTaken += dmg;
        }
    }

    void StartEnemyTurn()
    {
        MovingEnemiesAmount = CurEnemies.Count;
        MovedEnemiesAmount = 0;
        for (int i = 0; i < AllEnemies.Count; i++)
        {
            AllEnemies[i].transform.Find("Idle").gameObject.SetActive(false);
        }
        StartCoroutine(StartEnemyTurnCorou());
    }

    IEnumerator StartEnemyTurnCorou()
    {
        for (int i = 0; i < CurEnemies.Count; i++)
        {
            CurEnemies[i].transform.GetChild(0).gameObject.SetActive(true);
            CurEnemies[i].transform.GetChild(0).GetComponent<StatsSystem>().RestoreMaterial();
            CurEnemies[i].transform.GetChild(0).GetComponent<EnemyAI>().DoStartTurn(i);
            yield return new WaitForSeconds(1);
        }
        for (int i = 0; i < CurEnemies.Count; i++)
        {
            if (CurEnemies[i] == null) continue;
            CurEnemies[i].transform.GetChild(0).gameObject.SetActive(true);
            CurEnemies[i].transform.GetChild(0).GetComponent<StatsSystem>().RestoreMaterial();
            CurEnemies[i].transform.GetChild(0).GetComponent<EnemyAI>().DoStartTurn(i);
        }
    }

    void SpawnEnemies()
    {
        //Setting Randomization
        int EnemyTypeChance;
        int EnemyAmount;
        do
        {
            if (Wave>6)
            {
                Debug.Log("Enemy amount randomized");
                EnemyAmount = Random.Range(4, Wave + 1);
            }
            else
                EnemyAmount = Wave;
        }
        while (Spawnpoints.Length < EnemyAmount);

        bool[] SpawnpointTaken = new bool[Spawnpoints.Length];

        for (int i = 0; i < EnemyAmount; i++)
        {
            //Making sure Spawnpoints are not reused
            int SpawnPointChance;
            do
            {
                SpawnPointChance = Random.Range(0, Spawnpoints.Length);
            }
            while (SpawnpointTaken[SpawnPointChance] == true);

            //Spawning Enemy
            EnemyTypeChance = Random.Range(0, EnemyTypes.Length);
            int EnemyLevelChance = Random.Range(1, Wave+1);
            GameObject enemy = Instantiate(EnemyTypes[EnemyTypeChance], Spawnpoints[SpawnPointChance].transform.position + EnemySpawnOffset, Quaternion.identity);
            enemy.transform.GetChild(0).GetComponent<EnemyAI>().level = EnemyLevelChance;
            CurEnemies.Add(enemy);
            AllEnemies.Add(enemy);
            SpawnpointTaken[SpawnPointChance] = true;
        }
    }

    void EndTurn()
    {
        HideAttackPanel(true);
        DoCinemaView();
        HideChoicePanel(false);
        StartUpChoicePanel(true);
        for (int i = 0; i < CurEnemies.Count; i++)
        {
            CurEnemies[i].transform.GetChild(0).gameObject.SetActive(true);
            CurEnemies[i].transform.GetChild(0).gameObject.GetComponent<StatsSystem>().RestoreMaterial();
        }

        for (int i = 0; i < AllEnemies.Count; i++)
        {
            AllEnemies[i].transform.Find("Idle").gameObject.SetActive(false);
        }

        StartCoroutine(DelayedEndTurn());
        AddScore("SurvivedTurn");



        if (DamageTaken <= 0)
        {
            AddScore("NoHit");
            DamageTaken = 0;
        }

        DamageTaken = 0;
    }

    IEnumerator DelayedEndTurn()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < CurEnemies.Count; i++)
        {
            CurEnemies[i].transform.GetChild(0).gameObject.SetActive(true);
        }

        for (int i = 0; i < AllEnemies.Count; i++)
        {
            AllEnemies[i].transform.Find("Idle").gameObject.SetActive(false);
        }
    }

    void EndWave()
    {
        aud.PlaySound(aud.SoundFX, aud.s_ClearStage);
        doShootControls = false;
        for (int i = 0; i < AllEnemies.Count; i++)
        {
            Destroy(AllEnemies[i]);
        }
        AllEnemies.Clear();
        DoRerunIntro();
        Debug.Log("Wave Ended");
    }


    
    //Stats Panel Methods
    void RestartTooltip()
    {
        if (c_Tooltip!=null)
        {
            StopCoroutine(c_Tooltip);
            c_Tooltip = null;
        }
        if (!TooltipsShown)
        {
            Tooltips.alpha = 0;
        }
        c_Tooltip = StartCoroutine(EventuallyShowTooltip());
    }

    IEnumerator EventuallyShowTooltip()
    {
        yield return new WaitForSeconds(5);
        TooltipsShown = true;
        LeanTween.alphaCanvas(Tooltips, 1, 1);
    }

    IEnumerator Damaged()
    {
        float RandomIntensity = Random.Range(10, 200);
        float RandomShift = Random.Range(0, 1);
        ColorBleed.intensity = RandomIntensity;
        ColorBleed.shift = RandomShift;
        DamageEffect.SetActive(true);
        Instantiate(DamageEffectParticle, GameObject.Find("Player").transform.position+new Vector3(1,1,0), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        DamageEffect.SetActive(false);
        ColorBleed.intensity = 10;
        ColorBleed.shift = 0.01f;
    }

    void CinematizedHideWaveAndScore(bool v)
    {
        LeanTween.cancel(ScoreCounterOnCinematize.gameObject);
        LeanTween.cancel(WaveCounterOnCinematize.gameObject);

        if (v)
        {
            LeanTween.alphaCanvas(ScoreCounterOnCinematize, 0f, 1f).setEaseOutQuint();
            LeanTween.alphaCanvas(WaveCounterOnCinematize, 0f, 1f).setEaseOutQuint();
        }
        else
        {
            LeanTween.alphaCanvas(ScoreCounterOnCinematize, 1f, 2f).setEaseOutQuint();
            LeanTween.alphaCanvas(WaveCounterOnCinematize, 1f, 2f).setEaseOutQuint();
        }
    }

    public void AddScore(string condition)
    {
        int AddedScore;
        switch (condition)
        {
            case "SurvivedTurn":
                AddedScore = 300*(PlayerPrefs.GetInt("LVL", 2));
                Score += AddedScore;
                ConsoleText.AddText("Survived turn +"+AddedScore);
                break;

            case "ChoseFight":
                AddedScore = 225 * (PlayerPrefs.GetInt("LVL", 2));
                Score += AddedScore;
                ConsoleText.AddText("Chose to fight +" + AddedScore);
                break;

            case "EnemyShot":
                AddedScore = 75 * (PlayerPrefs.GetInt("LVL", 2));
                Score += AddedScore;
                ConsoleText.AddText("Shot enemy +" + AddedScore);
                break;

            case "Parried":
                AddedScore = 150 * (PlayerPrefs.GetInt("LVL", 2));
                Score += AddedScore;
                ConsoleText.AddText("PARRIED +" + AddedScore);
                break;

            case "NoHit":
                AddedScore = 500 * (PlayerPrefs.GetInt("LVL", 2));
                Score += AddedScore;
                ConsoleText.AddText("NO HIT +" + AddedScore);
                break;
            default:
                ConsoleText.AddText("Error: \""+condition+"\" may be mistyped");
                break;
        }
    }



    //Attack Panel Methods
    void AttackShoot()
    {
        //Bullet Animation
        if (ClipIndex>0)
        {

            ClipIndex--;
            Bullets[ClipIndex].transform.LeanMoveX(-2000, AnimationTime).setEaseOutQuint().setOnComplete(() =>
            {
                Bullets[ClipIndex].transform.LeanMoveX(3000, 0);
                Bullets[ClipIndex].SetActive(false);
            });


            AttackCylinderRotation += 60f;
            LeanTween.cancel(AttackCylinder);
            AttackCylinder.transform.LeanRotateZ(AttackCylinderRotation, AnimationTime).setEaseOutQuint();

            Clip.transform.LeanMoveLocalY(ClipOffset + (ClipIndex * 80), AnimationTime).setEaseOutQuint();
        }
    }

    IEnumerator ShootEffect()
    {
        aud.PlaySound(aud.SoundFX, aud.s_GunShot);

        ColorBleed.intensity = Random.Range(5f, 50f);
        ColorBleed.shift = Random.Range(0f, 0.1f);

        var muzzleflash = Instantiate(MuzzleFlash);
        var cursorTransform = SightCursor.transform;
        muzzleflash.transform.SetParent(cursorTransform.parent, false);

        if (SightCursor.TryGetComponent<RectTransform>(out var cursorRT) &&
            muzzleflash.TryGetComponent<RectTransform>(out var muzzleRT))
        {
            muzzleRT.localRotation = Quaternion.identity;
            muzzleRT.anchoredPosition = cursorRT.anchoredPosition;
            muzzleRT.SetAsLastSibling();
        }
        else
        {
            muzzleflash.transform.SetPositionAndRotation(cursorTransform.position, cursorTransform.rotation);
        }

        yield return new WaitForSeconds(0.1f);

        ColorBleed.intensity = 10;
        ColorBleed.shift = 0.01f;
    }

    void ReloadAttackPanel(int bullet)
    {
        if (bullet < 6)
            ClipIndex = bullet;
        else
            ClipIndex = 6;

        Debug.Log("Reloaded");
        foreach (GameObject bullets in Bullets) bullets.SetActive(false); 
        for (int i = 0; i < bullet; i++)
        {
            Bullets[i].SetActive(true);
            LeanTween.cancel(Bullets[i]);
            Bullets[i].transform.LeanMoveX(3000, 0);
            Bullets[i].transform.LeanMoveLocalX(0, AnimationTime).setEaseOutQuint();
        }
        Clip.transform.LeanMoveLocalY(ClipOffset + (ClipIndex * 80), AnimationTime).setEaseOutQuint();

        AttackCylinderRotation -= 120;
        LeanTween.cancel(AttackCylinder);
        AttackCylinder.transform.LeanRotateZ(AttackCylinderRotation, AnimationTime).setEaseOutQuint().setOnComplete(() =>
        {
            AttackCylinderRotation = 30;
            
        });

        if (CurEnemies.Count==0)
        {
            EndWave();
        }
    }

    IEnumerator Parry()
    {
        aud.PlaySound(aud.SoundFX, aud.s_ParryAttempt);
        isParrying = true;
        Tonfa.transform.LeanRotateZ(-60, 0.05f).setEaseOutCirc();
        yield return new WaitForSeconds(0.1f);
        Tonfa.transform.LeanRotateZ(0, 0.2f).setEaseInQuart();
        yield return new WaitForSeconds(0.1f);
        isParrying = false;
        yield return new WaitForSeconds(0.4f*(10/(9+PlayerStats.Speed))); //Cooldown
        c_Parry = null;
    }

    IEnumerator Parried()
    {
        float RandomIntensity = Random.Range(10, 200);
        float RandomShift = Random.Range(0, 1);
        ColorBleed.intensity = RandomIntensity;
        ColorBleed.shift = RandomShift;
        ParryEffect.SetActive(true);
        Time.timeScale = 0;
        Instantiate(DamageEffectParticle, GameObject.Find("Player").transform.position + new Vector3(1, 1, 0), Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        ParryEffect.SetActive(false);
        ColorBleed.intensity = 10;
        ColorBleed.shift = 0.01f;
        AddScore("Parried");
    }



    //Camera Methods
    public void DoNegotiateView()
    {
        if (c_CinemaView != null) StopCoroutine(c_CinemaView);
        CamFollow.doFollow = true;
        CamLookAt.doLook = true;
        CamFollow.SetPos(DynamicPositionPoints[7]);
        CamLookAt.SetLook(ViewPoints[2]);
        CamFollow.SetSpeed(LandscapeViewSpeed);
    }

    public void DoPlayerView()
    {
        if (c_CinemaView != null) StopCoroutine(c_CinemaView);
        CamFollow.doFollow = true;
        CamLookAt.doLook = true;
        CamFollow.SetPos(StaticPositionPoints[6]);
        CamLookAt.SetLook(StaticPositionPoints[0]);
        CamFollow.SetSpeed(CinemaViewSpeed);
    }

    public void DoEnemyView()
    {
        if (c_CinemaView != null) StopCoroutine(c_CinemaView);
        CamFollow.doFollow = true;
        CamLookAt.doLook = true;
        CamFollow.SetPos(StaticPositionPoints[6]);
        CamLookAt.SetLook(ViewPoints[2]);
        CamFollow.SetSpeed(CinemaViewSpeed);
    }

    public void DoLandscapeView()
    {
        if (c_CinemaView!=null) StopCoroutine(c_CinemaView);
        CamFollow.doFollow = true;
        CamLookAt.doLook = true;
        CamFollow.SetPos(StaticPositionPoints[1]);
        CamLookAt.SetLook(ViewPoints[1]);
        CamFollow.SetSpeed(LandscapeViewSpeed);
    }

    public void DoFirstPersonView()
    {
        if (c_CinemaView!=null) StopCoroutine(c_CinemaView);
        CamFollow.doFollow = true;
        CamLookAt.doLook = true;
        CamFollow.SetPos(StaticPositionPoints[0]);
        CamLookAt.SetLook(ViewPoints[0]);
        CamFollow.SetSpeed(FirstPersonViewSpeed);
    }

    public void DoCinemaView()
    {
        CamFollow.doFollow = true;
        CamLookAt.doLook = true;
        index = Random.Range(0, DynamicPositionPoints.Length-1);
        CamFollow.SetPos(DynamicPositionPoints[index]);
        index = 0;
        CamLookAt.SetLook(ViewPoints[index]);
        CamFollow.SetSpeed(CinemaViewSpeed);
        if (c_CinemaView!=null) StopCoroutine(c_CinemaView);
        c_CinemaView = StartCoroutine(CinemaView());
    }

    void StopCinemaView()
    {
        if (c_CinemaView!=null) StopCoroutine(c_CinemaView);
        c_CinemaView = null;
        CamFollow.doFollow = false;
        CamLookAt.doLook = false;
    }

    IEnumerator CinemaView()
    {
        while (true)
        {
            //Randomize Delay
            float delay = Random.Range(delayRange.x, delayRange.y);
            yield return new WaitForSeconds(delay);


            //Randomize Index
            if (doRandom)
            {
                int chance = Random.Range(0, 4);

                switch (chance)
                {
                    case 0:
                        {
                            index++;
                            break;
                        }
                    case 1:
                        {
                            index--;
                            break;
                        }
                    case 2:
                        {
                            index += 4;
                            break;
                        }
                    case 3:
                        {
                            index -= 4;
                            break;
                        }
                }
            }
            else
            {
                index++;
            }


            //Apply Wrap-around
            if (index >= DynamicPositionPoints.Length)
            {
                index -= DynamicPositionPoints.Length;
            }
            else if (index < 0)
            {
                index += DynamicPositionPoints.Length;
            }

            //Apply Index
            CamFollow.SetPos(DynamicPositionPoints[index]);
            CamFollow.SetSpeed(CinemaViewSpeed);
        }
    }



    //Scene
    IEnumerator GameOver()
    {
        aud.DoLerpPitch(aud.Music, 0.4f, 7f);
        aud.PlaySound(aud.SoundFX, aud.s_FailedStage);
        ColorBleed.intensity = 400;
        ConsoleText.ClearAllText();
        Crack.SetActive(true);
        HideAttackPanel(true);
        HideChoicePanel(true);
        HideStatsPanel(true);
        if (c_CinemaView != null) StopCoroutine(c_CinemaView);
        CamFollow.doFollow = true;
        CamLookAt.doLook = true;
        CamFollow.SetPos(StaticPositionPoints[3]);
        CamFollow.SetSpeed(0.5f);
        BorderCinematize();
        CamLookAt.SetSpeed(999);
        yield return new WaitForSeconds(3);
        CamFollow.SetPos(StaticPositionPoints[5]);
        CamTransfer.SetScene(2);
        yield return new WaitForSeconds(2);
        BorderOpen();
        yield return new WaitForSeconds(0.2f);
        HideHighscorePanel(false);
        Cursor.visible = true;
        c_GameOver = null;



    }

    public void DoExitGame(bool save)
    {
        StartCoroutine(ExitGame(save));
    }

    IEnumerator ExitGame(bool save)
    {
        BorderClose();
        yield return new WaitForSeconds(BorderAnimationTime);
        
        if (save)
        {
            string dif = PlayerPrefs.GetInt("LVL") switch
            {
                1 => "Easy",
                2 => "Normal",
                3 => "Hard",
                _ => "Overclock",
            };
            ScoreSystem.SaveScore(InputField.text, Score, "SCENE:"+Wave.ToString(), dif);
        }

        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
    }

    void DoRerunIntro()
    {
        LeanTween.cancel(HigherBorder);
        LeanTween.cancel(LowerBorder);
        HigherBorder.transform.LeanMoveY(BorderCloseDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint();
        LowerBorder.transform.LeanMoveY(-BorderCloseDistance + BorderOffset, BorderAnimationTime).setEaseOutQuint().setOnComplete(() =>
        {

            c_Intro = StartCoroutine(Intro());

        });
        StopCinemaView();
    }

    IEnumerator Intro()
    {
        ConsoleText.ClearAllText();
        Wave++;

        //Start Up
        HideChoicePanel(true);
        HideAttackPanel(true);
        HideStatsPanel(true);
        HideHighscorePanel(true);
        
        index = 0;
        ColorBleed.shift = 0.02f;
        ColorBleed.intensity = 10;
        ChoicePanel.transform.LeanMoveX(RightPanelHideDistance + RightPanelHideOffset, AnimationTime).setEaseOutQuint().setOnComplete(() =>
        {
            StartUpChoicePanel(true);
        });

        SpawnEnemies();

        //Environment Scene 1
        CamLookAt.doLook = true;
        CamLookAt.SetLook(ViewPoints[0]);
        CamLookAt.SetSpeed(999);
        CamTransfer.SetScene(0);
        yield return new WaitForSeconds(0.8f);

        CamFollow.doFollow = true;

        CamFollow.SetPos(StaticPositionPoints[3]);
        CamFollow.SetSpeed(0.5f);
        BorderCinematize();
        WaveCounterOnCinematize.GetComponent<TextMeshProUGUI>().text = "Scene : " + ConvertToRomanNumerals(Wave);
        ScoreCounterOnCinematize.GetComponent<TextMeshProUGUI>().text = "Score : "+Score;
        yield return new WaitForSeconds(1);
        isIntroSkippable = true;
        CinematizedHideWaveAndScore(false);
        yield return new WaitForSeconds(2);

        //Environment Scene 2
        CamFollow.SetPos(StaticPositionPoints[5]);
        CamTransfer.SetScene(2);
        yield return new WaitForSeconds(3);


        StartCoroutine(StartingWave());
    }

    IEnumerator StartingWave()
    {
        CinematizedHideWaveAndScore(true);
        isIntroSkippable = false;
        c_Intro = null;
        BorderClose();
        yield return new WaitForSeconds(1);
        CamTransfer.SetScene(5);
        CamLookAt.SetSpeed(10);
        DoCinemaView();
        BorderOpen();
        yield return new WaitForSeconds(1);

        Tooltips.alpha = 0;
        TooltipsShown = false;
        RestartTooltip();
        HideChoicePanel(false);
        HideStatsPanel(false);
    }



    //Utility 
    public string ConvertToRomanNumerals(int number)
    {
        if (number < 1 || number > 3999)
            throw new System.ArgumentOutOfRangeException("Number must be between 1 and 3999");

        StringBuilder roman = new();

        var values = new (int value, string symbol)[]
        {
            (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
            (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
            (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")
        };

        int remaining = number;
        foreach (var (value, symbol) in values)
        {
            while (remaining >= value)
            {
                roman.Append(symbol);
                remaining -= value;
            }
        }

        return roman.ToString();
    }

}
