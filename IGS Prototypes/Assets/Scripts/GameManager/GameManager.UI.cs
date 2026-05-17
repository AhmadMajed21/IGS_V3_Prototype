using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public partial class GameManager
{
    [SerializeField] Canvas canvas;

    [Header("Ability Panel")]
    [SerializeField] GameObject abilityPanel;
    [SerializeField] GameObject primaryButton;
    [SerializeField] GameObject secondaryButton;

    [Header("Component")]
    [SerializeField] GameObject componentPrefab;
    [SerializeField] Transform shipGrid;
    [SerializeField] Vector2 componentStartingPosition;
    [SerializeField] float componentSpacing;

    [Header("Fuel")]
    [SerializeField] GameObject fuelHolderPrefab;
    [SerializeField] GameObject fuelPrefab;
    [SerializeField] GameObject fuelPanelVeil;
    [SerializeField] Transform fuelPanel;
    [SerializeField] Vector2 fuelStartingPosition;
    [SerializeField] float fuelSpacing;

    private Dictionary<string, GameObject> components = new Dictionary<string, GameObject>();
    private List<GameObject> fuelObjects = new List<GameObject>();
    private Dictionary<FuelType, GameObject> fuelHolders = new Dictionary<FuelType, GameObject>();

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !IsMouseHovering())
        {
            HideAbilities();
        }
    }

    private void SetUpComponents(List<Component> components)
    {
        ClearComponents();
        abilityPanel.SetActive(false);

        Vector2 position = componentStartingPosition;

        foreach (Component component in components)
        {
            GameObject clone = Instantiate(componentPrefab, shipGrid);

            //Set position
            RectTransform componentTransform = clone.GetComponent<RectTransform>();
            componentTransform.anchoredPosition = position;
            position += new Vector2(componentTransform.sizeDelta.x + componentSpacing, 0);

            //Set text
            string componentName = component.name.Replace("(Incomplete)", "");
            clone.GetComponent<ComponentDisplayScript>().SetComponentNameText(componentName);
            clone.GetComponent<ComponentDisplayScript>().
                SetRequiredPowerText(component.GetRequiredPower());

            //Bind ability panel
            Button componentButton = clone.GetComponent<Button>();
            componentButton.onClick.AddListener(() => ShowAbilities(component));

            //Bind selection
            componentButton.onClick.AddListener(() => Select(component));

            //Setup drop zone
            DropZoneScript dropZone = clone.GetComponent<DropZoneScript>();
            dropZone.Initialize(this, component);

            //Add component
            this.components.Add(component.name, clone);

            //Reset Component
            component.Reset();
        }
    }

    private void SetAbilityFunctions(GameObject button, Component component, Ability ability)
    {
        //Get and reset ability display
        AbilityDisplayScript abilityDisplay = button.GetComponent<AbilityDisplayScript>();
        abilityDisplay.Unblock();

        //Clear binded ability functions
        button.GetComponent<Button>().onClick.RemoveAllListeners();

        //Check if ability exists
        if (ability == null)
        {
            abilityDisplay.SetText("");
            return;
        }

        //Set button text
        if(ability.GetText() == "")
        {
            abilityDisplay.SetText(ability.name);
        }
        else
        {
            abilityDisplay.SetText(ability.GetText());

        }

        //Binding buttons to ability functions
        if (ability is PrimaryAbility primaryAbility)
        {
            //Check cost
            if (!primaryAbility.CheckCost(component.GetAttachedFuel()))
            {
                abilityDisplay.Block();
            }

            //Check if primary has been already used
            if (primaryAbility.IsUsed())
            {
                abilityDisplay.Block();
            }

            //Check if navigation has fuel attached
            if (component is Navigation navigation)
            {
                if(!navigation.IsUsable())
                {
                    abilityDisplay.Block();
                }
            }

            button.GetComponent<Button>().onClick.AddListener(() =>
                ActivatePrimaryAbility(component));
        }
        else if(ability is SecondaryAbility secondaryAbility)
        {
            //Check if secondary has already been used
            if (secondaryAbility.IsUsed() && secondaryAbility.IsUsableOnce())
            {
                abilityDisplay.Block();
            }

            button.GetComponent<Button>().onClick.AddListener(() =>
                ActivateSecondaryAbility(component));
        }
    }

    private void SetUpFuel()
    {
        ClearFuelObjects();
        fuelTypes.Clear();

        for (int i = 0; i <= levels.startingLevel; i++)
        {
            FuelType newFuelType = levels.levels[i].fuelType;

            if (newFuelType != FuelType.None)
            {
                fuelTypes.Add(newFuelType);
            }
        }

        fuelPanelVeil.SetActive(false);

        Vector2 position = fuelStartingPosition;

        foreach (FuelType fuelType in fuelTypes)
        {
            GameObject clone = Instantiate(fuelHolderPrefab, fuelPanel);

            //Set position
            RectTransform fuelTransform = clone.GetComponent<RectTransform>();
            fuelTransform.anchoredPosition = position;
            position += new Vector2(fuelTransform.sizeDelta.x + fuelSpacing, 0);

            //Set color
            Color color = Color.HSVToRGB((int)fuelType * 0.2f % 1f, 0.8f, 0.9f);
            Image image = clone.GetComponent<Image>();
            image.color = color;

            //Add Fuel
            FuelHolderScript fuelHolderScript = clone.GetComponent<FuelHolderScript>();
            fuelHolderScript.CreateFuelClone(fuelType);

            fuelHolders.Add(fuelType, clone);
        }
    }

    private void ClearComponents()
    {
        foreach (KeyValuePair<string, GameObject> component in components)
        {
            if (component.Value != null)
            {
                Destroy(component.Value);
            }
        }

        components.Clear();
    }

    private void ClearFuelObjects()
    {
        foreach (GameObject fuelObject in fuelObjects)
        {
            Destroy(fuelObject);
        }

        fuelObjects.Clear();
    }

    public void ShowAbilities(Component component)
    {
        if (selectionMode) return;

        abilityPanel.SetActive(true);

        //Set position of ability panel
        RectTransform abilityPanelTransform = abilityPanel.GetComponent<RectTransform>();
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvasTransform, Mouse.current.position.ReadValue(), null, out Vector2 mousePos);
        abilityPanelTransform.anchoredPosition =
            mousePos + new Vector2(abilityPanelTransform.sizeDelta.x / 2, -abilityPanelTransform.sizeDelta.y / 2);

        //Set ability functions
        SetAbilityFunctions(primaryButton, component, component.GetPrimaryAbility());
        SetAbilityFunctions(secondaryButton, component, component.GetSecondaryAbility());
    }

    private void ShowRequiredPower(Component component)
    {
        components[component.name].GetComponent<ComponentDisplayScript>().
                SetRequiredPowerText(component.GetRemainingRequiredPower());
    }

    private void AddPowerToDisplay(Component component, List<Power> additonalPower, List<Power> remainingPower)
    {
        components[component.name].
            GetComponent<ComponentDisplayScript>().AddPower(additonalPower, remainingPower);
    }

    public void HideAbilities()
    {
        abilityPanel.SetActive(false);
    }

    private bool IsMouseHovering()
    {
        return primaryButton.GetComponent<AbilityDisplayScript>().IsMouseHovering() ||
            secondaryButton.GetComponent<AbilityDisplayScript>().IsMouseHovering();
    }

    public void ReplaceFuel(FuelType fuelType)
    {
        fuelHolders[fuelType].GetComponent<FuelHolderScript>().CreateFuelClone(fuelType);
    }

    private void ShowComponentIsStable(GhostEngine ghostEngine)
    {
        GameObject componentObject = components[ghostEngine.name];
        componentObject.GetComponent<Image>().color = Color.white;
        componentObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }

    private void ShowComponentIsUnstable(GhostEngine ghostEngine)
    {
        GameObject componentObject = components[ghostEngine.name];
        componentObject.GetComponent<Image>().color = Color.red;
        componentObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
}
