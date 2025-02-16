using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PewPew : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float fireRate = 0.2f;
    private float nextFire = 0;


    void FixedUpdate()
    {
        if(Mouse.current.leftButton.isPressed && Time.time >= nextFire)
        {
            Shoot();
            nextFire = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }





}
