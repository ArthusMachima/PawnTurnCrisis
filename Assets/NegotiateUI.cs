using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegotiateUI : MonoBehaviour
{
    
    //Singleton
    public static NegotiateUI Instance;
    private void OnEnable()
    {
        Instance = this;
    }


    public bool ShowNegotiatePanel;

    private void Update()
    {
        if (!ShowNegotiatePanel) return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(3))
        {
            GameManager.Instance.NegotiateMode(false);
        }
    }
}
