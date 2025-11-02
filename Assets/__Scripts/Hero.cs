using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S { get; private set; } // Singleton property

    [Header("Inscribed")]
    //These fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;
    // public WeaponFireDelegate[] weapons;

    [Header("Dynamic")]
    [Range(0, 4)]
    [SerializeField]
    private float _shieldLevel = 1;
    // public float shieldLevel = 1;

    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;
    //declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //create a WeaponFireDelegate even named fireEvent
    public event WeaponFireDelegate fireEvent;

    void Awake()
    {
        if (S == null)
        {
            S = this; //set the singleton only if it's null
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign secound Hero.S!");
        }
        // fireEvent += TempFire;

        ClearWeapons();
        weapons[0].SetType(eWeaponType.blaster);
    }

    // Update is called once per frame
    void Update()
    {
        //Pull in information from the Input class
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        //Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        //Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(vAxis * pitchMult, hAxis * rollMult, 0);

        //Allow the ship to fire
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     TempFire();
        // }

        //user the fireEvent to fire Weapons whe the Spacebar is pressed ddd
        if(Input.GetAxis("Jump") == 1 && fireEvent != null)
        {
            fireEvent();
        }

    }


    //     void TempFire(){
    //     GameObject projGO = Instantiate<GameObject>(projectilePrefab);
    //     projGO.transform.position = transform.position;
    //     Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
    //     // rigidB.velocity = Vector3.up * projectileSpeed;

    //     ProjectileHero proj = projGO.GetComponent <ProjectileHero>();
    //     proj.type = eWeaponType.blaster;
    //     float tSpeed = Main.GET_WEAPON_DEFINITION(proj.type).velocity;
    //     rigidB.velocity = Vector3.up * tSpeed;
    // }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        // Debug.Log("Shield trigger hit by: " + go.gameObject.name);

        //Make sure it's not the same triggering go as last time
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        PowerUp pUp = go.GetComponent<PowerUp>();
        if (enemy != null)
        {
            //if the shield was triggered by an enemy
            shieldLevel--; // Decrease the level of the shield by
            Destroy(go); //... and Destroy the enemy
        }
        else if (pUp != null)
        {
            //if the shield hits a PowerUp
            AbsorbPowerUp(pUp); // ...absorb the PowerUp
        }
        else
        {
            Debug.LogWarning("Shield trigger hit by non-Enemy:" + go.name);
        }
    }


    public void AbsorbPowerUp(PowerUp pUp)
    {
        Debug.Log("Abosrbed PowerUp: " + pUp.type);
        switch (pUp.type)
        {
            case eWeaponType.shield:
                shieldLevel++;
                break;

            default:
                if (pUp.type == weapons[0].type)
                {
                    Weapon weap = GetEmptyWeaponSlot();
                    if (weap != null)
                    {
                        //set it to pUp.type
                        weap.SetType(pUp.type);
                    }
                }
                else
                {
                    //if this is a different weapon type
                    ClearWeapons();
                    weapons[0].SetType(pUp.type);
                }
                break;
            //Leave this switch block empty for now
        }
        pUp.AbsorbedBy(this.gameObject);
    }
    
    public float shieldLevel
    {
        get { return (_shieldLevel); }
        private set
        {
            _shieldLevel = Mathf.Min(value, 4);
            //if the shield is going to be set to less than zero,...
            if (value < 0)
            {
                Destroy(this.gameObject);//Destroy the Hero
                Main.HERO_DIED();
            }
        }
    }

    //Finds the first empty Weapon slot(ie.,type = none ) and returns it

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == eWeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }


    //sets the type of all Weapon slots to none
    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(eWeaponType.none);
        }
    }
}
