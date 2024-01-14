using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Este script vai ser o nosso Singleton
public class PlayerManager : MonoBehaviour , ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int experience;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool ExperienceEnough(int _exp)
    {
        if(_exp > experience)
        {
            Debug.Log("nout enough exp");
            return false;
        }

        experience = experience - _exp;
        return true;
    }

    public int CurrentExp() => experience;

    public void LoadData(GameData _data)
    {
        experience = _data.experience;
    }

    public void SaveData(ref GameData _data)
    {
        _data.experience = this.experience;
    }
}
