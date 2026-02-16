using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stork : MonoBehaviour
{
    private bool movingRight = true;

    void Update()
    {
        // Checa si llegó a algún límite y cambia la dirección
        if (transform.position.x >= GlobalVariables.xLimit && movingRight)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x <= -GlobalVariables.xLimit && !movingRight)
        {
            movingRight = true;
            Flip();
        }

    }

    private void Flip()
    {
        // Invierte el eje X de la escala para girar el sprite
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
