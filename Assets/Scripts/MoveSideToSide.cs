using UnityEngine;

public class MoveSideToSide : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private bool movingRight = true;


    void Update()
    {
        //Checa si llegó a algún límite y cambia la dirección
        if (transform.position.x >= GlobalVariables.xLimit)
        {
            movingRight = false;
        }
        else if (transform.position.x <= -GlobalVariables.xLimit)
        {
            movingRight=true;
        }

        //Dependiendo del límite con el que chocó, se mueve a la derecha o a la izquierda
        if (movingRight)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        
    }
}