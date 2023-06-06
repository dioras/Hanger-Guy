using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Target
    {
        get => this.target;
        set
        {
            this.target = value;
            
            this.targetRigidbody = this.target.GetComponent<Rigidbody>();
        }
    }

    private Transform target;
    
    
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float minOffsetX;
    public float minOffsetZ;
    public float offsetXSpeedOut = 1f;
    public float offsetZSpeedOut = 5f;
    public float minSpeedForOffset = 3f;
    private Coroutine changeOffsetCor;
    public Vector3 StartOffset { get; private set; }
    public float StartCameraPosZ = -10f;

    private Rigidbody targetRigidbody;



    private void OnEnable()
    {
        if (StartOffset != Vector3.zero)
        {
            this.offset = StartOffset;
        }
    }

    private void Start()
    {
        StartOffset = this.offset;
    }

    private void FixedUpdate()
    {
        if (ReferenceEquals(Target, null))
        {
            return;
        }
    
        if (this.changeOffsetCor == null)
        {
            if (this.targetRigidbody != null)
            {
                if (this.targetRigidbody.velocity.magnitude > minSpeedForOffset)
                {
                    if (offset.x < minOffsetX)
                    {
                        offset.x += Time.deltaTime;
                    }
                    else
                    {
                        offset.x -= Time.deltaTime;
                    }

                    if (offset.z < minOffsetZ)
                    {
                        offset.z += Time.deltaTime;
                    }
                    else
                    {
                        offset.z -= Time.deltaTime;
                    }
                }
                else if (offset.z < StartCameraPosZ)
                {
                    if (offset.x > 0)
                    {
                        offset.x -= Time.deltaTime * offsetXSpeedOut;
                    }

                    if (offset.z < -10)
                    {
                        offset.z += Time.deltaTime * offsetZSpeedOut;
                    }
                }
            }
        }

        var desPos = Target.transform.position + offset;
        var smoothPos = Vector3.Lerp(transform.position, desPos, smoothSpeed);
        transform.position = new Vector3(smoothPos.x, transform.position.y, offset.z);
    }



    public void ChangeOffsetSmoothly(Vector3 targetOffset, float time)
    {
        if (this.changeOffsetCor != null)
        {
            StopCoroutine(this.changeOffsetCor);
        }
    
        this.changeOffsetCor = StartCoroutine(ChangeOffsetSmoothlyProcess(targetOffset, time));
    }

    private IEnumerator ChangeOffsetSmoothlyProcess(Vector3 targetOffset, float time)
    {
        var startOffset = this.offset;
        var currTime = time;

        while (currTime > 0)
        {
            var lerp = 1 - currTime / time;
            
            this.offset = Vector3.Lerp(startOffset, targetOffset, lerp);
        
            currTime -= Time.deltaTime;

            yield return null;
        }

        this.changeOffsetCor = null;
    }
}
