using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCollision : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (CharacterMover.Instance.desiredGrab == null) return;

        //Debug.Log("Collided with: " + other.name);
        CharacterMover.Instance.ExamineCollision(other);
    }
}
