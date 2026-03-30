
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement3DControler : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private Rigidbody player;
    [SerializeField] private bool AutoAddRigidBody;
    [SerializeField] private bool FaceObjectToDirections;
    [SerializeField] private bool LockMovementToCam;
    [SerializeField] private Vector3 RotationOffset;
    [Header("Axis")]
    [SerializeField] private bool XAxis=true;
    [SerializeField] private bool YAxis=true;
    [SerializeField] private bool ZAxis=true;
    [SerializeField] private bool LockRotation;
    [Header("Movement")]
    [SerializeField] private float Speed = 4;
    [SerializeField] private float RunPower = 2;
    [SerializeField] private float JumpPower = 5;

    private Vector3 moveDirection;
    private Transform MovePoint;

    

    private bool moving;
    private bool jumpInput;
    private bool OnGround;
    private GameObject Pivot;

    void Start()
    {
        //Rigid Body
        if (player == null)
        {
            if (AutoAddRigidBody)
            {
                gameObject.AddComponent<Rigidbody>();
                player = GetComponent<Rigidbody>();
                player.drag = 5;
                player.angularDrag = 10;
                player.interpolation = RigidbodyInterpolation.Interpolate;
                player.collisionDetectionMode = CollisionDetectionMode.Continuous;
                player.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                player.useGravity = false;
                player.isKinematic = false;

                if (LockRotation)
                {
                    player.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }
            } else
            {
                player = GetComponent<Rigidbody>();
                if (player == null)
                {
                    Debug.LogError("Movement3DControler: No Rigidbody assigned and AutoAddRigidBody is false. Please assign a Rigidbody or enable AutoAddRigidBody.");
                }
            }
        }

        //Movepoint
        Pivot = new("Pivot");
        Pivot.transform.SetParent(transform, false);
        if (LockMovementToCam)
        {
            Pivot.AddComponent<Billboard>();
            Pivot.GetComponent<Billboard>().DisableY=true;
        }

        GameObject Point = new("MovePoint");
        Point.transform.SetParent(Pivot.transform, false);
        MovePoint = Point.transform;

    }


    private void Update()
    {
        // MOVEMENT

        //Gamepad Input
        if (Gamepad.current != null)
        {
            MovePoint.localPosition = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Movement Detection and Running
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                moving = true;
            }
            else
            {
                moving = false;
            }
        }

        //Keyboard Input
        if (Input.GetKey(KeyCode.W)&&ZAxis) { MovePoint.localPosition = new(    MovePoint.localPosition.x, MovePoint.localPosition.y,  1); moving = true; }
        if (Input.GetKey(KeyCode.S)&&ZAxis) { MovePoint.localPosition = new(    MovePoint.localPosition.x, MovePoint.localPosition.y, -1); moving = true; }
        if (Input.GetKey(KeyCode.D)&&XAxis) { MovePoint.localPosition = new( 1, MovePoint.localPosition.y, MovePoint.localPosition.z    ); moving = true; }
        if (Input.GetKey(KeyCode.A)&&XAxis) { MovePoint.localPosition = new(-1, MovePoint.localPosition.y, MovePoint.localPosition.z    ); moving = true; }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D)|| Input.GetKeyUp(KeyCode.S)|| Input.GetKeyUp(KeyCode.A))
        {
            MovePoint.localPosition = Vector3.zero;
            moving = false;
        }

        //Relating applied object's movement to camera
        moveDirection = Pivot.transform.TransformDirection(new Vector3(MovePoint.localPosition.x, MovePoint.localPosition.y, MovePoint.localPosition.z));

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && YAxis)
        {
            jumpInput = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && YAxis)
        {
            jumpInput = false;
        }
    }

    //For continuous physics updates
    void FixedUpdate()
    {
        if (moving)
        {
            //Running
            if (Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.LeftControl))
                player.AddForce(moveDirection.normalized * (Speed + RunPower), ForceMode.VelocityChange);

            //Walking
            else
                player.AddForce(moveDirection.normalized * Speed, ForceMode.VelocityChange);


            //Rotation
            if (FaceObjectToDirections)
            {
                transform.rotation = Quaternion.Slerp(
                    Quaternion.Euler(RotationOffset),
                    Quaternion.LookRotation(moveDirection),
                    Time.deltaTime * 7f
                );
                RotationOffset = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0, RotationOffset.y, 0);
            }
        }
        else
        {
            if (FaceObjectToDirections)
            {
                
            }
        }

        //Jump Physics
        if (jumpInput && OnGround)
        {
            player.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            jumpInput = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        OnGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        OnGround = false;
    }
}
