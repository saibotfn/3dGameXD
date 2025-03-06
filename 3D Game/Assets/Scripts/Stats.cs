using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Base Stats")]
    public float currentHealth; //Done ish
    public float speed; //Done
    public float maxHealth; //Done ish
    public float fireRate; //Done
    public float bulletSpeed; //Done
    public float sprintSpeed; //Done
    public float bulletAmount; //Done
    public float bulletSize; //Done
    public float critChance; //Done
    public float critDamage; //Done
    public float bulletSpread; //Done
    public float magSize; //Done
    public float reloadTime; //Done
    public float baseDamage; //Done

    [Header("Bullet effekts")]
    public bool knockback;
    public bool fire;
    public bool ice;
    public bool poisen;
    public bool bleed;
    public bool lighting;
    public bool death;
    public bool explosion;

    [Header("Modifier")]
    public bool shotgun;
    public bool sniper;
    public bool railgun;
    public bool lifeSteal;

    [Header("Extra item effeks or stats")]
    public float headShotMuliplier;
    public float damageReduction;

    private void Start()
    {
        currentHealth = maxHealth;
    }

}
