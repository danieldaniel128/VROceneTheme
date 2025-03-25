using UnityEngine;
using UnityEngine.UIElements;

public class AxeController : MonoBehaviour
{
    [SerializeField] float damage = 1;
    [SerializeField] ParticleSystem _hitVfx;
    [SerializeField] Transform hitPoint;
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            Debug.Log("hit tree");
            Instantiate(_hitVfx, hitPoint.position, hitPoint.rotation,hitPoint);
            health.TakeDamage(damage, other.attachedRigidbody.gameObject);
            
        }
    }
}
