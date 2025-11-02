using UnityEngine;
using UnityEngine.Serialization;

public class MovementSc : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private RotateSC rotateSC;
    
    [Header("Movement Values")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody rb;

    [Header("Check on Board")] [SerializeField]
    private float zOffset;
    [SerializeField] private float xOffset;
    [SerializeField] private bool zHasPosSpace = true, zHasNegSpace = true;
    [FormerlySerializedAs("xHasSpace")] [SerializeField] private bool xHasPosSpace = true;
    [SerializeField] private bool xHasNegSpace = true;
    [SerializeField] private float range;
    [SerializeField] private Vector2 direection;

    public Vector3 rotationVector;
    public Vector3 movementVector;
    void FixedUpdate()
    {
        MovePlayer(inputController.movement);
    }

    void Update()
    {
        zHasPosSpace = CheckPositiveZ();
        zHasNegSpace = CheckNegativeZ();
        xHasPosSpace = CheckPositiveX();
        xHasNegSpace = CheckNegativeX();
    }

    private void MovePlayer(Vector2 movement)
    {
        Vector3 adjustVector = new Vector3(movement.x, 0, movement.y);
       rotationVector = Quaternion.AngleAxis(rotateSC.newAngle, UnityEngine.Vector3.up) * adjustVector.normalized;
       movementVector = rotationVector;
        // if (rotateSC.rotating)
        // {
        //    movementVector = Quaternion.AngleAxis(rotateSC.newAngle, UnityEngine.Vector3.up) * movement;;
        // }
        // else
        // {
        //     movementVector = movement.normalized;
        // }
         direection = new Vector2(movementVector.x, movementVector.z).normalized;
        if (direection.y > 0)
        {
            
            if (!zHasPosSpace)
            {
                Debug.Log("PositiveZ");
                direection.y = 0;
             //   return;
            }
        }
        else if (direection.y < 0)
        {
            if (!zHasNegSpace)
            {
                direection.y = 0;
                Debug.Log("NegativeZ");
             //   return;
            }
        }

        if (direection.x > 0)
        {
            Debug.Log(CheckPositiveX());
            if (!xHasPosSpace)
            {
                direection.x = 0;
                Debug.Log("PositiveX");
             //   return;
            }
        }
        else if (direection.x < 0)
        {
            if (!xHasNegSpace)
            {
                direection.x = 0;
                Debug.Log("NegativeX");
               // return;
            }
        }
        Debug.DrawRay(transform.position, movementVector * movementSpeed, Color.red);
        Debug.DrawRay(transform.position, direection * movementSpeed, Color.green);
        Vector3 newPos = new Vector3(direection.x* movementSpeed * Time.fixedDeltaTime, transform.position.y, direection.y*movementSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + newPos);
       // rb.linearVelocity = new Vector3(direection.x* Vector3.right.x* movementSpeed * Time.fixedDeltaTime, rb.linearVelocity.y, direection.y* Vector3.forward.z*movementSpeed * Time.fixedDeltaTime);
       
    }

    private bool CheckPositiveX()
    {
        Vector3 positiveX = new Vector3(rb.transform.position.x + xOffset, rb.transform.position.y,
            rb.transform.position.z);
        
        RaycastHit positiveXHit;
        bool positive = Physics.Raycast(positiveX, Vector3.down, out positiveXHit, range);
        Debug.DrawRay(positiveX, Vector3.down * range, Color.red);

        if (positive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckNegativeX()
    {
        Vector3 negativeX = new Vector3(rb.transform.position.x - xOffset, rb.transform.position.y,
            rb.transform.position.z);
        
        RaycastHit negativeXHit;
        bool negative = Physics.Raycast(negativeX, Vector3.down, out negativeXHit, range);
        Debug.DrawRay(negativeX, Vector3.down * range, Color.blue);

        if (negative)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckPositiveZ()
    {
        Vector3 positiveZ = new Vector3(rb.transform.position.x, rb.transform.position.y,rb.transform.position.z + zOffset);
        
        RaycastHit positiveZHit;
        bool positive = Physics.Raycast(positiveZ, Vector3.down, out positiveZHit, range);
        
        Debug.DrawRay(positiveZ, Vector3.down * range, Color.green);

        if (positive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckNegativeZ()
    {
        Vector3 negativeZ = new Vector3(rb.transform.position.x, rb.transform.position.y,rb.transform.position.z - zOffset);
        
        RaycastHit negativeZHit;
        bool negative = Physics.Raycast(negativeZ, Vector3.down, out negativeZHit, range);
        Debug.DrawRay(negativeZ, Vector3.down * range, Color.yellow);
        if (negative)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
