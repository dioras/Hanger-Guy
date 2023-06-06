using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    [SerializeField] private float Speed;

    private void FixedUpdate()
    {
        this.transform.Rotate(Vector3.up * Time.deltaTime * Speed);
    }
}
