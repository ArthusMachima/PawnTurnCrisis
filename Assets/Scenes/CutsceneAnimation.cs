using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneAnimation : MonoBehaviour
{
    [SerializeField] private float delay;

    private void Start()
    {
        StartCoroutine(Girly());
    }

    IEnumerator Girly()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Gameplay");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
