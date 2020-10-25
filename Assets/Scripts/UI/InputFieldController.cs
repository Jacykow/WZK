using TMPro;
using UniRx;
using UnityEngine;

[SelectionBase]
public class InputFieldController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI labelText;

    public ReactiveProperty<string> InputProperty { get; } = new ReactiveProperty<string>("");
    public string Label { get => labelText.text; set => labelText.text = value; }
    public string Value
    {
        get => InputProperty.Value;
        set
        {
            inputField.text = value;
            InputProperty.Value = value;
        }
    }

    private void Start()
    {
        InputProperty.Value = inputField.text;
        inputField.onEndEdit.AddListener(value => InputProperty.Value = value);
    }
}
