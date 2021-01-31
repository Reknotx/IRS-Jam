using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : SingletonPattern<CharacterMover>
{
    [Range(5, 10)]
    public float speed = 10f;


    public Transform holdArea;
    
    Transform playerTrans;

    private int grabbableMask = (1 << 9) | (1 << 10);
    
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
        if (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.A))
        {
            Move();
        }
    }

    private void Update()
    {
        Rotate();

        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
                GrabItem();
            else
                ThrowItem();
        }

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

        playerRB.MovePosition(playerTrans.position + (direction * speed * Time.deltaTime));
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
