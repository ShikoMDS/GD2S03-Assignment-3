using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                GameObject player = other.gameObject;
                gameManager.RespawnPlayer(player);
            }
            else
            {
                Debug.LogError("GameManager not found!");
            }
        }
    }
}