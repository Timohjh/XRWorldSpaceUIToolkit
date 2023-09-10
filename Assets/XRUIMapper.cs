using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUIMapper : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] XRRayInteractor rightHand, leftHand;
    [SerializeField] InputActionProperty selectRight, selectLeft;
    XRRayInteractor activeInteractor;

    void OnEnable()
    {
        if (document == null) 
            document = GetComponent<UIDocument>();

        activeInteractor = rightHand;

        document.panelSettings.SetScreenToPanelSpaceFunction((Vector2 screenPos) =>
        {
            var invalid = new Vector2(float.NaN, float.NaN);

            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * 100, Color.blue);

            RaycastHit hit;
            if(!activeInteractor.TryGetCurrent3DRaycastHit(out hit))
            {
                Debug.Log("invalid position");
                return invalid;
            }

            activeInteractor.TryGetCurrent3DRaycastHit(out hit);

            Vector2 pixelUV = hit.textureCoord;
            pixelUV.y = 1 - pixelUV.y;
            pixelUV.x *= document.panelSettings.targetTexture.width;
            pixelUV.y *= document.panelSettings.targetTexture.height;

            var cursor = document.rootVisualElement.Q<VisualElement>("cursor");

            if(cursor != null)
            {
                cursor.style.left = pixelUV.x;
                cursor.style.top = pixelUV.y;
            }

            return pixelUV;
        });
    }

    void Update()
    {
        if (selectRight.action.WasPressedThisFrame())
        {
            SwitchHands(true);
        }else if (selectLeft.action.WasPressedThisFrame())
        {
            SwitchHands(false);
        }
    }

    void SwitchHands(bool isRight)
    {
        if (isRight)
        {
            leftHand.enabled = false;
            rightHand.enabled = true;
            activeInteractor = rightHand;

        }
        else
        {
            rightHand.enabled = false;
            leftHand.enabled = true;
            activeInteractor = leftHand;
        }
    }
}
