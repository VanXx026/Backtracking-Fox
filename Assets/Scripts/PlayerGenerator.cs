using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject rollBacksPrefab;

    void Start()
    {
        var player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        GameManager.instance.player = player.GetComponent<PlayerController>();
        
        var rollBacks = Instantiate(rollBacksPrefab, transform.position, Quaternion.identity);
        player.GetComponent<PlayerController>().rollBacks = rollBacks.transform;
    }

}
