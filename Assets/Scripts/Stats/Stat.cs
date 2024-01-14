using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue; //Valor base da estatística.

    public List<int> modifiers; //Lista de modificadores aplicados à estatística.

    public int GetValue() //Calcula o valor final da estatística somando o valor base aos modificadores.
    {
        int finalValue = baseValue;

        foreach(int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public void DefaultValues(int _value)
    {
        baseValue = _value;
    }
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
