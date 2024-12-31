using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    [SerializeField] Transform _teleportedObject;
    float _teleportY;
    [SerializeField] Transform _rayOrigin;
    public float _rayDistance = 15f; // Distance of the raycast
    public LayerMask _teleportLayer; // Assign the TeleportLayer in the Inspector
    [SerializeField] private float _teleportTime;
    private float _teleportProgressTime;
    [SerializeField] private float _teleportOffset;
    [SerializeField] private LineRenderer _rayIndicator;
    //private float _teleportSpeed;
    private void Start()
    {
        _rayIndicator.positionCount = 2;
        _teleportY = _teleportedObject.position.y;
    }
    void Update()
    {
        _rayIndicator.SetPosition(0, _teleportedObject.position);
        Vector3 localRayDirectionForward = _rayOrigin.forward;
        if (Physics.Raycast(_teleportedObject.position, localRayDirectionForward, out RaycastHit hit, _rayDistance, _teleportLayer))
        {
            _teleportProgressTime += Time.deltaTime;
            if (_teleportProgressTime >= _teleportTime)
            {
                var teleportPosition = hit.point - _teleportOffset * (hit.point - _teleportedObject.position).normalized;
                teleportPosition.y = _teleportY;
                _teleportedObject.position = hit.point -_teleportOffset * (hit.point - _teleportedObject.position).normalized;
                _teleportProgressTime = 0;
            }
        }
        else
        {
            _teleportProgressTime = 0;
        }
    }
}
