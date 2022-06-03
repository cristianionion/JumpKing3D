using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.name =="castle_guard_01")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.name =="castle_guard_01")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
