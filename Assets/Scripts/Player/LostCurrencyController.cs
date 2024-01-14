using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    public int experiencePlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.experience += experiencePlayer;
            Destroy(this.gameObject);
        }
    }
}
