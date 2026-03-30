using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackBehavior : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 centerPivot;

    
    private void OnEnable()
    {
        AudioManager aud = FindAnyObjectByType<AudioManager>();

        aud.PlaySound(aud.SoundFX, aud.s_EnemyStep);
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        LeanTween.cancel(gameObject);
        //Shake effect
        float randomShakeX = Random.Range(-intensity, intensity);
        float randomShakeY = Random.Range(intensity, intensity);

        gameObject.LeanMoveLocal(new Vector3(centerPivot.x, centerPivot.y, 0), duration).setOnComplete(()=>{
            gameObject.LeanMoveLocal(new Vector3(randomShakeX + centerPivot.x, randomShakeY + centerPivot.y, 0), duration).setOnComplete(() =>
            {
                float randomShakeX = Random.Range(-intensity, intensity);
                float randomShakeY = Random.Range(-intensity, intensity);
                gameObject.LeanMoveLocal(new Vector3(randomShakeX + centerPivot.x, randomShakeY + centerPivot.y, 0), duration).setOnComplete(() =>
                {
                    gameObject.LeanMoveLocal(new Vector3(centerPivot.x, centerPivot.y, 0), duration).setOnComplete(() =>
                    {
                        //Fade out effect
                        LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0f, 0.5f).setOnComplete(() =>
                        {
                            gameObject.SetActive(false);
                        });
                    });
                });
            });
        });
    }
    
}
