using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

public class BBSViewModel : MonoBehaviour
{
    public FieldContainer inputFieldContainer, outputFieldContainer, testFieldContainer;

    private const int randomSize = 20000;

    private void Start()
    {
        inputFieldContainer.AddButton("Menu").OnClickAsObservable()
            .Subscribe(_ =>
            {
                ViewManager.main.CurrentView = ViewConfig.Views.Menu;
            })
            .AddTo(this);

        var pField = inputFieldContainer.AddInputField("p", "2503");
        var qField = inputFieldContainer.AddInputField("q", "3119");
        var calculateButton = inputFieldContainer.AddButton("Calculate");

        Observable.Merge(pField.InputProperty.AsUnitObservable(),
            qField.InputProperty.AsUnitObservable(),
            calculateButton.OnClickAsObservable())
            .Subscribe(_ =>
            {
                try
                {
                    int p = int.Parse(pField.Value);
                    int q = int.Parse(qField.Value);
                    int n = p * q;
                    long x = GetCorrectX(p, q, n);

                    outputFieldContainer.Clear();

                    var nField = outputFieldContainer.AddOutputField("N");
                    nField.Value = n.ToString();

                    var xField = outputFieldContainer.AddOutputField("x0");
                    xField.Value = x.ToString();

                    StringBuilder randomBits = new StringBuilder(randomSize);
                    for (int i = 0; i < randomSize; i++)
                    {
                        randomBits.Append(x % 2);
                        x = (x * x) % n;
                    }

                    var randomField = outputFieldContainer.AddLongOutputField("Random");
                    string random = randomBits.ToString();
                    randomField.Value = random;

                    testFieldContainer.Clear();

                    testFieldContainer.AddOutputField("Test pojedynczych bitów").Value = Test1(random).ToString();
                    testFieldContainer.AddOutputField("Test serii").Value = Test2(random).ToString();
                    testFieldContainer.AddOutputField("Test długiej serii").Value = Test3(random).ToString();
                    testFieldContainer.AddOutputField("Test pokerowy").Value = Test4(random).ToString();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }).AddTo(this);
    }

    private bool Test1(string random)
    {
        var count = random.Count(c => c == '1');
        return count > 9725 && count < 10275;
    }

    private bool Test2(string random)
    {
        var cDict = new Dictionary<int, int>();

        int lastC = 0;
        char c = random[0];
        int length;
        for (int i = 1; i <= random.Length; i++)
        {
            if (i == random.Length || random[i] != c)
            {
                length = i - lastC;
                if (!cDict.ContainsKey(length))
                {
                    cDict[length] = 0;
                }
                if (c == '1')
                {
                    cDict[length]++;
                }
                if (i != random.Length)
                {
                    c = random[i];
                    lastC = i;
                }
            }
        }

        var count6 = cDict.Where(kvp => kvp.Key >= 6).Select(kvp => kvp.Value).Sum();
        return cDict[1] > 2315 && cDict[1] < 2685
            && cDict[2] > 1114 && cDict[2] < 1386
            && cDict[3] > 527 && cDict[3] < 723
            && cDict[4] > 240 && cDict[4] < 384
            && cDict[5] > 103 && cDict[5] < 209
            && count6 > 103 && count6 < 209;
    }

    private bool Test3(string random)
    {
        var cDict = new Dictionary<int, int>();

        int lastC = 0;
        char c = random[0];
        int length;
        for (int i = 1; i <= random.Length; i++)
        {
            if (i == random.Length || random[i] != c)
            {
                length = i - lastC;
                if (!cDict.ContainsKey(length))
                {
                    cDict[length] = 0;
                }
                if (c == '1')
                {
                    cDict[length]++;
                }
                if (i != random.Length)
                {
                    c = random[i];
                    lastC = i;
                }
            }
        }

        return !cDict.Keys.Any(key => key >= 26);
    }

    private bool Test4(string random)
    {
        var cDict = new Dictionary<int, int>();

        for (int i = 0; i * 4 < random.Length; i++)
        {
            int value = random[i] * 8 + random[i + 1] * 4 + random[i + 2] * 2 + random[i + 3];
            if (!cDict.ContainsKey(value))
            {
                cDict[value] = 0;
            }
            cDict[value]++;
        }

        var x = 16.0 / 5000.0 * cDict.Values.Select(v => 1.0 * v * v).Sum() - 5000;

        return x > 2.16 && x < 46.17;
    }

    private int GetCorrectX(int p, int q, int n)
    {
        int start;
        do
        {
            start = UnityEngine.Random.Range(1, n);
        } while (start % p == 0 || start % q == 0);
        return start;
    }

    private int GetCorrectPrime()
    {
        int start = UnityEngine.Random.Range(1000, 5000);
        do
        {
            start = NextPrime(start + 1);
        } while (start % 4 != 3);
        return start;
    }

    private int NextPrime(int start)
    {
        while (!IsPrime(start))
        {
            start++;
        }
        return start;
    }

    private bool IsPrime(int number)
    {
        for (int i = 2; i * i <= number; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}
