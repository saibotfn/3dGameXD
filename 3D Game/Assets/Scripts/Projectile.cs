using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletLife = 10f;
    public float damage;
    public float critChance;
    public float critDamage;


 
    void Start()
    {
        Destroy(gameObject, bulletLife);
    }
    void FixedUpdate()
    {
        transform.position += transform.forward * bulletSpeed;
    }

    void OnTriggerEnter(Collider Other)
    {
        if(Other.gameObject.tag == "Enemy") 
        { 
            if (Other.GetType() == typeof(SphereCollider)) return; 
            
            if (Random.Range(0, 100) <= critChance)
            {
                Other.gameObject.GetComponent<Health>().reduceHealth(damage * critDamage);
            }
            else
            {
                Other.gameObject.GetComponent<Health>().reduceHealth(damage);
            }
            
        }
        if(Other.gameObject.tag == "Projectile") return;
        
        Debug.Log(Other.gameObject.tag);
        Destroy(gameObject);
    }

}
