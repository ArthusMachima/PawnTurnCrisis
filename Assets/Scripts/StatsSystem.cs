using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsSystem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private GameObject HPBar;
    [SerializeField] private TextMeshProUGUI HPNum;
    public int MaxHP = 1000;
    [Range(0, 1000)] public int HP = 1000;
    [SerializeField] int DEF=1;
    [SerializeField] int ATK=100;
    [SerializeField] int ElemATK=100;
    [SerializeField] int CritRate=10;
    [SerializeField] int Speed=1;

    [Header("Reference")]
    [SerializeField] private GameObject DeathParticle;
    [SerializeField] private Material DamageMatFX;
    [SerializeField] TextFlash textFlash;


    [Header("Properties")]
    [SerializeField] bool DoDamage = true;
    [SerializeField] bool DoDeath = true;
    [SerializeField] bool IndicateDamage;
    [SerializeField] bool DoDamageEffect = true;


    [Header("State")]
    public bool Damaged;

    [Header("Debug")]
    [SerializeField] int TestDamage=100;
    [SerializeField] bool DamageEntity;

    //Data
    private readonly List<Material> normal_mat = new();
    private List<Renderer> render = new();
    private bool IsDead;
    private Coroutine c_dmgEffect;

    // UI bar caching
    private RectTransform hpBarRect;
    private float hpBarFullWidth = 0f;
    private float hpBarFullScaleX = 1f;

    void Start()
    {
        HP = MaxHP;

        // Cache RectTransform and original width (for UI)
        if (HPBar != null)
        {
            hpBarRect = HPBar.GetComponent<RectTransform>();
            if (hpBarRect != null)
            {
                // capture the full width when at MaxHP
                hpBarFullWidth = hpBarRect.rect.width;
            }
            else
            {
                // fallback: capture original localScale.x
                hpBarFullScaleX = HPBar.transform.localScale.x;
            }
        }


        textFlash = GetComponentInChildren<TextFlash>();
    }



    public void DeductTurn()
    {

    }



    void Update()
    {
        if (DamageEntity)
        {
            TakeDamage(TestDamage);
            DamageEntity = false;
        }

        //Death Detection
        if (HP <= 0 && !IsDead && DoDeath)
        {
            Instantiate(DeathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.1f);
            IsDead = true;
        }


        //HP Bar Display
        if (HPNum != null) HPNum.text = $"HP: {HP}/{MaxHP} \n"
                                      + $"DEF: {DEF}";
        if (HPBar != null)
        {
            float proportion = MaxHP > 0 ? Mathf.Clamp01((float)HP / (float)MaxHP) : 0f;

            // If HPBar is a UI element with RectTransform, adjust width.
            if (hpBarRect != null)
            {
                float newWidth = hpBarFullWidth * proportion;
                hpBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            }
            else
            {
                // Fallback for non-UI objects: scale X relative to captured full scale
                Vector3 s = HPBar.transform.localScale;
                s.x = hpBarFullScaleX * proportion;
                HPBar.transform.localScale = s;
            }
        }
    }


    public void TakeDamage(int damage)
    {
        int actualDamage = damage - DEF;
        if (textFlash!=null) textFlash.FlashText("" + actualDamage);

        if (DoDamage)
        {
            HP -= actualDamage;
            if (DoDamageEffect)
            {
                if (c_dmgEffect != null)
                {
                    StopCoroutine(DamageEffect());
                    c_dmgEffect = null;
                }
                if (c_dmgEffect == null) StartCoroutine(DamageEffect());
            }
        }


        if (IndicateDamage)
        {
            GameObject damageText = new("DamageText");
            //damageText.transform.;
            damageText.AddComponent<Billboard>();
            damageText.AddComponent<TextMeshProUGUI>();
            damageText.GetComponent<TextMeshProUGUI>().text = actualDamage.ToString();
            damageText.AddComponent<LifeSpan>();
            damageText.GetComponent<LifeSpan>().lifespan = 1f;
            damageText.AddComponent<EasedTransform>();
            damageText.GetComponent<EasedTransform>().Location = new Vector3(0, 1, 0);
        }
    }

    public void RestoreMaterial()
    {
        //Restore original just incase of a sudden interuption
        for (int i = 0; i < render.Count; i++) render[i].material = normal_mat[i];
        render.Clear();
        normal_mat.Clear();
    }

    IEnumerator DamageEffect()
    {
        Damaged = true;
        //THIS CODE MAY BE UNOPTIMIZED

        //Restore original just incase of a sudden interuption
        for (int i = 0; i < render.Count; i++) render[i].material = normal_mat[i];
        render.Clear();
        normal_mat.Clear();

        //Initialize
        render = GetComponentsInChildren<Renderer>().ToList();
        if (normal_mat.Count < render.Count) normal_mat.AddRange(new Material[render.Count - normal_mat.Count]);

        //Store original materials for backup
        for (int i = 0; i < render.Count; i++) normal_mat[i] = render[i].material;

        //Apply damage material
        foreach (var r in render) r.material = DamageMatFX;
        yield return new WaitForSeconds(0.1f);

        //Restore original materials
        for (int i = 0; i < render.Count; i++) render[i].material = normal_mat[i];
        render.Clear();
        normal_mat.Clear();
        c_dmgEffect = null;
        Damaged = false;
    }

    public void AddHP(int amount) { HP += amount; if (HP > MaxHP) HP = MaxHP; }
    public void AddDEF(int amount) { DEF += amount; }
    public void AddATK(int amount) { ATK += amount; }
    public void AddElemATK(int amount) { ElemATK += amount; }
    public void AddCritRate(int amount) { CritRate += amount; }
    public void AddSpeed(int amount) { Speed += amount; }

    public void SetHP(int amount) { HP = amount; if (HP > MaxHP) HP = MaxHP; }
    public void SetDEF(int amount) { DEF = amount; }
    public void SetATK(int amount) { ATK = amount; }
    public void SetElemATK(int amount) { ElemATK = amount; }
    public void SetCritRate(int amount) { CritRate = amount; }
    public void SetSpeed(int amount) { Speed = amount; }

    public int GetHP() { return HP; }
    public int GetDEF() { return DEF; }
    public int GetATK() { return ATK; }
    public int GetElemATK() { return ElemATK; }
    public int GetCritRate() { return CritRate; }
    public int GetSpeed() { return Speed; }
}
