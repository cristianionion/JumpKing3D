using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Level3 : MonoBehaviour
{
    public Transform player;

    public void ButtonPress(){
        player.position = new Vector3(0,75,0);
	  }
}
