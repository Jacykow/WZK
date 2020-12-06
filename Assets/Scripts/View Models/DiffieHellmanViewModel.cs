using System;
using UniRx;
using UnityEngine;

public class DiffieHellmanViewModel : MonoBehaviour
{
    public FieldContainer inputFieldContainer, outputFieldContainer;

    private void Start()
    {
        inputFieldContainer.AddMenuButton();

        var nField = inputFieldContainer.AddInputField("n", "7919");
        var gField = inputFieldContainer.AddInputField("g", "49");
        var privateXField = inputFieldContainer.AddInputField("A: private x", "1337");
        var privateYField = inputFieldContainer.AddInputField("B: private y", "997");

        Observable.Merge(
            nField.InputProperty.AsUnitObservable(),
            gField.InputProperty.AsUnitObservable(),
            privateXField.InputProperty.AsUnitObservable(),
            privateYField.InputProperty.AsUnitObservable(),
            inputFieldContainer.AddButton("Calculate key").OnClickAsObservable()).
            Subscribe(_ =>
            {
                outputFieldContainer.Clear();
                try
                {
                    int n = int.Parse(nField.Value);
                    int g = int.Parse(gField.Value);
                    int privateX = int.Parse(privateXField.Value);
                    int privateY = int.Parse(privateYField.Value);

                    int publicX = MathG.ModPow(g, privateX, n);
                    int publicY = MathG.ModPow(g, privateY, n);

                    outputFieldContainer.AddOutputField("A: public y").Value = publicY.ToString();
                    outputFieldContainer.AddOutputField("B: public x").Value = publicX.ToString();

                    var kA = MathG.ModPow(publicY, privateX, n);
                    var kB = MathG.ModPow(publicX, privateY, n);

                    outputFieldContainer.AddOutputField("A: k").Value = kA.ToString();
                    outputFieldContainer.AddOutputField("B: k").Value = kB.ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }).AddTo(this);

    }
}
