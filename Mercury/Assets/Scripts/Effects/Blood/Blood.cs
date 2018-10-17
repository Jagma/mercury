using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {

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
    float offset = 2f;

    void Start () {
        //Instanciate center position
        center = gameObject.transform.position;

        List<GameObject> chunks = Factory.instance.CreateChunks(center,placementRadius);

        foreach (GameObject chunk in chunks)
        {
            //Offset to create randomness feel
            float y = center.y + Random.Range(-offset, offset);
            float x = center.x + Random.Range(-offset, offset);
            float z = center.z + Random.Range(-offset, offset);


            explosionPoint = new Vector3(x, y, z);
            power = Random.Range(6, 8);

            Rigidbody rigidbody = chunk.GetComponent<Rigidbody>();
            rigidbody.AddExplosionForce(power, explosionPoint, radius, upModifier, ForceMode.Impulse);
        }
        GameObject.Destroy(gameObject, 10f);
    }
	
	void Update () {

    }

}
