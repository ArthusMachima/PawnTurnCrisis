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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.Instance.DisplayMessage("Feature unfinished, may be added someday...", false, 2);
        }
    }
}
