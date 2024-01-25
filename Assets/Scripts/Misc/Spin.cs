using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 axis;
    
    void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(speed * Time.deltaTime, axis);
    }
}
