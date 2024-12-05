using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobRotates : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
    }
}