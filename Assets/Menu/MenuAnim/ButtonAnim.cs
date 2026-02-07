using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public void SetHover(bool parametro) { anim.SetBool("hover", parametro); }
    public void SetPressed(bool parametro) { anim.SetBool("pressed", parametro); }

}