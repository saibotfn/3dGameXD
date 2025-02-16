using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletLife = 10f;


 
    void Start()
    {
        Destroy(gameObject, bulletLife);
    }
    void FixedUpdate()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    void onTriggerEnter(Collider Other)
    {
        //do damage
        Destroy(gameObject);
    }

}
