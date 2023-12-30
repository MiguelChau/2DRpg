using UnityEngine;



public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _enemyPos)
    {
        Debug.Log("Applied Effect");
    }
}
