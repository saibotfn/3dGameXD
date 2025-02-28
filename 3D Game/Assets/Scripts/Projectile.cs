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

    void OnTriggerEnter(Collider Other)
    {
        if(Other.gameObject.tag == "Enemy") { if (Other.GetType() == typeof(SphereCollider)) return; }
        
        //do damage
        Debug.Log(Other.gameObject.name);
        Destroy(gameObject);
    }

}
