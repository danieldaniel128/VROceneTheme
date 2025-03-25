using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    public float MaxHealth { get => _maxHealth; set { _maxHealth = value; } }
    [SerializeField] float _hp;
    public float CurrentHealth { get => _hp; set { _hp = value; } }
    public int DeathWoodDropAmount;
    private void Start()
    {
        InitHealth();
    }
    public void InitHealth()
    {
        CurrentHealth = MaxHealth;
    }
    [ContextMenu("take damage")]
    public void TakeDamage(float damageTaken,GameObject deathObject)
    {
        CurrentHealth -= damageTaken;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            GiveDrop();
            Destroy(deathObject);
        }
    }
    [ContextMenu("drop")]
    public void GiveDrop()
    {
        GameManager.Instance.TowerBuilder.AddWood(DeathWoodDropAmount);
    }
}
