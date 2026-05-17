using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZoneScript : MonoBehaviour, IDropHandler
{
    [Header("Fuel Marker")]
    [SerializeField] GameObject fuelMarkerPrefab;
    [SerializeField] Vector2 fuelMarkerStartingPosition;
    [SerializeField] float fuelMarkerSpacing;

    private Component component;

    private Dictionary<FuelType, List<GameObject>> fuelMarkers = new Dictionary<FuelType, List<GameObject>>();
    private Vector2 fuelMarkerPosition;

    private GameManager gameManager;

    private void Awake()
    {
        fuelMarkerPosition = fuelMarkerStartingPosition;
    }

    public void AddFuelMarker(FuelType fuelType)
    {
        GameObject clone = Instantiate(fuelMarkerPrefab, transform);

        //Set position
        RectTransform fuelMarkerTransform = clone.GetComponent<RectTransform>();
        fuelMarkerTransform.anchoredPosition = fuelMarkerPosition;
        fuelMarkerPosition += new Vector2(fuelMarkerTransform.sizeDelta.x + fuelMarkerSpacing, 0);

        //Set color
        Color color = Color.HSVToRGB((int)fuelType * 0.2f % 1f, 0.8f, 0.9f);
        Image image = clone.GetComponent<Image>();
        image.color = color;

        //Record markers
        if(fuelMarkers.ContainsKey(fuelType))
        {
            fuelMarkers[fuelType].Add(clone);
        }
        else
        {
            fuelMarkers.Add(fuelType, new List<GameObject> { clone });
        }
    }

    public void RemoveFuelMarker(FuelType fuelType)
    {
        List<GameObject> fuelMarkers = this.fuelMarkers[fuelType];
        GameObject fuelMarker = fuelMarkers[fuelMarkers.Count - 1];

        Destroy(fuelMarker);
        fuelMarkers.RemoveAt(fuelMarkers.Count - 1);

        RectTransform fuelMarkerTransform = fuelMarker.GetComponent<RectTransform>();
        fuelMarkerPosition -= new Vector2(fuelMarkerTransform.sizeDelta.x + fuelMarkerSpacing, 0);
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableScript draggableScript = eventData.pointerDrag?.GetComponent<DraggableScript>();
        FuelScript fuelScript = eventData.pointerDrag?.GetComponent<FuelScript>();

        if (draggableScript == null || fuelScript == null)
        {
            return;
        }

        draggableScript.Hide();

        FuelType fuelType = fuelScript.GetFuelType();
        component.AddFuel(fuelType);
        AddFuelMarker(fuelType);
        gameManager.ReplaceFuel(fuelType);

        if(component is GhostEngine ghostEngine)
        {
            ghostEngine.Destabilize();
            gameManager.DestabilizeComponent(ghostEngine);
        }
    }

    public void Initialize(GameManager gameManager, Component component)
    {
        this.gameManager = gameManager;
        this.component = component;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}