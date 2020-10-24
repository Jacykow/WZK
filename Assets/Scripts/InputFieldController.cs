using TMPro;
using UniRx;
using UnityEngine;

[SelectionBase]
public class InputFieldController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    public ReactiveProperty<string> InputProperty { get; } = new ReactiveProperty<string>();
    public string Description { get => descriptionText.text; set => descriptionText.text = value; }

    private void Start()
    {
        InputProperty.Value = inputField.text;
        inputField.onEndEdit.AddListener(value => InputProperty.Value = value);
    }
}
