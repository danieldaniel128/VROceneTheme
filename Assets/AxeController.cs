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
            health.TakeDamage(damage);
            Instantiate(_hitVfx, hitPoint.position, hitPoint.rotation,hitPoint);
            if (health.CurrentHealth <= 0)
            {
                GameManager.Instance.TowerBuilder.AddWood(health.DeathWoodDropAmount);
                Destroy(other.attachedRigidbody.gameObject);
            }
        }
    }
}
