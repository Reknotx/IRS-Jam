using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : SingletonPattern<CharacterMover>
{
    [Range(5, 10)]
    public float speed = 10f;

    public Transform holdArea;
    Transform playerTrans;
    
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

        //parentTrans.position += direction * speed * Time.deltaTime;

        playerRB.MovePosition(playerTrans.position + (direction * speed * Time.deltaTime));
    }

    public void GrabItem()
    {
        desiredGrab = null;
        grabArea.GetComponent<BoxCollider>().enabled = true;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, 1000f, 1 << 9);

        if (hit.collider == null) return;

        desiredGrab = hit.collider.gameObject;
        
        //Debug.Log("Want to grab " + desiredGrab.name);
    }

    public void ThrowItem()
    {
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        
        grabbedObject.transform.parent = null;
        
        grabbedObject = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == desiredGrab)
        {
            other.gameObject.transform.parent = holdArea;
            
            other.gameObject.transform.localPosition = Vector3.zero;
            
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            
            grabbedObject = other.gameObject;
            
            grabArea.GetComponent<BoxCollider>().enabled = false;
            
            desiredGrab = null;
        }
    }
}
