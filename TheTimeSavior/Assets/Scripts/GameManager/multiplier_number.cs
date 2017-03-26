using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class multiplier_number : MonoBehaviour {

    Text number;
	
	void Awake () {

        number = GetComponent<Text>();
    }
	
    void Update()
    {
        number.text = "" + score_manager_script._score.MultiplierValue();
    }
}
