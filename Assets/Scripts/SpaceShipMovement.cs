
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpaceShipControllerV2 controller = null;

    [Header("Physics")]
    [Tooltip("Engine thrust")][Range(0f, 1f)] public float thrust = 0f;
    [Tooltip("Engine thrust step")][Range(0f, 1f)] public float thrustStep = .1f;
    [Tooltip("MaxForce to push plane forwards with")]public float maxSpeed = 100f;
    [Tooltip("Pitch, Yaw, Roll")] public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    [Tooltip("Multiplier for all forces")] public float forceMult = 1000f;

    [Header("Autopilot")]
    [Tooltip("Sensitivity for autopilot flight.")] public float sensitivity = 5f;
    [Tooltip("Angle at which airplane banks fully into target.")] public float aggressiveTurnAngle = 10f;

    [Header("Input")]
    [SerializeField][Range(-1f, 1f)] private float pitch = 0f;
    [SerializeField][Range(-1f, 1f)] private float yaw = 0f;
    [SerializeField][Range(-1f, 1f)] private float roll = 0f;


    [Header("Input Keys")]
    [SerializeField] private KeyCode keyToAcc = KeyCode.LeftShift;
    [SerializeField] private KeyCode keyToSlowDown = KeyCode.LeftControl;


    [Header("Effects")]
    [Tooltip("Engine VFX")] public VisualEffect visualEffect = null;
    [Tooltip("Global VFX")] public VolumeProfile volumeProfile = null;


    private ChromaticAberration ca;

    public float Pitch { set { pitch = Mathf.Clamp(value, -1f, 1f); } get { return pitch; } }
    public float Yaw { set { yaw = Mathf.Clamp(value, -1f, 1f); } get { return yaw; } }
    public float Roll { set { roll = Mathf.Clamp(value, -1f, 1f); } get { return roll; } }

    private Rigidbody rigid;

    private bool rollOverride = false;
    private bool pitchOverride = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        if (controller == null)
            Debug.LogError(name + ": Missing reference");

        if (visualEffect == null)
            Debug.Log(name + ": Missing VFX");

        for (int i = 0; i < volumeProfile.components.Count; i++)
        {
            if (volumeProfile.components[i].name == "ChromaticAberration")
            {
                ca = (ChromaticAberration)volumeProfile.components[i];
            }
        }
    }

    private void Update()
    {
       
        rollOverride = false;
        pitchOverride = false;

        if (Input.GetKey(keyToAcc))
        {
            if (thrust < 1f) 
            { 
                thrust += thrustStep * Time.deltaTime;
                if (ca.intensity.value < 0.5f)
                {
                    ca.intensity.value += thrust / 10 * Time.deltaTime;
                }
                
            }
        }

        if (Input.GetKey(keyToSlowDown))
        {
            if (thrust > 0f) 
            {
                thrust -= thrustStep * Time.deltaTime;
                if (ca.intensity.value > 0.01f)
                {
                    ca.intensity.value -= thrust / 10 * Time.deltaTime;
                }
            }
           
            
        }

        if (visualEffect != null)
        {
            visualEffect.playRate = thrust * 10f;
            visualEffect.transform.localScale = new Vector3(thrust * 2f, thrust * 2f, thrust * 2f);
        }

        float keyboardRoll = Input.GetAxis("Horizontal");
        if (Mathf.Abs(keyboardRoll) > .25f)
        {
            rollOverride = true;
        }

        float keyboardPitch = Input.GetAxis("Vertical");
        if (Mathf.Abs(keyboardPitch) > .25f)
        {
            pitchOverride = true;
            rollOverride = true;
        }

        
        float autoYaw = 0f;
        float autoPitch = 0f;
        float autoRoll = 0f;
        if (controller != null)
            RunAutopilot(controller.MouseAimPos, out autoYaw, out autoPitch, out autoRoll);

        yaw = autoYaw;
        pitch = (pitchOverride) ? keyboardPitch : autoPitch;
        roll = (rollOverride) ? keyboardRoll : autoRoll;
    }

    private void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch, out float roll)
    {

        var localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized * sensitivity;
        var angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);


        yaw = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1f, 1f);

        var agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);

        var wingsLevelRoll = transform.right.y;

        var wingsLevelInfluence = Mathf.InverseLerp(0f, aggressiveTurnAngle, angleOffTarget);
        roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
    }

    private void FixedUpdate()
    {
        rigid.AddRelativeForce(Vector3.forward * thrust * forceMult * maxSpeed, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch,
                                            turnTorque.y * yaw,
                                            -turnTorque.z * roll) * forceMult,
                                ForceMode.Force);
    }
}
