using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotationAroundItsAxis : MonoBehaviour
{
    public float rotatePerFrameX = 0f;
    public float rotatePerFrameY = 0f;
    public float rotatePerFrameZ = 0f;
    void FixedUpdate()
    {
        transform.Rotate(rotatePerFrameX * Time.deltaTime, rotatePerFrameY * Time.deltaTime, rotatePerFrameZ * Time.deltaTime, Space.Self);
    }
}
