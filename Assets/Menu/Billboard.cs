using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public AnimationCurve curvaVelocidad;
    public Transform aroReferencia;
    void LateUpdate()
    {
        if (aroReferencia == null) return;

        transform.rotation = Camera.main.transform.rotation;

        // Obtenemos el ángulo actual del padre
        float anguloPadre = transform.parent.localEulerAngles.z;

        // Evaluamos la curva para obtener una "velocidad personalizada" en ese punto
        float multiplicador = curvaVelocidad.Evaluate(anguloPadre);

        // Aplicamos la rotación usando ese multiplicador
        transform.Rotate(0, 0, -anguloPadre * multiplicador);
    }
}
