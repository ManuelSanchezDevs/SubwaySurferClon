using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RestartLevelPosition : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;
    private Transform levelTransform;
    [SerializeField]private int limitLevel;
    private int newLevelTransform;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerTransform = player.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (player.transform.position.z >= limitLevel)
        {
            print(player.transform.position.z);
            playerTransform.position = new Vector3(0, 0, 0);
            newLevelTransform-=2;
            transform.position = new Vector3(0,0,newLevelTransform);
        }
    }
}
