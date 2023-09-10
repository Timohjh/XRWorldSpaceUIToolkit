using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUIMapper : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] XRRayInteractor rightHand, leftHand;
    [SerializeField] InputActionProperty selectRight, selectLeft;

    XRRayInteractor _activeInteractor;
    public XRRayInteractor ActiveInteractor { get { return _activeInteractor; } }

    void OnEnable()
    {
        if (document == null) 
            document = GetComponent<UIDocument>();

        _activeInteractor = rightHand;

        document.panelSettings.SetScreenToPanelSpaceFunction((Vector2 screenPos) =>
        {
            var invalid = new Vector2(float.NaN, float.NaN);

            RaycastHit hit;
            if(!_activeInteractor.TryGetCurrent3DRaycastHit(out hit))
            {
                return invalid;
            }

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
        }
        else if (selectLeft.action.WasPressedThisFrame())
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
            _activeInteractor = rightHand;

        }
        else
        {
            rightHand.enabled = false;
            leftHand.enabled = true;
            _activeInteractor = leftHand;
        }
    }
}
