using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 10;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag != "Projectile") return;
        Debug.Log("Hit");
        health--;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
