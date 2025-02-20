
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpaceShipControllerV2 controller = null;
    [SerializeField] private SpaceShipMovement shipMovement = null;

    [Header("HUD Elements")]
    [SerializeField] private RectTransform boresight = null;
    [SerializeField] private RectTransform mousePos = null;
    [SerializeField] private Slider thrustUI = null;

    private Camera playerCam = null;
    private void Awake()
    {
        if (controller == null)
            Debug.LogError(name + ": Hud - Controller not assigned!");

        playerCam = controller.GetComponentInChildren<Camera>();

        if (playerCam == null)
            Debug.LogError(name + ": Hud - No camera found on assigned!");

        if (shipMovement == null)
            Debug.LogError(name + ": Hud - No space ship movement script found on assigned!");

        if (thrustUI == null)
        {
            Debug.LogError(name + ": Hud - No thrustUI found on assigned!");
        }
        else
        {
            thrustUI.enabled = true;
            thrustUI.maxValue = 1f;
            thrustUI.minValue = 0f;
            thrustUI.value = 0f;
        }
            

    }

    private void Update()
    {
        if (controller == null || playerCam == null || thrustUI == null || shipMovement == null)
            return;

        UpdateGraphics(controller);
    }

    private void UpdateGraphics(SpaceShipControllerV2 controller)
    {
        if (boresight != null)
        {
            boresight.position = playerCam.WorldToScreenPoint(controller.BoresightPos);
            boresight.gameObject.SetActive(boresight.position.z > 1f);
        }

        if (mousePos != null)
        {

            mousePos.position = playerCam.WorldToScreenPoint(controller.MouseAimPos);
            mousePos.gameObject.SetActive(mousePos.position.z > 1f);

        }

        if (thrustUI != null)
        {
       
            thrustUI.normalizedValue = shipMovement.thrust;
            Debug.Log(shipMovement.thrust);
        }
    }
}
