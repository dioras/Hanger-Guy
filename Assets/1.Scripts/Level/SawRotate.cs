using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawRotate : MonoBehaviour
{
    [SerializeField] private float Speed;


    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.forward * Time.deltaTime* Speed);
    }
}
