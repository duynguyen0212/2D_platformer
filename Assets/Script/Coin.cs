using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    
    void Start(){
        
    }
    void Update()
    {
        transform.Rotate(1,0,0);
    }

    public void Disappear(){
        gameObject.SetActive(false);
    }

    
}
