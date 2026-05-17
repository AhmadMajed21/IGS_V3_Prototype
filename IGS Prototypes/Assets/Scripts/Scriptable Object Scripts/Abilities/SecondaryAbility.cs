using UnityEngine;

[CreateAssetMenu(fileName = "SecondaryAbility", menuName = "Scriptable Objects/Ability/SecondaryAbility")]
public class SecondaryAbility : Ability
{
    [SerializeField] private bool usableOnce = true;

    public bool IsUsableOnce()
    {
        return usableOnce;
    }
}
