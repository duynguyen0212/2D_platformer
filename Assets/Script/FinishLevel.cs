using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        PauseMenu obj = GetComponent<PauseMenu>();
        if(other.gameObject.tag== "Player"){
            obj.winMenu.SetActive(true);
        }
    }
}
