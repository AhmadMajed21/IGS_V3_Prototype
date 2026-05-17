using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public partial class GameManager
{
    private Dictionary<string, MethodInfo> methodInfoMap = new Dictionary<string, MethodInfo>();
    private Component selectedComponent;
    private Action<Component> queuedFunction;

    private void SetMethodInfoMap()
    {
        MethodInfo[] methods = typeof(GameManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).
            Where(m => m.GetCustomAttribute<AbilityAttribute>() != null).ToArray();

        foreach (MethodInfo methodInfo in methods)
        {
            methodInfoMap.Add(methodInfo.Name, methodInfo);
        }
    }

    private void SetAbilityAction(Ability ability)
    {
        if(ability == null)
        {
            return;
        }

        MethodInfo methodInfo;

        string abilityName = ability.name;

        if(methodInfoMap.ContainsKey(abilityName))
        {
            methodInfo = methodInfoMap[ability.name];
        }
        else
        {
            methodInfo = methodInfoMap["PrimaryAbilityNoEffect"];
        }

        Action<Component> action =
                (Action<Component>)Delegate.CreateDelegate(typeof(Action<Component>), this, methodInfo);

        ability.SetAbilityAction(action);
    }

    private void ActivatePrimaryAbility(Component component)
    {
        PrimaryAbility primaryAbility = component.GetPrimaryAbility();

        primaryAbility.UseAbility();
        SelectComponentThenActivateAbility(component, primaryAbility.GetAbilityAction());
    }

    private void ActivateSecondaryAbility(Component component)
    {
        SecondaryAbility secondaryAbility = component.GetSecondaryAbility();

        if(secondaryAbility.IsUsableOnce())
        {
            secondaryAbility.UseAbility();
        }

        if(secondaryAbility.RequiresSelection())
        {
            SelectComponentThenActivateAbility(component, secondaryAbility.GetAbilityAction());
        }
        else
        {
            secondaryAbility.GetAbilityAction().Invoke(component);
        }
    }

    private void SelectComponentThenActivateAbility(Component component, Action<Component> action)
    {
        EnterSelectionMode(component, action);
    }

    private void PowerComponent(List<Power> power)
    {
        selectedComponent.PowerComponent(power);
        AddPowerToDisplay(selectedComponent, selectedComponent.GetAppliedPower(), selectedComponent.GetRemainingRequiredPower());
        CheckIfShipComplete();
    }

    private void PowerComponent(Component component)
    {
        List<Power> power = component.GetPrimaryAbility().GetCompletePower();
        selectedComponent.PowerComponent(power);
        AddPowerToDisplay(selectedComponent, selectedComponent.GetAppliedPower(), selectedComponent.GetRemainingRequiredPower());
        CheckIfShipComplete();
    }

    private IEnumerator SelectComponent(Component component)
    {
        queuedFunction = null;
        selectedComponent = null;

        yield return new WaitUntil(() => selectedComponent != null);

        queuedFunction.Invoke(component);
        ExitSelectionMode();

        yield return null;
    }
}
