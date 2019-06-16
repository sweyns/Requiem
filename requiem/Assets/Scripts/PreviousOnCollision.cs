using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameManager gameManager;
    void Start()
    {
        //gameManager = null;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {

    	if (collider.gameObject.tag == "Player") {
    		gameManager.LoadPreviousScene();
    	}
    }
}
