using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class TowerController : MonoBehaviour , ITakeDamage
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    [SerializeField] Transform projectile;
    [SerializeField] float _projMaxHeight;
    [SerializeField] float _attackTimer;
    [SerializeField] float _attackCooldown = 2;
    [SerializeField] float _scanTimer;
    [SerializeField] float _scanCooldown = 0.5f;
    [SerializeField] Transform _spawnPoint;
    public LayerMask targetLayer;
    Transform _target;
    private void Update()
    {
        Transform target = Scan();
        if(target!=null)
            _target = target;
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0)
        { 
            _attackTimer = _attackCooldown; 
            if (_target != null)
            {
                Transform projectile = Instantiate(this.projectile, _spawnPoint.position, Quaternion.identity, _spawnPoint);
                if (((Time.time - preTime) / durationHit) <= 1)
                    MoveProjectile(projectile, transform.position, target.position, (Time.time - preTime) / durationHit);
            }
        }
    }
    public float radius = 4f;
    float preTime;
    [SerializeField] float durationHit;
    private void Start()
    {
        preTime = Time.time;
    }
    Transform Scan()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer <= 0)
        {   
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
            //reset scan timer
            _scanTimer = _scanCooldown;
            if (hitColliders.Length > 0)
                return hitColliders[0].transform;
        }
        return null;
    }
    public void MoveProjectile(Transform target, Vector3 a, Vector3 b, float time)
    {
        float target_X = a.x + (b.x - a.x) * time;
        float maxHeigh = (a.y + b.y) / 2 + _projMaxHeight;
        float target_Y = a.y + ((b.y - a.y)) * time + _projMaxHeight * (1 - (Mathf.Abs(0.5f - time) / 0.5f) * (Mathf.Abs(0.5f - time) / 0.5f));
        target.position = new Vector3(target_X, target_Y);
    }
    public void InitHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damageTaken)
    {
        CurrentHealth -= damageTaken;
        if(CurrentHealth<0)
            CurrentHealth = 0;
        //gameobject death here
    }
    
    
}
