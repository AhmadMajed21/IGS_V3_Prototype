using UnityEngine;

public class FuelScript : MonoBehaviour
{
    private FuelType fuelType;

    public void SetFuelType(FuelType fuelType)
    {
        this.fuelType = fuelType;
    }

    public FuelType GetFuelType()
    {
        return fuelType;
    }
}
