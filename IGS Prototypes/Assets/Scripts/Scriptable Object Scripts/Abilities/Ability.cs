using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability")]
public class Ability: ScriptableObject
{
    [SerializeField] private string text;
    [SerializeField] private bool requiresSelection = false;

    protected Action<Component> abilityAction;
    protected bool used = false;

    public void SetAbilityAction(Action<Component> action)
    {
        abilityAction = action;
    }

    public void UseAbility()
    {
        used = true;
    }

    public virtual void Reset()
    {
        used = false;
    }

    public string GetText()
    {
        return text;
    }

    public bool RequiresSelection()
    {
        return requiresSelection;
    }

    public Action<Component> GetAbilityAction()
    {
        return abilityAction;
    }

    public bool IsUsed()
    {
        return used;
    }
}