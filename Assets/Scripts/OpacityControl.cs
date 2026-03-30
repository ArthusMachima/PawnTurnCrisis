using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityControl : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private CanvasGroup[] AppliedObject;
    public int index;
    [SerializeField] private float Duration=0.5f;
    public float OpacityA = 10;
    public float OpacityB = 0;
    [Header("Debug")]
    [SerializeField] private bool TriggerOpacityA;
    [SerializeField] private bool TriggerOpacityB;
    [SerializeField] private bool TriggerLoopOpacity;
    [SerializeField] private bool TriggerKillLoop;

    private bool isOpacityB;
    private Coroutine OpacityLoop;

    private void Update()
    {
        if (TriggerOpacityA)
        {
            ToOpacityA();
            TriggerOpacityA = false;
        }

        if (TriggerOpacityB)
        {
            ToOpacityB();
            TriggerOpacityB = false;
        }

        if (TriggerLoopOpacity)
        {
            OpacityLoop = StartCoroutine(LoopOpacity());
            TriggerLoopOpacity = false;
        }

        if (TriggerKillLoop)
        {
            StopCoroutine(OpacityLoop);
            TriggerKillLoop = false;
        }
    }

    public void ToOpacityA()
    {
        if (isOpacityB)
        {
            AppliedObject[index].LeanAlpha(OpacityA / 10, Duration);
            isOpacityB = false;
        }
    }

    public void ToOpacityB()
    {
        if (!isOpacityB)
        {
            AppliedObject[index].LeanAlpha(OpacityB/10, Duration);
            isOpacityB = true;
        }
    }

    public IEnumerator LoopOpacity()
    {
        while (true)
        {
            if (isOpacityB)
            {
                AppliedObject[index].LeanAlpha(OpacityA / 10, Duration);
                isOpacityB = false;
            }
            else
            {
                AppliedObject[index].LeanAlpha(OpacityB / 10, Duration);
                isOpacityB = true;
            }
            yield return new WaitForSeconds(Duration);
        }
    }
}
