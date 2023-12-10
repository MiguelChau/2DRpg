using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDur;
    [Space]
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_SkillController>().SetupClone(_clonePosition, cloneDur, canAttack, _offset);
    }
}
