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
    public int DEF=1;
    public int ATK=100;
    public int ElemATK=100;
    public int CritRate=10;
    public int Speed=1;

    [Header("Reference")]
    [SerializeField] private GameObject DeathParticle;
    [SerializeField] private Material DamageMatFX;


    [Header("Properties")]
    [SerializeField] private bool DoDamage = true;
    [SerializeField] private bool DoDeath = true;
    [SerializeField] private bool IndicateDamage;
    [SerializeField] private bool DoDamageEffect = true;


    [Header("State")]
    public bool Damaged;

    [Header("Debug")]
    [SerializeField] int TestDamage=100;
    [SerializeField] private bool DamageEntity;

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
        if (HPNum != null) HPNum.text = "HP: " + HP.ToString() + "/" + MaxHP.ToString();
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
        int actualDamage;

        if (DEF == 0)
        {
            // Special case: No defense modifier, use base multiplier.
            actualDamage = (int)(damage * 1.2f);
        }
        else
        {
            float multiplier;
            if (DEF > 0)
            {
                // Positive DEF: Use ratio formula for guaranteed reduction (R1).
                multiplier = (1.2f / (1.0f + DEF * 0.01f));
            }
            else // DEF < 0
            {
                // Negative DEF: Keep original amplification formula, which works well here.
                multiplier = (-0.1f * DEF) + 1.2f;
            }

            actualDamage = (int)(damage * multiplier);
        }

        // FIX R2: Ensure minimum damage is always 10, regardless of calculation result.
        actualDamage = Mathf.Max(10, actualDamage);




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
}
