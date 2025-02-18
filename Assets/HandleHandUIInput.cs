using UnityEngine;
using UnityEngine.UI;

public class HandleHandUIInput : MonoBehaviour
{
    [SerializeField] Button _handUIBTN;
    [SerializeField] LayerMask _handMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        _handUIBTN.targetGraphic.color = _handUIBTN.colors.pressedColor;
        _handUIBTN.onClick?.Invoke();
        Debug.Log("clicked pressed");
    }
    private void OnTriggerExit(Collider other)
    {
        //if ((_handMask.value & (1 << other.transform.gameObject.layer)) > 0)
            _handUIBTN.targetGraphic.color = _handUIBTN.colors.highlightedColor;
    }
}
