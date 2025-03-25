using UnityEngine;

public class Health : MonoBehaviour,ITakeDamage
{
    [SerializeField] private float _maxHealth;
    public float MaxHealth { get => _maxHealth; set { _maxHealth = value; } }
    public float CurrentHealth { get ; set ; }
    public int DeathWoodDropAmount;
    private void Start()
    {
        InitHealth();
    }
    public void InitHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damageTaken)
    {
        CurrentHealth -= damageTaken;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
    }
}
