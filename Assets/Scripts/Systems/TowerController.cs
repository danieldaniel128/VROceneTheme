using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.WSA;
using static UnityEngine.GraphicsBuffer;

public class TowerController : MonoBehaviour
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
    public bool IsPlaced;
    private void Update()
    {
        if (!IsPlaced)
            return;
        Transform target = Scan();
        if(target!=null)
            _target = target;
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0)
        { 
            _attackTimer = _attackCooldown; 
            if (_target != null)
            {
                Debug.Log("taget is not null");
                Transform projectile = Instantiate(this.projectile, _spawnPoint.position, Quaternion.identity, _spawnPoint);
                Launch(projectile, _spawnPoint.position, _target.position, durationHit, 2);/* MoveProjectile(projectile, _spawnPoint.position, target.position, (Time.time - preTime) / durationHit);*/
                Destroy(projectile.gameObject, 1f);
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
    public void Launch(Transform arcedProj, Vector3 start, Vector3 end, float duration, float arcHeight)
    {
        StartCoroutine(MoveArc(arcedProj, start, end, duration, arcHeight));
    }

    private System.Collections.IEnumerator MoveArc(Transform arcedProj,Vector3 start, Vector3 end, float duration, float arcHeight)
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // Linear interpolation between start and end
            Vector3 linearPos = Vector3.Lerp(start, end, t);

            // Arc using a parabolic height curve
            float height = arcHeight * 4 * (t - t * t); // 4t(1-t) gives a nice parabola from 0 to 1

            // Add arc height to the y position
            arcedProj.position = new Vector3(linearPos.x, linearPos.y + height, linearPos.z);

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is set
        arcedProj.position = end;
    }
}
    
