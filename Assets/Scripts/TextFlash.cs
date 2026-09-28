
using TMPro;
using UnityEngine;

public class TextFlash : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] GameObject prefab;
    [SerializeField] private float Size = 4.0f;
    [SerializeField] private float moveAmount = 1;
    [SerializeField] private float lifespan = 0.5f;
    [SerializeField] private float animDur = 0.5f;
    [SerializeField] private Vector3 offset;
    [Header("Debug")]
    [SerializeField] string TestText = "Text";
    [SerializeField] bool TestTextFlash;

    private void Update()
    {
        if (TestTextFlash) { FlashText(TestText); TestTextFlash = false; }
    }


    public void FlashText(string text)
    {
        if (prefab == null) return;
        GameObject textObj = Instantiate(prefab, transform.position+offset, Quaternion.identity);
        TextMeshProUGUI textMesh = textObj.GetComponent<TextMeshProUGUI>();
        textMesh.fontSize = Size/10;
        textMesh.text = text;
        textObj.transform.LeanMoveY(textObj.transform.position.y + moveAmount, animDur).setEaseOutQuad();
        textObj.LeanDelayedCall(lifespan, () => Destroy(textObj));
    }

}