using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRunEffect : MonoBehaviour {

    public float bulletSpeed = 2;
    public int damage = 2;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(Vector3.right*Time.deltaTime*bulletSpeed,Space.Self);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Wood":
                Destroy(gameObject);
                break;

            case"Obstruct":

                break;

            case "Mushroom":
                collision.SendMessage("MushroomDamage", damage);
                break;
        }
    }

}
