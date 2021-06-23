using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void ChangeColor(string color)
    {
        string parametr = color switch
        {
            "red" => "RColor",
            "blue" => "BColor",
            "yellow" => "YColor",
            "green" => "GColor"
        };
        
        _animator.SetBool(parametr, true);
    }
}
