using System;
using UniRx;
using UnityEngine;

public class TrivialViewModel : MonoBehaviour
{
    public FieldContainer inputFieldContainer, outputFieldContainer;

    private void Start()
    {
        var nField = inputFieldContainer.AddInputField("Key amount");
        var kField = inputFieldContainer.AddInputField("k");
        var secretField = inputFieldContainer.AddInputField("secret");
        var calculateButton = inputFieldContainer.AddButton("Calculate");

        Observable.Merge(nField.InputProperty.AsUnitObservable(),
            kField.InputProperty.AsUnitObservable(),
            secretField.InputProperty.AsUnitObservable(),
            calculateButton.OnClickAsObservable())
            .Subscribe(_ =>
            {
                try
                {
                    int n = int.Parse(nField.InputProperty.Value);
                    int k = int.Parse(kField.InputProperty.Value);
                    int secret = int.Parse(secretField.InputProperty.Value);
                    var keys = new int[n];
                    int lastKey = secret;
                    for (int x = 0; x < keys.Length - 1; x++)
                    {
                        keys[x] = UnityEngine.Random.Range(0, k);
                        lastKey -= keys[x];
                    }
                    keys[keys.Length - 1] = (lastKey % k + k) % k;

                    outputFieldContainer.Clear();
                    for (int x = 0; x < keys.Length; x++)
                    {
                        outputFieldContainer.AddOutputField($"Key {x + 1}").Value = keys[x].ToString();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            })
            .AddTo(this);
    }
}
