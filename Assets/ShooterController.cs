using UnityEngine;
using UnityEngine.InputSystem;
public class ShooterController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _spawnBulletsPoint;
    [SerializeField] private float _bulletSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputActionProperty _shootingAction;

    private void Start()
    {
        _shootingAction.action.started += OnShotPerformed;
    }

    private void OnDestroy()
    {
        _shootingAction.action.started -= OnShotPerformed;
    }
    private void OnApplicationQuit()
    {
        _shootingAction.action.started -= OnShotPerformed;
    }

    private void OnShotPerformed(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(_bulletPrefab, _spawnBulletsPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(_spawnBulletsPoint.right* _bulletSpeed,ForceMode.Impulse);
        Destroy(bullet, 5);
        Debug.Log("Shot action performed!");
    }
}
