using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {


    System.Random random = new System.Random(91169420);

    //Radius of circle used to place rigidbodies
    float placementRadius = 0.3f;
    //Center point of the circle
    public Vector3 center;

    /// <summary>
    /// Variables for explosion
    /// </summary>
    Vector3 explosionPoint;
    float power;
    float radius = 5f;
    float upModifier = 0.3f;
    float yOffset = 0.1f;
    float zOffset = 0.1f;
    float xOffset = 0.1f;

    void Start () {
        //Instanciate center position
        center = gameObject.transform.position;

        List<GameObject> chunks = Factory.instance.CreateChunks(center,placementRadius);



        foreach (GameObject chunk in chunks)
        {
            //Offset to create randomness feel
            float y = center.y + yOffset * random.Next(-1, 1);
            float x = center.x + xOffset * random.Next(-1, 1);
            float z = center.z + zOffset * random.Next(-1, 1);


            explosionPoint = new Vector3(x, y, z);
            power = random.Next(1, 5);

            Rigidbody rigidbody = chunk.GetComponent<Rigidbody>();
            rigidbody.AddExplosionForce(power, explosionPoint, radius, upModifier, ForceMode.Impulse);
        }

        GameObject.Destroy(gameObject, 10f);
    }
	
	void Update () {

    }

}
