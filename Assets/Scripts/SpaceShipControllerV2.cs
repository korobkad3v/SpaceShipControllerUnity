using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControllerV2 : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    [Tooltip("Transform of the aircraft the rig follows and references")]
    private Transform spaceship = null;
    [SerializeField]
    [Tooltip("Transform of the object the mouse rotates to generate MouseAim position")]
    private Transform mouseAim = null;
    [SerializeField]
    [Tooltip("Transform of the object on the rig which the camera is attached to")]
    private Transform cameraRig = null;
    [SerializeField]
    [Tooltip("Transform of the camera itself")]
    private Transform cam = null;

    [Header("Options")]
    [SerializeField]
    [Tooltip("Follow aircraft using fixed update loop")]
    private bool useFixed = true;

    [SerializeField]
    [Tooltip("How quickly the camera tracks the mouse aim point.")]
    private float camSmoothSpeed = 5f;

    [SerializeField]
    [Tooltip("Mouse sensitivity for the mouse flight target")]
    private float mouseSensitivity = 3f;

    [SerializeField]
    [Tooltip("How far the boresight and mouse flight are from the aircraft")]
    private float aimDistance = 500f;


    private Vector3 frozenDirection = Vector3.forward;
    private bool isMouseAimFrozen = false;

    [Header("Input Keys")]
    [SerializeField]
    [Tooltip("Key to frozen mouse")] 
    private KeyCode keyToFrozenMouseAim = KeyCode.C;

    public Vector3 BoresightPos
    {
        get
        {
            return spaceship == null
                 ? transform.forward * aimDistance
                 : (spaceship.transform.forward * aimDistance) + spaceship.transform.position;
        }
    }

    
    public Vector3 MouseAimPos
    {
        get
        {
            if (mouseAim != null)
            {
                return isMouseAimFrozen
                    ? GetFrozenMouseAimPos()
                    : mouseAim.position + (mouseAim.forward * aimDistance);
            }
            else
            {
                return transform.forward * aimDistance;
            }
        }
    }

    private void Awake()
    {
        if (spaceship == null)
            Debug.LogError(name + "No spaceship transform assigned!");
        if (mouseAim == null)
            Debug.LogError(name + "No mouse aim transform assigned!");
        if (cameraRig == null)
            Debug.LogError(name + "No camera rig transform assigned!");
        if (cam == null)
            Debug.LogError(name + "No camera transform assigned!");

        transform.parent = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        if (useFixed == false)
            UpdateCameraPos();

        RotateRig();
    }

    private void FixedUpdate()
    {
        if (useFixed == true)
            UpdateCameraPos();
        //RotateRig();
    }

    private void RotateRig()
    {
        if (mouseAim == null || cam == null || cameraRig == null)
            return;

        if (Input.GetKeyDown(keyToFrozenMouseAim))
        {
            isMouseAimFrozen = true;
            frozenDirection = mouseAim.forward;
        }
        else if (Input.GetKeyUp(keyToFrozenMouseAim))
        {
            isMouseAimFrozen = false;
            mouseAim.forward = frozenDirection;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

        
        mouseAim.Rotate(cam.right, mouseY, Space.World);
        mouseAim.Rotate(cam.up, mouseX, Space.World);

        Vector3 upVec = (Mathf.Abs(mouseAim.forward.y) > 0.9f) ? cameraRig.up : Vector3.up;

        cameraRig.rotation = Damp(cameraRig.rotation,
                                  Quaternion.LookRotation(mouseAim.forward, upVec),
                                  camSmoothSpeed,
                                  Time.deltaTime);
    }

    private Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    private Vector3 GetFrozenMouseAimPos()
    {
        if (mouseAim != null)
            return mouseAim.position + (frozenDirection * aimDistance);
        else
            return transform.forward * aimDistance;
    }

    private void UpdateCameraPos()
    {
        
            transform.position = spaceship.GetChild(0).position;

    }
}
