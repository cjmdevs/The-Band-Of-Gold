using UnityEngine;

public class spectre_proj : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy projectile after time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            collision.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            Destroy(gameObject); // Destroy projectile on impact
        }
    }
}
