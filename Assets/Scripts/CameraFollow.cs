using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.3f;
    private Vector3 currentVelocity;

    // Update is called once per frame
    void LateUpdate()
    {
        //La cámara solo se mueve hacia arriba y no regresa para abajo.
        if (target.position.y > transform.position.y)
        {
            //Calcula la última posición más alta.
            Vector3 newPos = new Vector3(transform.position.x, target.position.y, transform.position.z);

            //Suaviza el movimiento de cámara
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVelocity, smoothSpeed);
        }
    }
}
