using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class PowerUp : MonoBehaviour
{
    [Header("Inscribed")]
    // This is an unusual but handy use of Vector2s
    [Tooltip("xholds a min value and y a max value for a Random.Rage() call.")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    [Tooltip("x holds a min value and y a max value for a Ranom.Range() call.")]
    public Vector2 driftMinMiax = new Vector2(.25f, 2);
    public float lifeTime = 10; // PowerUp wil exist for # seconds
    public float fadTime = 4; // Then it fades over # seconds

    [Header("Dynamic")]
    public eWeaponType _type; //the type of the PowerUp
    public GameObject cube; // Reference to the PowerCube child
    public TextMesh letter; //Reference to the TextMesh
    public Vector3 rotPerSecond; // Euler rotation speed for PowerCube
    public float birthTime; // theTime this was instantiated
    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Material cubeMat;


    void Awake()
    {
        //Find the cube reference (Ther's only a single chidl)
        cube = transform.GetChild(0).gameObject;
        //Find the TextMesh and other components
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeMat = cube.GetComponent<Renderer>().material;

        //Set a random velocity
        Vector3 vel = Random.onUnitSphere; //Get Random XYZ velocity
        vel.z = 0; // Flatten the vel to the XY plane
        vel.Normalize(); //Normalizing a VEctor3 sets its length to 1m

        vel *= Random.Range(driftMinMiax.x, driftMinMiax.y);
        rigid.velocity = vel;

        //Set the rotation of this powerUp GameObject to R:[0,0,0]
        transform.rotation = Quaternion.identity;
        //Quaternion.identity is equal to no rotation.

        //Randomize rotPerSecond for PowerCube using rotMinMax x&y
        rotPerSecond = new Vector3 (Random.Range(rotMinMax[0], rotMinMax[1]),
                                    Random.Range(rotMinMax[0], rotMinMax[1]),
                                    Random.Range(rotMinMax[0], rotMinMax[1]));

        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //Fade out the PowerUp over time
        //Given the defualt values, a PowerUp wil exist for 10 seconds
        // and then fade out over 4 seconds.
        float u = (Time.time - (birthTime + lifeTime)) / fadTime;
        //If u >= 1, destroy this Powerup
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //if u > 0, decrase the opacity (i.e., alpha) of the PowerCube & letter

        if (u > 0)
        {
            Color c = cubeMat.color;
            c.a = 1f - u; // set the alpha of powercube to 1-u
            cubeMat.color = c;
            //Fade the Letter to, just not as much 
            c = letter.color;
            c.a = 1f - (u * 0.5f); // Set the alpha of the letter 1 - (u/2)
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            //if the PowerUp has drifted entirely off screen, destroy it
            Destroy(gameObject);
        }

    }

    public eWeaponType type { get { return _type; } set { SetType(value); } }

    public void SetType(eWeaponType wt)
    {
        //Grab the WeaponDefinition from Main
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION(wt);
        cubeMat.color = def.powerUpColor; // set the color of PowerCube
        //letter .color = def.color; //We could colorize the letter too
        letter.text = def.letter; //set the letter that is shown
        _type = wt; // finally actualy set the type
    }

    //This function is called by the Hero Class when a PwerUp is collected

    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
