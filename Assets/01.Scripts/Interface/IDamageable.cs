using UnityEngine;

public interface IDamageable
{
    void TakeDamage(double damage);
    void Heal(double amount, bool show_Text);

    bool is_Dead { get; set; }
}