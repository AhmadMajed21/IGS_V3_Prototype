public partial class GameManager
{
    [Ability]
    private void PrimaryAbilityNoEffect(Component component)
    {
        //EnterSelectionMode(component, PowerComponent);
        PowerComponent(component);
    }

    [Ability]
    private void Haunt(Component component)
    {
        component.AddFuel(FuelType.Ghost);
        components[component.name].GetComponent<DropZoneScript>().AddFuelMarker(FuelType.Ghost);
    }

    [Ability]
    private void Locate(Component component)
    {
        component.GetPrimaryAbility().ReduceCost(FuelType.Coordinates);
    }

    [Ability]
    private void Nullify(Component component)
    {
        selectedComponent.ChangeComponentType(ComponentType.None);
        PowerComponent(component);
    }

    [Ability]
    private void Neutralize(Component component)
    {
        if (selectedComponent is GhostEngine ghostEngine)
        {
            ghostEngine.Stabilize();
            ghostEngine.AddRequiredPower(Power.Pi);

            ShowComponentIsStable(ghostEngine);
            ShowRequiredPower(ghostEngine);
        }
    }
}
