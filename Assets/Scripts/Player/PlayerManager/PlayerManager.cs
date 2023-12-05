using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script vai ser o nosso Singleton
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
