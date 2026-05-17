using UnityEngine;
using UnityEngine.UI;

public class FuelHolderScript : MonoBehaviour
{
    [SerializeField] private GameObject fuelPrefab;
    [SerializeField] private float fuelSpacing;

    public void CreateFuelClone(FuelType fuelType)
    {
        GameObject clone = Instantiate(fuelPrefab, transform);

        //Set position
        RectTransform fuelTransform = clone.GetComponent<RectTransform>();
        fuelTransform.anchoredPosition = Vector2.zero;

        //Setup fuel type
        FuelScript fuelScript = clone.GetComponent<FuelScript>();
        fuelScript.SetFuelType(fuelType);

        //Set color
        Color color = Color.HSVToRGB((int)fuelType * 0.2f % 1f, 0.8f, 0.9f);
        Image image = clone.GetComponent<Image>();
        image.color = color;
    }
}
