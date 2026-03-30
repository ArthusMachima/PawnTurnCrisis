using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    public string Name;




    private void OnMouseEnter()
    {
        Debug.Log("Entered " + Name);
    }

    private void OnMouseDown()
    {
        Debug.Log("Pressed " + Name);
    }

    private void OnMouseExit()
    {
        Debug.Log("Exited " + Name);
    }
}
