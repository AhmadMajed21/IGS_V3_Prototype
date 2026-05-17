using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "Scriptable Objects/Ship")]
public class Ship : ScriptableObject
{
    [SerializeField] private List<Component> components = new List<Component>();

    public List<Component> GetComponents()
    {
        return components;
    }
}
