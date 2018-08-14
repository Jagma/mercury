using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0f, 10f * Time.deltaTime));
    }
}
