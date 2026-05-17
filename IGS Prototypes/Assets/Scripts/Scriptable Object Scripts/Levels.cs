using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "Scriptable Objects/Levels")]
public class Levels : ScriptableObject
{
    public int startingLevel = 0;
    public List<Level> levels = new List<Level>();
}
