using UnityEngine;

[CreateAssetMenu(fileName = "Navigation", menuName = "Scriptable Objects/Component/Navigation")]
public class Navigation : Component
{
    private bool usable = true;

    public override void AddFuel(FuelType fuelType)
    {
        base.AddFuel(fuelType);
        usable = false;
    }

    public override void RemoveFuel(FuelType fuelType)
    {
        base.RemoveFuel(fuelType);

        if(GetAttachedFuel().Count == 0)
        {
            usable = true;

        }
    }

    public bool IsUsable()
    {
        if(GetComponentType() == ComponentType.None)
        {
            return true;
        }

        return usable;
    }
}
