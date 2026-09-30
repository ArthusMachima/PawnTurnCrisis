using System;
using UnityEngine;

public class Util_BarVisualizer : MonoBehaviour
{
    [SerializeField] Transform Bar;
    [SerializeField] bool intMode;
    [SerializeField] float MaxValue=1000;
    [Range(0, 1000)] [SerializeField] float CurrentValue;

    void Update()
    {
        UpdateBar();
    }

    private void Start()
    {
        SetCurrent(MaxValue);
        UpdateBar();
    }

    public void SetMax(float value)
    {
        MaxValue=value;
        if (CurrentValue>MaxValue) CurrentValue = MaxValue;
        UpdateBar();
    }

    public void SetCurrent(float value)
    {
        CurrentValue=value;
        UpdateBar();
    }
    
    void UpdateBar()
    {
        if (intMode) Bar.localScale = new((int)(CurrentValue/MaxValue), (int)Bar.localScale.y, (int)Bar.localScale.z);
        else Bar.localScale = new(CurrentValue / MaxValue, Bar.localScale.y, Bar.localScale.z);
    }
}
