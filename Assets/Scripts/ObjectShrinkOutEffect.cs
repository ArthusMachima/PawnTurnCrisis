using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ObjectShrinkOutEffect : MonoBehaviour
{
    [SerializeField] float shrinkRate = 0.1f;
    [SerializeField] Vector3 og_size;
    [SerializeField] UnityEvent onEffectComplete;

    private void Start()
    {
        og_size = transform.localScale;
        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        transform.localScale = og_size;
        transform.LeanScale(Vector3.zero, shrinkRate).setOnComplete(() =>
        {
            onEffectComplete?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
