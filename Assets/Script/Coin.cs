using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public TextMeshProUGUI coinCounter;
    public int coin;
    void Start(){
        coin = 0;
        coinCounter.text = "9";
    }
    void Update()
    {
        transform.Rotate(1,0,0);
        coinCounter.SetText(""+coin);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        // Check if the collision is with the target object
        if (other.CompareTag("Player"))
        {
            coin += 1;
            gameObject.SetActive(false);
        }
        
    }
}
