using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Content;
using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryAbility", menuName = "Scriptable Objects/Ability/PrimaryAbility")]
public class PrimaryAbility : Ability
{
    [SerializeField] private List<FuelType> cost;
    [SerializeField] private List<Power> power = new List<Power>();

    private List<Power> additionalPower = new List<Power>();
    private List<FuelType> discount = new List<FuelType>();

    public void AddPower(Power additionalPower)
    {
        this.additionalPower.Add(additionalPower);
    }

    public void AddPower(List<Power> additionalPower)
    {
        foreach (Power power in additionalPower)
        {
            this.additionalPower.Add(power);
        }
    }

    public void ReduceCost(FuelType discount)
    {
        this.discount.Add(discount);
    }

    public void ReduceCost(List<FuelType> discount)
    {
        foreach(FuelType fuelType in discount)
        {
            this.discount.Add(fuelType);
        }
    }

    public override void Reset()
    {
        base.Reset();
        additionalPower.Clear();
        discount.Clear();
    }

    public bool CheckCost(List<FuelType> fuelTypes)
    {
        fuelTypes.AddRange(discount);

        return !cost
            .GroupBy(x => x)
            .Any(g => fuelTypes.Count(x => x.Equals(g.Key)) < g.Count());
    }

    public List<Power> GetCompletePower()
    {
        List<Power> power = new List<Power>();

        power.AddRange(this.power);
        power.AddRange(additionalPower);

        return power;
    }
}