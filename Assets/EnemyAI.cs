using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{

    [Header("Enemy Properties")]
    public StatsSystem statsSystem;
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
    [SerializeField] ItemClass[] Drops;

    private void Start()
    {
        LVLText.text = "LVL "+level;
        LVLText.gameObject.SetActive(false);
        GameManager = FindObjectOfType<GameManager>();
        statsSystem = GetComponent<StatsSystem>();
        statsSystem.DEF = Random.Range(1, level + 1);
        statsSystem.ATK = Random.Range(100, 100+(100*level)/4);
        statsSystem.CritRate = Random.Range(10, 10 + (10 * level) / 4);
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
        if (statsSystem.Damaged != previousDamagedState)
        {
            if (statsSystem.Damaged)
            {
                GameManager.AddScore("EnemyShot");
            }
            else
            {
                // Do something when it becomes false
            }

            // Update the previous state
            previousDamagedState = statsSystem.Damaged;
        }
    }

    private void OnDestroy()
    {
        IdleModle.SetActive(false);
        GameManager.CurEnemies.Remove(gameObject.transform.parent.gameObject);
        GameManager.MovedEnemiesAmount++;

        int randomItemPick = Random.Range(0, Drops.Length - 1);
        int chance = Random.Range(1, Drops[randomItemPick].RarityValue);
        if (chance == 1)
        {
            InventoryUI.Instance.PlayerInventory.Add(Drops[randomItemPick]);
            GameManager.Instance.DisplayMessage($"You obtained {Drops[randomItemPick].itemName}!", false, 1);
        }
    }


    public void DoStartTurn(int num)
    {
        StartCoroutine(StartTurn(num));
    }

    IEnumerator StartTurn(int num)
    {
        //Debug.LogWarning("TURN STARTED");
        yield return new WaitForSeconds(1+num);
        GameManager.doShootControls = true;
        float RandomTime;

        switch (type)
        {
            case 0: //Queen
                MoveAmount = Random.Range(0, 6);

                while (MoveAmount > 0)
                {
                    //Debug.Log($"Moving in index {MoveAmount}");
                    int RandomPosition;
                    do
                    {
                        RandomPosition = Random.Range(0, 4);
                    } while (RandomPosition == prev_Point);
                    prev_Point = RandomPosition;
                    RandomTime = Random.Range(0.5f * (10/(9+ (float)statsSystem.Speed)), 1 * (float)statsSystem.Speed);
                    MoveTo(RandomPosition, RandomTime, false);
                    yield return new WaitForSeconds(RandomTime);
                    MoveAmount--;
                }

                MoveTo(5, 0.5f * (10 / (9 + (float)statsSystem.Speed)), true);
                aud.PlaySound(aud.SoundFX, aud.s_AttackAudCue); // audio que
                yield return new WaitForSeconds(0.5f);
                int critChance = Random.Range(1, 101);
                if (critChance <= statsSystem.CritRate)
                {
                    Debug.Log("Queen Critical Hit!!!");
                    float trueDamage = (statsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage*2, true);
                    LVLText.gameObject.SetActive(true);
                }
                else
                {
                    float trueDamage = (statsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage, false);
                    LVLText.gameObject.SetActive(true);
                }
                FinalMove();
                break;



            case 1: //Pawn
                RandomTime = Random.Range(0.25f * (10 / (9 + (float)statsSystem.Speed)), 0.5f * (10 / (9 + (float)statsSystem.Speed)));
                MoveTo(5, RandomTime, true);
                aud.PlaySound(aud.SoundFX, aud.s_AttackAudCue); // audio que
                yield return new WaitForSeconds(RandomTime);
                int critChancePawn = Random.Range(1, 101);
                if (critChancePawn <= statsSystem.CritRate)
                {
                    Debug.Log("Pawn Critical Hit!!!");
                    float trueDamage = (statsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage * 2, true);
                    LVLText.gameObject.SetActive(true);
                }
                else
                {
                    float trueDamage = (statsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage, false);
                    LVLText.gameObject.SetActive(true);
                }
                FinalMove();
                break;



            case 2: //Knight
                MoveAmount = Random.Range(0, 2);

                while (MoveAmount > 0)
                {

                    int RandomPosition;
                    do
                    {
                        RandomPosition = Random.Range(0, 4);
                    } while (RandomPosition == prev_Point);
                    prev_Point = RandomPosition;

                    RandomTime = Random.Range(0.5f * (10 / (9 + (float)statsSystem.Speed)), 1* (10 / (9 + (float)statsSystem.Speed)));
                    MoveTo(RandomPosition, RandomTime, false);
                    transform.LeanMoveY(-4, RandomTime / 2).setEaseOutQuart().setOnComplete(() =>
                    {
                        transform.LeanMoveY(-7.5f, RandomTime / 2).setEaseInQuart(); //Jumping
                    });
                    yield return new WaitForSeconds(RandomTime);
                    MoveAmount--;
                }

                MoveTo(5, 0.75f * (10 / (9 + (float)statsSystem.Speed)), true);
                transform.LeanMoveY(-4, 0.5f / 2).setEaseOutQuart().setOnComplete(() =>
                {
                    transform.LeanMoveY(-7.5f, 0.5f / 2).setEaseInQuart(); //Jumping
                });
                aud.PlaySound(aud.SoundFX, aud.s_AttackAudCue); // audio que
                yield return new WaitForSeconds(0.5f);
                critChance = Random.Range(1, 101);
                if (critChance <= statsSystem.CritRate)
                {
                    Debug.Log("Queen Critical Hit!!!");
                    float trueDamage = (statsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage * 2, true);
                    LVLText.gameObject.SetActive(true);
                }
                else
                {
                    float trueDamage = (statsSystem.ATK * (2 + level / 3)) * (PlayerPrefs.GetInt("LVL", 2) * 0.5f);
                    GameManager.DamagePlayer((int)trueDamage, false);
                    LVLText.gameObject.SetActive(true);
                }
                FinalMove();
                break;
        }
    }

    void MoveTo(int i, float time, bool finalMove)
    {
        if (Movepoint[i] == null) return;
        float finalX = Movepoint[i].transform.position.x + XOffset + (i == 5 ? 1 : 0);
        float finalZ = Movepoint[i].transform.position.z;

        LeanTweenType ease = finalMove ? LeanTweenType.easeOutCirc : eases[Random.Range(0, eases.Length)];

        transform.LeanMoveX(finalX, time).setEase(ease);
        transform.LeanMoveZ(finalZ, time).setEase(ease).setOnComplete(() =>
        {
            aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
        });
    }

    private readonly LeanTweenType[] eases = {
    LeanTweenType.easeOutElastic,
    LeanTweenType.easeInOutSine,
    LeanTweenType.easeOutQuint,
    LeanTweenType.easeOutCirc
};

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
