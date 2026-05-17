using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    private Levels levels;
    private Ship currentShip;
    private List<FuelType> fuelTypes = new List<FuelType>();

    private bool selectionMode = false;
    private int currentLevel = 0;

    private void Start()
    {
        LoadLevels();
        SetMethodInfoMap();
        SetUpFuel();
        SetUpNextShip();
    }

    private void LoadLevels()
    {
        levels = Resources.Load<Levels>("Levels");
        currentLevel = levels.startingLevel;
    }

    //Ship Setup

    private void SetUpNextShip()
    {
        if(currentLevel > levels.levels.Count - 1)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        currentShip = levels.levels[currentLevel].ship;

        ResetComponents(currentShip.GetComponents());
        SetUpComponents(currentShip.GetComponents());
        SetupAbilityActions(currentShip.GetComponents());

        currentLevel++;
    }

    private void ResetComponents(List<Component> components)
    {
        foreach(Component component in components)
        {
            component.Reset();
        }
    }

    public void ResetShip()
    {
        currentLevel--;
        SetUpNextShip();
    }

    private void CheckIfShipComplete()
    {
        if(!CheckStability())
        {
            return;
        }

        foreach(Component component in currentShip.GetComponents())
        {
            if(!component.IsComplete())
            {
                return;
            }
        }

        Debug.Log("Thank you");
        Invoke("SetUpNextShip", 1f);
    }

    //Abilities

    private void SetupAbilityActions(List<Component> components)
    {
        foreach(Component component in components)
        {
            SetAbilityAction(component.GetPrimaryAbility());
            SetAbilityAction(component.GetSecondaryAbility());
        }
    }

    //Selection

    public void Select(Component component)
    {
        if(!selectionMode)
        {
            return;
        }

        selectedComponent = component;

    }

    public void EnterSelectionMode(Component component, Action<Component> action)
    {
        selectionMode = true;
        StartCoroutine(SelectComponent(component));
        queuedFunction = (component) => action(component);
    }

    public void ExitSelectionMode()
    {
        selectionMode = false;
    }

    //Fuel

    public void AddFuel(Component component, FuelType fuelType)
    {
        component.AddFuel(fuelType);
        components[component.name].GetComponent<DropZoneScript>().AddFuelMarker(fuelType);
    }

    public void RemoveFuel(Component component, FuelType fuelType)
    {
        component.RemoveFuel(fuelType);
        components[component.name].GetComponent<DropZoneScript>().RemoveFuelMarker(fuelType);
    }

    //Component Specific

    public void StabilizeComponent(GhostEngine ghostEngine)
    {
        ghostEngine.Stabilize();
        ShowComponentIsStable(ghostEngine);
    }

    public void DestabilizeComponent(GhostEngine ghostEngine)
    {
        ghostEngine.Destabilize();
        ShowComponentIsUnstable(ghostEngine);
    }

    private bool CheckStability()
    {
        int totalInstability = 0;

        foreach (Component component in currentShip.GetComponents())
        {
            if(component is GhostEngine ghostEngine)
            {
                if (!ghostEngine.IsStable())
                {
                    totalInstability++;
                }
            }
        }

        if (totalInstability >= 2)
        {
            return false;
        }

        return true;
    }
}
