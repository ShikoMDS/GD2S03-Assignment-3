using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reduce player's health and respawn them
            GameManager.instance.RespawnPlayer(other.gameObject);
        }
    }
}
