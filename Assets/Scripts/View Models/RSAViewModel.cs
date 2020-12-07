using System;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

public class RSAViewModel : MonoBehaviour
{
    public FieldContainer inputFieldContainer, middleContainer, outputFieldContainer;
    private InputFieldController eField, dField, messageField;

    private int n;

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

                    n = p * q;
                    int phi = (p - 1) * (q - 1);

                    int e = GenerateE(phi);
                    int d = GenerateD(phi, e);

                    eField = middleContainer.AddInputField("e", e.ToString());
                    dField = middleContainer.AddInputField("d", d.ToString());
                    messageField = middleContainer.AddInputField("message", RandomMessage());

                    return Observable.Merge(
                        Observable.ReturnUnit(),
                        eField.InputProperty.AsUnitObservable().
                            Select(__ =>
                            {
                                dField.Value = GenerateD(phi, int.Parse(eField.Value)).ToString();
                                return Unit.Default;
                            }),
                        dField.InputProperty.AsUnitObservable(),
                        messageField.InputProperty.AsUnitObservable(),
                        middleContainer.AddButton("Encrypt & Decrypt").OnClickAsObservable());
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

                return Observable.ReturnUnit();

            }).Subscribe(_ =>
            {
                outputFieldContainer.Clear();
                try
                {
                    int e = int.Parse(eField.Value);
                    int d = int.Parse(dField.Value);
                    string msg = messageField.Value;

                    var msgBytes = Encoding.UTF8.GetBytes(msg);
                    var encodedMsgInts = msgBytes.Select(b => MathG.ModPow(b, e, n)).ToArray();
                    byte[] encodedMsgBytes = new byte[encodedMsgInts.Length * sizeof(int)];
                    Buffer.BlockCopy(encodedMsgInts, 0, encodedMsgBytes, 0, encodedMsgBytes.Length);
                    string encodedMsg = Convert.ToBase64String(encodedMsgBytes);

                    outputFieldContainer.AddLongOutputField("Encoded Message").Value = encodedMsg;

                    var encodedMsgBytes2 = Convert.FromBase64String(encodedMsg);
                    int[] encodedMsgInts2 = new int[encodedMsgBytes2.Length / sizeof(int)];
                    Buffer.BlockCopy(encodedMsgBytes2, 0, encodedMsgInts2, 0, encodedMsgBytes2.Length);
                    var decodedMsgBytes = encodedMsgInts2.Select(i => (byte)MathG.ModPow(i, d, n)).ToArray();
                    var decodedMsg = Encoding.UTF8.GetString(decodedMsgBytes);

                    outputFieldContainer.AddOutputField("Decoded Message").Value = decodedMsg;

                    outputFieldContainer.AddOutputField("Original Message").Value = msg;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }).AddTo(this);
    }

    private int RandomLargePrime()
    {
        return MathG.NextPrime(UnityEngine.Random.Range(2000, 9800));
    }

    private int Random(int max)
    {
        return UnityEngine.Random.Range(1, max);
    }

    private int GenerateE(int phi)
    {
        int e = Random(phi);
        while (!MathG.IsPrime(e) || MathG.GCD(phi, e) != 1)
        {
            e++;
            if (e >= phi)
            {
                e = 2;
            }
        }
        return e;
    }

    private int GenerateD(int phi, int e)
    {
        return MathG.ModInverse(e, phi);
    }

    private string RandomMessage()
    {
        int chars = 30;
        string msg = "";
        while (chars-- > 0)
        {
            msg += (char)UnityEngine.Random.Range('A', 'Z');
        }
        return msg;
    }
}
