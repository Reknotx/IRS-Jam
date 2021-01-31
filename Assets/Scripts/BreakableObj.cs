using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObj : MonoBehaviour
{
    public List<GameObject> pieces = new List<GameObject>();
    private Rigidbody parentRB;

    [Range(0.5f, 5f)]
    public float velocityToSmash = 1f;

    public bool Thrown { get; set; } = false;
    private bool breakOnCollide = false;

    private void Awake()
    {
        parentRB = GetComponent<Rigidbody>();

        foreach (Transform child in gameObject.transform)
        {
            pieces.Add(child.gameObject);
            child.GetComponent<Rigidbody>().isKinematic = true;
            
            //if (child.GetComponent<Collider>() != null)
            //{
            //    child.GetComponent<Collider>().enabled = false;
            //}
        }

        enabled = false;
    }

    private void Update()
    {
        if (!Thrown) return;

        if (parentRB.velocity.magnitude > velocityToSmash) breakOnCollide = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12) return;

        //Debug.Log(gameObject.name + " collided with: " + collision.gameObject.name + "\nWith a velocity of: " + parentRB.velocity.magnitude);

        if (breakOnCollide)
        {
            foreach (GameObject piece in pieces)
            {
                piece.transform.parent = null;
                Collider[] colliders = piece.GetComponents<Collider>();

                foreach (Collider collider in colliders)
                {
                    collider.enabled = true;
                }
                piece.layer = 9;

                piece.GetComponent<Rigidbody>().useGravity = true;
                piece.GetComponent<Rigidbody>().isKinematic = false;
            }

            Destroy(gameObject);
        }
        else
        {
            this.enabled = false;
        }
    }
}
