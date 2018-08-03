using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    bool camShakeStatus = true;
    public static CameraSystem instance;
    private void Awake()
    {
        instance = this;
    }

    public List<Transform> trackedList = new List<Transform>();
    Vector3 cameraAngle = new Vector3(45, 45, 0);

    void Update ()
    {
        Vector3 targetPos = Vector3.zero;
        for (int i=0; i < trackedList.Count; i ++)
        {
            if (trackedList[i] == null)
            {
                trackedList.RemoveAt(i);
                i--;                
            } else
            {
                targetPos += trackedList[i].position;
            }            
        }
        targetPos -= transform.forward * 10;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
        transform.localEulerAngles = cameraAngle;
    }

    public void setCamShakeStatus(bool status)
    {
        camShakeStatus = status;
    }
    public void SubscribeToTracking (Transform t)
    {
        if (trackedList.Contains(t) == false)
        {
            trackedList.Add(t);
        }
    }

    public void UnsubscribeFromTracking (Transform t)
    {
        if (trackedList.Contains(t))
        {
            trackedList.Remove(t);
        }
    }
    public void ShakePosition(Vector3 direction)
    {
        if (camShakeStatus)
        {
            transform.position += direction;
        }
    }
}
