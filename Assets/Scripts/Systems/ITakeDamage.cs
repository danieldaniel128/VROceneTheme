public interface ITakeDamage
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public void TakeDamage(float damageTaken);
    public void InitHealth();
}