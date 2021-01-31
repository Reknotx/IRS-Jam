using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : SingletonPattern<CharacterMover>
{
    [Range(5, 10)]
    public float speed = 10f;


    public Transform holdArea;
    
    Transform playerTrans;

    public Animator playerAnim;

    private int grabbableMask = (1 << 9) | (1 << 10) | (1 << 17);
    
    [Range(500, 1500)]
    public int forceModifier = 500;

    public GameObject grabArea, desiredGrab, grabbedObject;

    private Rigidbody playerRB;

    protected override void Awake()
    {
        base.Awake();
        playerTrans = transform.parent;
        playerRB = transform.parent.GetComponent<Rigidbody>();

        if (desiredGrab != null) desiredGrab = null;
        if (grabbedObject != null) grabbedObject = null;
    }

    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.W)
        //    || Input.GetKey(KeyCode.S)
        //    || Input.GetKey(KeyCode.D)
        //    || Input.GetKey(KeyCode.A))
        //{
            Move();
        //}
    }

    private void Update()
    {
        Rotate();

        if (grabbedObject == null && Input.GetMouseButtonDown(0))
            GrabItem();
        else if (grabbedObject != null && Input.GetMouseButtonUp(0))
            ThrowItem();

        if (grabbedObject != null)
        {
            grabbedObject.transform.localPosition = Vector3.zero;
        }
    }

    public void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, 1000f, 1 << 8);

        if (hit.collider == null) return;

        transform.LookAt(new Vector3(hit.point.x, 1f, hit.point.z));
    }

    public void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (direction != Vector3.zero)
        {
            playerAnim.SetBool("IsRunning", true);

            playerAnim.SetBool("IsRunningForward", false);
            playerAnim.SetBool("IsRunningRight", false);
            playerAnim.SetBool("IsRunningBack", false);
            playerAnim.SetBool("IsRunningLeft", false);

            if (Input.GetAxis("Vertical") > 0f)
            {
                ///Running forward
                switch (GetFacingDirection())
                {
                    case Dir.forward:
                        playerAnim.SetBool("IsRunningForward", true);
                        break;

                    case Dir.right:
                        playerAnim.SetBool("IsRunningLeft", true);
                        break;

                    case Dir.back:
                        playerAnim.SetBool("IsRunningBack", true);
                        break;

                    case Dir.left:
                        playerAnim.SetBool("IsRunningRight", true);
                        break;

                    default:
                        break;
                }
            }
            else if (Input.GetAxis("Vertical") < 0f)
            {
                ///Running back
                switch (GetFacingDirection())
                {
                    case Dir.forward:
                        playerAnim.SetBool("IsRunningBack", true);
                        break;

                    case Dir.right:
                        playerAnim.SetBool("IsRunningRight", true);
                        break;

                    case Dir.back:
                        playerAnim.SetBool("IsRunningForward", true);
                        break;

                    case Dir.left:
                        playerAnim.SetBool("IsRunningLeft", true);
                        break;

                    default:
                        break;
                }

            }
            //else
            //{
            //    playerAnim.SetBool("IsRunningForward", false);
            //    playerAnim.SetBool("IsRunningBack", false);
            //}

            if (Input.GetAxis("Horizontal") > 0f)
            {
                ///Running right
                switch (GetFacingDirection())
                {
                    case Dir.forward:
                        playerAnim.SetBool("IsRunningRight", true);
                        break;

                    case Dir.right:
                        playerAnim.SetBool("IsRunningForward", true);
                        break;

                    case Dir.back:
                        playerAnim.SetBool("IsRunningLeft", true);
                        break;

                    case Dir.left:
                        playerAnim.SetBool("IsRunningBack", true);
                        break;

                    default:
                        break;
                }
            }
            else if (Input.GetAxis("Horizontal") < 0f)
            {
                ///Running left
                switch (GetFacingDirection())
                {
                    case Dir.forward:
                        playerAnim.SetBool("IsRunningLeft", true);
                        break;

                    case Dir.right:
                        playerAnim.SetBool("IsRunningBack", true);
                        break;

                    case Dir.back:
                        playerAnim.SetBool("IsRunningRight", true);
                        break;

                    case Dir.left:
                        playerAnim.SetBool("IsRunningForward", true);
                        break;

                    default:
                        break;
                }
            }


        }
        else
        {
            playerAnim.SetBool("IsRunning", false);
            playerAnim.SetBool("IsRunningForward", false);
            playerAnim.SetBool("IsRunningRight", false);
            playerAnim.SetBool("IsRunningBack", false);
            playerAnim.SetBool("IsRunningLeft", false);
        }


        playerRB.MovePosition(playerTrans.position + (direction * speed * Time.deltaTime));
    }

    enum Dir 
    { 
        /// <summary> Facing in the forward direction </summary>
        forward, 

        /// <summary> Facing in the right direction </summary>
        right, 

        /// <summary> Facing in the down direction. </summary>
        back, 

        /// <summary> Facing in the left direction. </summary>
        left 
    }

    private Dir GetFacingDirection()
    {
        float rotation = transform.localRotation.eulerAngles.y;

        if (rotation < 0f)
        {
            rotation += 360;
        }
        Debug.Log(rotation);

        if (rotation > 45 && rotation < 135)
        {
            ///Facing right
            Debug.Log("Facing right");
            return Dir.right;
        }
        else if (rotation <= 45 || rotation >= 315)
        {
            ///Facing Forward
            Debug.Log("Facing forward");
            return Dir.forward;
        }
        else if (rotation >= 135 && rotation <= 225)
        {
            ///Facing down
            Debug.Log("Facing back");
            return Dir.back;
        }
        else
        {
            ///Facing left
            Debug.Log("Facing left");
            return Dir.left;
        }

    }

    public void GrabItem()
    {
        desiredGrab = null;
        grabArea.GetComponent<BoxCollider>().enabled = true;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, 1000f, grabbableMask);

        if (hit.collider == null) return;

        desiredGrab = hit.collider.gameObject;

        StartCoroutine(GrabDelay());

        //Debug.Log("Want to grab " + desiredGrab.name);
    }

    public void ThrowItem()
    {
        Rigidbody grabbedRB = grabbedObject.GetComponent<Rigidbody>();
        grabbedRB.useGravity = true;
        grabbedRB.isKinematic = false;
        grabbedRB.drag = 0;
        grabbedRB.angularDrag = 0.05f;
        grabbedRB.AddForce(transform.forward * forceModifier);

        if (grabbedObject.GetComponent<BreakableObj>() != null)
        {
            grabbedObject.GetComponent<BreakableObj>().enabled = true;
            grabbedObject.GetComponent<BreakableObj>().Thrown = true;
        }

        grabbedObject.transform.parent = null;
        grabbedObject.layer = 9;

        grabbedObject = null;
    }

    public void ExamineCollision(Collider other)
    {
        if (desiredGrab == null) return;

        if (other.gameObject == desiredGrab)
        {
            //Debug.Log("Grabbing " + other.name);
            Rigidbody grabbedRB = other.gameObject.GetComponent<Rigidbody>();

            grabbedRB.useGravity = false;
            //other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedRB.drag = Mathf.Infinity;
            grabbedRB.angularDrag = Mathf.Infinity;
            
            grabbedObject = other.gameObject;

            grabbedObject.transform.parent = holdArea;
            
            grabbedObject.transform.localPosition = Vector3.zero;
            
            if (grabbedObject.GetComponent<Page>() != null)
            {
                grabbedObject.GetComponent<Page>().Collect();
                grabbedObject = null;
            }
            else
            {
                grabbedObject.layer = 11;
                foreach (Transform child in grabbedObject.transform)
                {
                    child.gameObject.layer = 11;
                }
            }
            
            grabArea.GetComponent<BoxCollider>().enabled = false;
            desiredGrab = null;
        }
    }

    IEnumerator GrabDelay()
    {
        yield return new WaitForSeconds(.2f);
        desiredGrab = null;
    }
}
