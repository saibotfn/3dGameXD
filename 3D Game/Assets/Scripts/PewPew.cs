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

    private float magSize;
    private float reloadTime;
    private float currentMag;
    private float timeTilReload;
    private float bulletSpread;
    private bool reloading = false;

    private void Start()
    {
        fireRate = playerStats.fireRate;
        magSize = playerStats.magSize;
        reloadTime = playerStats.reloadTime;
        currentMag = magSize;
        bulletSpread = playerStats.bulletSpread;
    }

    void FixedUpdate()
    {
        if (currentMag != 0)
        {
            if (Mouse.current.leftButton.isPressed && Time.time >= nextFire)
            {
                Shoot();
                nextFire = Time.time + fireRate;
            }
        }
        else if (reloading)
        {
            timeTilReload -= Time.deltaTime;
            if (timeTilReload < 0)
            {
                reloading = false;
                currentMag = magSize;
            }
        }
        else
        {
            reloading = true;
            timeTilReload = reloadTime;
        }
    }

    public void Shoot()
    {
        for (int i = 0; i < playerStats.bulletAmount; i++)
        {
            if (currentMag != 0)
            {
                currentMag--;
                Quaternion randRot = Quaternion.Euler(Random.Range(0, bulletSpread), Random.Range(0, bulletSpread), 0);
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * randRot);
                bullet.GetComponent<Projectile>().bulletSpeed = playerStats.bulletSpeed;
                bullet.GetComponent<Projectile>().damage = playerStats.baseDamage;
                bullet.GetComponent<Projectile>().critChance = playerStats.critChance;
                bullet.GetComponent<Projectile>().critDamage = playerStats.critDamage;
                bullet.transform.localScale *= playerStats.bulletSize;
            }
        }
    }

}
