using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITerminalActivation : MonoBehaviour
{
    public float ActivationDistance;
    public Transform Player;
    public Transform Terminal;

    void Update()
    {
        if(((Vector3.Distance(gameObject.transform.position,Player.position) <= ActivationDistance)) && (Input.GetKeyDown(KeyCode.B)))
        {
            if(!Terminal.gameObject.activeInHierarchy)
            {
                player_script.pl_script.IsInMenu = true;
                Terminal.gameObject.SetActive(true);
            }
            else
            {
                Terminal.gameObject.SetActive(false);
                player_script.pl_script.IsInMenu = false;
            }
        }
    }
}
