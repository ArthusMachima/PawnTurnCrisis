using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegotiateSystem : MonoBehaviour
{
    
    //Singleton
    public static NegotiateSystem inst;
    private void OnEnable()
    {
        inst = this;
    }


    //public bool ShowNegotiatePanel;

    private void Update()
    {
        //if (!ShowNegotiatePanel) return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(3))
        {
            GameManager.Instance.NegotiateMode(false);
        }

        //Confirm
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
        {
            //GameManager.Instance.DisplayMessage("Feature unfinished, may be added someday...", false, 2);
        }

        //Up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0f)
        {

        }

        //Down
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {

        }
    }
}
