using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int experience;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentID;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostExperienceX;
    public float lostExperienceY;
    public int lostExperienceAmount;
    public GameData()
    {
        this.lostExperienceX = 0;
        this.lostExperienceY = 0;
        this.lostExperienceAmount = 0;

        this.experience = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentID = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
    }
}
