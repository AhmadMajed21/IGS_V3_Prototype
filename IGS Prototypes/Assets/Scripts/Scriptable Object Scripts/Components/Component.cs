using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Component", menuName = "Scriptable Objects/Component")]
public class Component : ScriptableObject
{
    [SerializeField] private ComponentType fuelType;
    [SerializeField] private PrimaryAbility primaryAbility;
    [SerializeField] private SecondaryAbility secondaryAbility;
    [SerializeField] private List<Power> requiredPower = new List<Power>();

    private List<Power> appliedPower = new List<Power>();
    private List<Power> additionalRequiredPower = new List<Power>();
    private List<FuelType> attachedFuel = new List<FuelType>();
    private List<string> appliedEffects = new List<string>();

    public void ChangeComponentType(ComponentType newComponentType)
    {
        fuelType = newComponentType;
    }

    public void PowerComponent(List<Power> appliedPower)
    {
        this.appliedPower.AddRange(appliedPower);
    }

    public virtual void AddFuel(FuelType fuelType)
    {
        attachedFuel.Add(fuelType);
    }

    public virtual void RemoveFuel(FuelType fuelType)
    {
        for(int i = 0; i < attachedFuel.Count; i++)
        {
            if (attachedFuel[i] == fuelType)
            {
                attachedFuel.RemoveAt(i);
                return;
            }
        }
    }

    public void AddEffect(string effect)
    {
        appliedEffects.Add(effect);
    }

    public void AddRequiredPower(Power power)
    {
        additionalRequiredPower.Add(power);
    }

    public virtual void Reset()
    {
        primaryAbility?.Reset();
        secondaryAbility?.Reset();

        appliedPower.Clear();
        additionalRequiredPower.Clear();
        attachedFuel.Clear();
        appliedEffects.Clear();
    }

    public bool IsComplete()
    {
        List<Power> totalRequiredPower = new List<Power>();
        totalRequiredPower.AddRange(requiredPower);
        totalRequiredPower.AddRange(additionalRequiredPower);

        return !totalRequiredPower
            .GroupBy(x => x)
            .Any(g => appliedPower.Count(x => x.Equals(g.Key)) < g.Count());
    }

    public ComponentType GetComponentType()
    {
        return fuelType;
    }

    public List<FuelType> GetAttachedFuel()
    {
        return attachedFuel;
    }

    public PrimaryAbility GetPrimaryAbility()
    {
        return primaryAbility;
    }

    public SecondaryAbility GetSecondaryAbility()
    {
        return secondaryAbility;
    }

    public List<Power> GetRequiredPower()
    {
        List<Power> totalRequiredPower = requiredPower;
        totalRequiredPower.AddRange(additionalRequiredPower);

        return totalRequiredPower;
    }

    public List<Power> GetRemainingRequiredPower()
    {
        List<Power> remainingRequiredPower = new List<Power>();

        List<Power> totalRequiredPower = requiredPower;
        totalRequiredPower.AddRange(additionalRequiredPower);

        for (int i = 0; i < totalRequiredPower.Count - appliedPower.Count; i++)
        {
            remainingRequiredPower.Add(requiredPower[i]);
        }

        return remainingRequiredPower;
    }

    public List<Power> GetAppliedPower()
    {
        return appliedPower;
    }
}