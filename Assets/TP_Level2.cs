using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Level2 : MonoBehaviour
{
    public Transform player;

    public void ButtonPress(){
		player.position = new Vector3(0,38,0);
	  }
}
