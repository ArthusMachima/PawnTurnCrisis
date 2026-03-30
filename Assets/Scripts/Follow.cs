
using UnityEngine;
using Random = UnityEngine.Random;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float FollowSpeed=10;
    public bool doFollow;
    [SerializeField] private bool followCursor;
    private float ogspeed;

    private void Start()
    {
        ogspeed = FollowSpeed;
    }

    public void SetPos(Transform pos)
    {
        target = pos;
    }

    public void SetSpeed(float speed)
    {
        FollowSpeed = speed;
    }

    public void ResetSpeed()
    {
        FollowSpeed = ogspeed;
    }

    void Update()
    {
        if (doFollow && target!=null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * FollowSpeed);
        } else if (followCursor)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gameObject.GetComponent<RectTransform>().parent as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 localPoint
        );

            gameObject.GetComponent<RectTransform>().anchoredPosition = localPoint;
        }
    }
}