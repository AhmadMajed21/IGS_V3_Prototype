using UnityEngine;

[CreateAssetMenu(fileName = "GhostEngine", menuName = "Scriptable Objects/Component/GhostEngine")]
public class GhostEngine : Component
{
    private bool stable = true;

    public override void AddFuel(FuelType fuelType)
    {
        base.AddFuel(fuelType);
    }

    public override void Reset()
    {
        base.Reset();
        Stabilize();
    }

    public void Stabilize()
    {
        stable = true;
    }

    public void Destabilize()
    {
        stable = false;
    }

    public bool IsStable()
    {
        return stable;
    }
}
