/*
 * Author: Chase O'Connor
 * Date: 1/29/2020
 */

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
