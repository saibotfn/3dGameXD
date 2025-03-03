using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PewPew : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Stats playerStats;

    private float fireRate;
    private float nextFire = 0;

    private void Start()
    {
        fireRate = playerStats.fireRate;
    }

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
        for (int i = 0; i < playerStats.bulletAmount; i++)
        {
            //ADD BULLET SPREAD HEAR --- RANDOM ROTATION APPLIED SKALLING WITH PLAYERSTATS.BULLETSPREAD
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Projectile>().bulletSpeed = playerStats.bulletSpeed;
            bullet.GetComponent<Projectile>().damage = playerStats.baseDamage;
            bullet.GetComponent<Projectile>().critChance = playerStats.critChance;
            bullet.GetComponent<Projectile>().critDamage = playerStats.critDamage;
            bullet.transform.localScale *= playerStats.bulletSize;
        }
    }

}
