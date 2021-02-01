/*
 * Author: Chase O'Connor
 * Date: 1/29/2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public void Collect()
    {
        UIManager.Instance.Score++;
        Destroy(gameObject);
    }
}
