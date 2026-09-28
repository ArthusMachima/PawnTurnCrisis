
using UnityEngine;

public class ShootWithMouse : MonoBehaviour
{
    [SerializeField] private bool DoLog = false;
    [SerializeField] private Vector3 ShootLocation;
    [Header("Bullet Damage")]
    [SerializeField] private bool doDamage;
    public bool doShoot;
    [Header("Bullet Effect")]
    [SerializeField] private GameObject BulletHitEffect;


    public void Shoot()
    {
        if (doShoot)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (BulletHitEffect != null)
                {
                    Instantiate(BulletHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
                if (DoLog) Debug.Log("Shot " + hit.transform.name);
                if (doDamage)
                {
                    int critChance = Random.Range(0, 101);
                    if (critChance <= GameManager.Instance.PlayerStats.CritRate)
                    {
                        SendDamage(hit.transform.gameObject, GameManager.Instance.PlayerStats.ATK * 2, true);
                    }
                    else
                    {
                        SendDamage(hit.transform.gameObject, GameManager.Instance.PlayerStats.ATK, false);
                    }
                }
            }
        }
    }

    private void SendDamage(GameObject obj, int dmg, bool crit)
    {
        try
        {
            if (!obj.TryGetComponent<StatsSystem>(out var stats)) return;
            if (crit) GameManager.Instance.DoParryEffect();
            stats.TakeDamage(dmg);
        }
        catch
        {
            if (DoLog) Debug.Log("No Health Component found on " + obj.name);
        }
    }

    /*
    private IEnumerator TimedDeath(GameObject obj, Coroutine coroutine)
    {
        yield return new WaitForSeconds(lifespan);
        StopCoroutine(coroutine);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    private IEnumerator DetectObject(GameObject obj)
    {
        if (doDamage)
        {
            do
            {
                if (obj.GetComponent<CollisionDetection>().collided)
                {
                    SendDamage(obj.GetComponent<CollisionDetection>().DetectedObject, GameManager.Instance.PlayerStats.ATK);
                }
                yield return null;
            }
            while (obj.GetComponent<CollisionDetection>().DetectedObject == null);
        }
    }
    */
}
