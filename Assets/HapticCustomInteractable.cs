using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class HapticCustomInteractable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(0, 1)]
    [SerializeField] float _intensity;
    [SerializeField] float _duration;
    private void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(TrigerHaptic);
    }
    public void TrigerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if(eventArgs.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
            TrigerHaptic(controllerInteractor.xrController);
    }
    public void TrigerHaptic(XRBaseController controller)
    {
        if (_intensity>0)
        {
            controller.SendHapticImpulse(_intensity, _duration);
        }
    }
}
