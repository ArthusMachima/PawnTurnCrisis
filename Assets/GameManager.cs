using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{

    void HideAttackPanel(bool v)
    {
        if (v)
        {
            doShootControls = false;
            AttackPanel.transform.LeanMoveX(Screen.width+(Screen.width/2), AnimationTime).setEaseOutQuint().setOnComplete(() =>
            {
                AttackPanelCanvas.alpha = 0;
            });
            Cursor.visible = true;
            SightCursor.SetActive(false);
        }
        else
        {
            AttackPanelCanvas.alpha = 1;
            AttackPanel.transform.LeanMoveX(Screen.width-80, AnimationTime).setEaseOutQuint();
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
            ChoicePanelCanvas.alpha = 1;
            aud.PlaySound(aud.SoundFX, aud.s_ReloadGun);
            ChoicePanel.transform.LeanMoveX(Screen.width-80, AnimationTime).setEaseOutQuint().setOnComplete(() =>
            {
                doChoiceControls = true;
            });
        }
    }

    void HideHighscorePanel(bool v)
    {
        if (v)
        {
            GameOverPanel.LeanMoveY((Screen.height*2)-(Screen.height/4), 1f).setEaseOutQuint();
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
    [SerializeField] private StatsSystem PlayerStats;
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
                            Debug.Log("Guard");
                            break;
                        }
                    case 1:
                        {
                            Debug.Log("Negotiate");
                            break;
                        }
                    case 2:
                        {
                            InventoryMode(true);
                            break;
                        }
                    case 3:
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
                    EndTurn();
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
                    Debug.Log("Out of Ammo!");
                }

            }

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X))
            {
                c_Parry ??= StartCoroutine(Parry());
            }
        }


        //Variable Syncing
        ScoreCounterText.text = "Score: " + Score;
        Shooter.doShoot = doShootControls;
        VentButtonText.text = "VENT ("+HeldCalliber+"/"+HeldCalliberMax+")";
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
        HighScoreCounter.text = "HIGHSCORE: "+ScoreSystem.GetHighestScoreString();
    }



    //Choice Panel Methods

    public IEnumerator OnItemUsed()
    {
        InvUI.ShowInventory(false);
        yield return new WaitForSeconds(0.5f);
        HideStatsPanel(false);
        DoFirstPersonView();
        HideChoicePanel(true);
        HideAttackPanel(false);
        ReloadAttackPanel(0);

        int effectAmount = 3;
        for (int i=0; i<effectAmount; i++)
        {
            //Insert effect logic here
            Debug.Log("ITEM USED!! *insert item effect here*");
            yield return new WaitForSeconds(1f);
        }

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
    }

    public void InventoryMode(bool show)
    {
        if (show)
        {
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
        StartUpChoicePanel();
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
        List.transform.LeanMoveLocalY(-160 + (ChoiceIndex * 80), AnimationTime).setEaseOutQuint();


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

    void StartUpChoicePanel()
    {
        for (int i = 0; i <= ChoiceBulletAmount; i++)
        {
            LeanTween.cancel(ChoiceButton[i]);
            ChoiceButton[i].transform.LeanMoveLocalX(ChoiceButtonPos[i] * BulletDiagonalAmount, AnimationTime).setEaseOutQuint();
        }

        HeldCalliber += ClipIndex;
        if (HeldCalliber>HeldCalliberMax)
            HeldCalliber = HeldCalliberMax;

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
            PlayerStats.TakeDamage(dmg);
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
        for (int i = 0; i < CurEnemies.Count; i++)
        {
            CurEnemies[i].transform.GetChild(0).gameObject.SetActive(true);
            CurEnemies[i].transform.GetChild(0).GetComponent<StatsSystem>().RestoreMaterial();
            CurEnemies[i].transform.GetChild(0).GetComponent<EnemyAI>().DoStartTurn();
        }
        for (int i = 0; i < AllEnemies.Count; i++)
        {
            AllEnemies[i].transform.Find("Idle").gameObject.SetActive(false);
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
        StartUpChoicePanel();
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
        yield return new WaitForSeconds(0.4f); //Cooldown
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
        ChoicePanel.transform.LeanMoveX(RightPanelHideDistance + RightPanelHideOffset, AnimationTime).setEaseOutQuint().setOnComplete(() =>
        {
            StartUpChoicePanel();
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
