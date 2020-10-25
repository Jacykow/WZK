using TMPro;
using UniRx;
using UnityEngine;

[SelectionBase]
public class OutputFieldController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text, labelText;

    public string Value { get => text.text; set => text.text = value; }
    public string Label { get => labelText.text; set => labelText.text = value; }
}
