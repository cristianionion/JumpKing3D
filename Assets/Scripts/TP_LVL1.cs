using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TP_LVL1 : MonoBehaviour
{
    public Transform player;

    public void ButtonPress(){
		player.position = new Vector3(0,1,0);
	  }
    
}
