using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class SecretShamirViewModel : MonoBehaviour
{
    public FieldContainer inputFieldContainer, intermediateFieldContainer,
        outputFieldContainer, keyInputFieldContainer, secretOutputFieldContainer;

    private int[] coefficients;
    private InputFieldController[] inputKeys;
    private int prime;

    private void Start()
    {
        inputFieldContainer.AddMenuButton();

        var nField = inputFieldContainer.AddInputField("Keys generated", "5");
        var tField = inputFieldContainer.AddInputField("Keys required", "3");
        var pField = inputFieldContainer.AddInputField("Prime", "1523");
        var secretField = inputFieldContainer.AddInputField("Secret", "420");
        var calculateButton = inputFieldContainer.AddButton("Calculate");

        Observable.Merge(
            nField.InputProperty.AsUnitObservable(),
            tField.InputProperty.AsUnitObservable(),
            pField.InputProperty.AsUnitObservable(),
            secretField.InputProperty.AsUnitObservable(),
            calculateButton.OnClickAsObservable())
            .SelectMany(_ =>
            {
                try
                {
                    intermediateFieldContainer.Clear();
                    outputFieldContainer.Clear();
                    keyInputFieldContainer.Clear();
                    secretOutputFieldContainer.Clear();

                    int n = int.Parse(nField.Value);
                    prime = int.Parse(pField.Value);
                    int t = int.Parse(tField.Value);
                    int secret = int.Parse(secretField.Value);

                    coefficients = new int[t];
                    coefficients[0] = secret;
                    intermediateFieldContainer.AddOutputField($"Coefficient 0 (Secret)").Value = coefficients[0].ToString();
                    for (int x = 1; x < coefficients.Length; x++)
                    {
                        coefficients[x] = UnityEngine.Random.Range(1, 1000);
                        intermediateFieldContainer.AddOutputField($"Coefficient {x}").Value = coefficients[x].ToString();
                    }

                    var keys = new int[n];
                    int lastKey = secret;
                    for (int x = 0; x < keys.Length; x++)
                    {
                        keys[x] = 0;
                        int degreeValue = 1;
                        for (int y = 0; y < coefficients.Length; y++)
                        {
                            keys[x] = (keys[x] + coefficients[y] * degreeValue) % prime;
                            degreeValue *= x + 1;
                        }
                        outputFieldContainer.AddOutputField($"Key {x + 1}").Value = keys[x].ToString();
                    }

                    var randomKeys = new List<int>();
                    int keysLeft = t;
                    for (int x = 0; x < n; x++)
                    {
                        if (UnityEngine.Random.value < (float)keysLeft / (n - x))
                        {
                            randomKeys.Add(x);
                            keysLeft--;
                        }
                    }
                    for (int x = 0; x < randomKeys.Count; x++)
                    {
                        int swap = UnityEngine.Random.Range(x, randomKeys.Count);
                        int temp = randomKeys[swap];
                        randomKeys[swap] = randomKeys[x];
                        randomKeys[x] = temp;
                    }

                    inputKeys = new InputFieldController[t];
                    for (int x = 0; x < inputKeys.Length; x++)
                    {
                        inputKeys[x] = keyInputFieldContainer.AddInputField($"\"id,key\" pair {x + 1}", $"{randomKeys[x] + 1},{keys[randomKeys[x]]}");
                    }

                    return Observable.Merge(
                        inputKeys.Select(inputKey => inputKey.InputProperty.AsUnitObservable())
                        .Append(keyInputFieldContainer.AddButton("Calculate Secret").OnClickAsObservable()));
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

                return Observable.ReturnUnit();
            })
            .Subscribe(_ =>
            {
                try
                {
                    secretOutputFieldContainer.Clear();

                    var degrees = new int[inputKeys.Length];
                    var keys = new int[inputKeys.Length];
                    for (int x = 0; x < inputKeys.Length; x++)
                    {
                        var splitInput = inputKeys[x].Value.Split(',');
                        degrees[x] = int.Parse(splitInput[0]);
                        keys[x] = int.Parse(splitInput[1]);
                    }

                    var yl = new int[inputKeys.Length];
                    for (int x = 0; x < inputKeys.Length; x++)
                    {
                        int lTop = 1;
                        for (int y = 0; y < inputKeys.Length; y++)
                        {
                            if (y == x) continue;
                            lTop *= -degrees[y];
                        }
                        int lBottom = 1;
                        for (int y = 0; y < inputKeys.Length; y++)
                        {
                            if (y == x) continue;
                            lBottom *= degrees[x] - degrees[y];
                        }

                        int l = MathG.ModDivide(lTop, lBottom, prime);
                        yl[x] = MathG.Mod(l * keys[x], prime);

                        secretOutputFieldContainer.
                            AddOutputField($"l{x + 1}; y{x + 1}l{x + 1}").Value = $"{l}; {yl[x]}";
                    }

                    int calculatedSecret = 0;
                    for (int x = 0; x < yl.Length; x++)
                    {
                        calculatedSecret += yl[x];
                    }
                    calculatedSecret = MathG.Mod(calculatedSecret, prime);

                    secretOutputFieldContainer.AddOutputField("Calculated Secret").Value = (calculatedSecret).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            })
            .AddTo(this);
    }
}
