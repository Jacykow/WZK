using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject inputFieldPrefab, outputFieldPrefab, buttonPrefab;

    public InputFieldController AddInputField(string label, string defaultValue = "")
    {
        var inputField = Instantiate(inputFieldPrefab, transform).GetComponent<InputFieldController>();
        inputField.Label = label;
        inputField.Value = defaultValue;
        return inputField;
    }

    public OutputFieldController AddOutputField(string label)
    {
        var outputField = Instantiate(outputFieldPrefab, transform).GetComponent<OutputFieldController>();
        outputField.Label = label;
        return outputField;
    }

    public Button AddButton(string label)
    {
        var button = Instantiate(buttonPrefab, transform).GetComponent<Button>();
        button.GetComponentInChildren<TextMeshProUGUI>().text = label;
        return button;
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
