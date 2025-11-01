using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an enum of the various possible weapon types.
/// It Also includes a "shield" type to allow a shield PowerUp.
/// </summary>

public enum eWeaponType
{
    none,   //The defualt / no weapon
    blaster,   // A simple blaster
    spread, //Multiple shots simultaneously
    phaser, //[NI] Shots that move in waves
    missile, // [NI] Homing missiles
    laser, //[NI] Damage over time
    shield     //Rise shieldLevel
}

/// <summary>
/// The Weapon Definition class allows you to set the properties
/// of a specific weapon in the Inspector.  The main class has 
/// an array of WeaponDefinitions that makes this possible
/// </summary>

[System.Serializable]
public class WeaponDefinition
{
    public eWeaponType type = eWeaponType.none;
    [Tooltip("Letter to show on the PowerUp Cube")]
    public string letter;
    [Tooltip("Color of PowerUp Cube")]
    public Color powerUpColor = Color.white;
    [Tooltip("Prefab of Weapon model that is attached to the Player Ship")]
    public GameObject weaponModelPrefab;
    [Tooltip("Prefab of projectile that is fired")]
    public GameObject projectilePrefab;
    [Tooltip("Color of the Projectile that is fired")]
    public Color projectileColor = Color.white;
    [Tooltip("Damage caused when a single Projectile hits an Enemy")]
    public float damgeOnHit = 0;
    [Tooltip("Damage caused per second by the Laser [Not Impolemented]")]
    public float damgerPerSec = 0;
    [Tooltip("Seconds to delay between shots")]
    public float delayBetweenShots = 0;
    [Tooltip("Veolocity of individual Projectile")]
    public float velocity = 50;
}
public class Weapon : MonoBehaviour
{

}
