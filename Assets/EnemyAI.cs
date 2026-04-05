using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{

    [Header("Enemy Properties")]
    public StatsSystem StatsSystem;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private Transform[] Movepoint;
    [SerializeField] private int MoveAmount = 0;
    [SerializeField] private int prev_Point=0;
    [SerializeField] private float YOffset;
    [SerializeField] private float XOffset;
    [SerializeField] private GameObject IdleModle;
    public AudioManager aud;
    [SerializeField] private TextMeshProUGUI LVLText;

    [Header("Type Properties")]
    [SerializeField] private int type;
    public int level;

    private void Start()
    {
        LVLText.text = "LVL "+level;
        LVLText.gameObject.SetActive(false);
        GameManager = FindObjectOfType<GameManager>();
        StatsSystem = GetComponent<StatsSystem>();
        aud = FindAnyObjectByType<AudioManager>();

        Movepoint[5] = GameObject.Find("Player").transform;
    }

    private void OnEnable()
    {
        IdleModle.SetActive(false);
    }

    private bool previousDamagedState;

    private void Update()
    {
        if (StatsSystem.Damaged != previousDamagedState)
        {
            if (StatsSystem.Damaged)
            {
                GameManager.AddScore("EnemyShot");
            }
            else
            {
                // Do something when it becomes false
            }

            // Update the previous state
            previousDamagedState = StatsSystem.Damaged;
        }
    }

    private void OnDestroy()
    {
        IdleModle.SetActive(false);
        GameManager.CurEnemies.Remove(gameObject.transform.parent.gameObject);
        GameManager.MovedEnemiesAmount++;
    }


    public void DoStartTurn()
    {
        StartCoroutine(StartTurn());
    }

    IEnumerator StartTurn()
    {
        yield return new WaitForSeconds(1f);
        GameManager.doShootControls = true;
        float RandomTime;

        switch (type)
        {
            case 0: //Queen
                MoveAmount = Random.Range(0, 6);

                while (MoveAmount > 0)
                {

                    int RandomPosition;
                    do
                    {
                        RandomPosition = Random.Range(0, 4);
                    } while (RandomPosition == prev_Point);
                    prev_Point = RandomPosition;

                    RandomTime = Random.Range(1 / (float)PlayerPrefs.GetInt("LVL", 2), 2f / (float)PlayerPrefs.GetInt("LVL", 2));
                    MoveTo(RandomPosition, RandomTime, false);
                    yield return new WaitForSeconds(RandomTime);
                    MoveAmount--;
                }

                MoveTo(5, 1 / (float)PlayerPrefs.GetInt("LVL", 2), true);
                yield return new WaitForSeconds(0.5f / (float)PlayerPrefs.GetInt("LVL", 2));
                int critChance = Random.Range(1, 101);
                if (critChance <= StatsSystem.CritRate)
                {
                    Debug.Log("Queen Critical Hit!!!");
                    float trueDamage = (StatsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage*2, true);
                    LVLText.gameObject.SetActive(true);
                }
                else
                {
                    float trueDamage = (StatsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage, false);
                    LVLText.gameObject.SetActive(true);
                }
                FinalMove();
                break;



            case 1: //Pawn

                RandomTime = Random.Range(0.3f / (float)PlayerPrefs.GetInt("LVL", 2), 1 / (float)PlayerPrefs.GetInt("LVL", 2));
                MoveTo(5, RandomTime, true);
                yield return new WaitForSeconds(RandomTime);
                int critChancePawn = Random.Range(1, 101);
                if (critChancePawn <= StatsSystem.CritRate)
                {
                    Debug.Log("Pawn Critical Hit!!!");
                    float trueDamage = (StatsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage * 2, true);
                    LVLText.gameObject.SetActive(true);
                }
                else
                {
                    float trueDamage = (StatsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage, false);
                    LVLText.gameObject.SetActive(true);
                }
                FinalMove();
                break;
        }
    }

    void MoveTo(int i, float time, bool finalMove)
    {
        Vector3 FinalPos = new (Movepoint[i].transform.position.x+XOffset, YOffset, Movepoint[i].transform.position.z);
        if (i==5) FinalPos += new Vector3(1,0,0);

        if (!finalMove)
        {
            int chance = Random.Range(1, 5);
            switch (chance)
            {
                case 1:
                    transform.LeanMove(FinalPos, time).setEaseOutElastic().setOnComplete(() =>
                    {
                        aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                    });
                    break;
                case 2:
                    transform.LeanMove(FinalPos, time).setEaseInOutSine().setOnComplete(() =>
                    {
                        aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                    });
                    break;
                case 3:
                    transform.LeanMove(FinalPos, time).setEaseOutQuint().setOnComplete(() =>
                    {
                        aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                    });
                    break;
                case 4:
                    transform.LeanMove(FinalPos, time).setEaseOutCirc().setOnComplete(() =>
                    {
                        aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                    });
                    break;
            }
        }
        else
        {
            transform.LeanMove(FinalPos, time).setEaseOutCirc().setOnComplete(() =>
            {
                aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
            });
        }
    }

    void FinalMove()
    {
        Vector3 FinalPos = new(Movepoint[0].transform.position.x + XOffset, YOffset, Movepoint[0].transform.position.z);

        int chance = Random.Range(1, 5);
        switch (chance)
        {
            case 1:
                transform.LeanMove(FinalPos, 1).setEaseOutElastic().setOnComplete(() => {
                    GameManager.MovedEnemiesAmount++;
                    IdleModle.SetActive(true);
                    gameObject.SetActive(false);
                    aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                });
                break;
            case 2:
                transform.LeanMove(FinalPos, 1).setEaseInOutSine().setOnComplete(() => {
                    GameManager.MovedEnemiesAmount++;
                    IdleModle.SetActive(true);
                    gameObject.SetActive(false);
                    aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                });
                break;
            case 3:
                transform.LeanMove(FinalPos, 1).setEaseOutQuint().setOnComplete(() => {
                    GameManager.MovedEnemiesAmount++;
                    IdleModle.SetActive(true);
                    gameObject.SetActive(false);
                    aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                });
                break;
            case 4:
                transform.LeanMove(FinalPos, 1).setEaseOutCirc().setOnComplete(() => {
                    GameManager.MovedEnemiesAmount++;
                    IdleModle.SetActive(true);
                    gameObject.SetActive(false);
                    aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
                });
                break;
        }
    }
}
