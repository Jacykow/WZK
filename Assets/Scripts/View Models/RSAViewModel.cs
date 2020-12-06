using System;
using UniRx;
using UnityEngine;

public class RSAViewModel : MonoBehaviour
{
    public FieldContainer inputFieldContainer, middleContainer, outputFieldContainer;
    private InputFieldController eField, dField, messageField;

    private void Start()
    {
        inputFieldContainer.AddMenuButton();

        var pField = inputFieldContainer.AddInputField("p", RandomLargePrime().ToString());
        var qField = inputFieldContainer.AddInputField("q", RandomLargePrime().ToString());

        Observable.Merge(
            pField.InputProperty.AsUnitObservable(),
            qField.InputProperty.AsUnitObservable(),
            inputFieldContainer.AddButton("Calculate e and d").OnClickAsObservable()).
            SelectMany(_ =>
            {
                middleContainer.Clear();

                try
                {
                    int p = int.Parse(pField.Value);
                    int q = int.Parse(qField.Value);

                    int n = p * q;
                    int phi = (p - 1) * (q - 1);

                    eField = middleContainer.AddInputField("e", NextSpecialCoprime(phi).ToString());
                    dField = middleContainer.AddInputField("d", "463"); //TODO e
                    messageField = middleContainer.AddInputField("message", Random(n).ToString());
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

                return Observable.ReturnUnit();

            }).Subscribe(_ =>
            {
                try
                {
                    int e = int.Parse(eField.Value);
                    int d = int.Parse(dField.Value);
                    int msg = int.Parse(messageField.Value);


                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }).AddTo(this);
    }

    private int RandomLargePrime()
    {
        return MathG.NextPrime(UnityEngine.Random.Range(2000, 10000));
    }

    private int Random(int max)
    {
        return UnityEngine.Random.Range(1, max);
    }

    private int NextSpecialCoprime(int phi)
    {
        int start = Random(phi);
        while (!MathG.IsPrime(start)) // TODO add GCD with phi as condition
        {
            start++;
        }
        return start;
    }
}
