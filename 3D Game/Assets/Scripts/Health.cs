using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 10;

    public void reduceHealth(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
