using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComponentDisplayScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI componentNameText;
    [SerializeField] private TextMeshProUGUI powerText;

    public void SetComponentNameText(string componentName)
    {
        componentNameText.text = componentName;
    }

    public void SetRequiredPowerText(List<Power> power)
    {
        powerText.text = power.ToCustomString();
        powerText.color = Color.gray;
    }

    public void AddPower(List<Power> additonalPower, List<Power> remainingPower)
    {
        string additonalPowerText = "<color=blue>" + additonalPower.ToCustomString() + "</color>";
        string previousPowerText = "<color=grey>" + remainingPower.ToCustomString() + "</color>";

        powerText.text = additonalPowerText + previousPowerText;
    }
}
