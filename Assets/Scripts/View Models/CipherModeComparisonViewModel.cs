using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;

public class CipherModeComparisonViewModel : MonoBehaviour
{
    public FieldContainer inputFieldContainer, outputFieldContainer, textFieldContainer;
    private InputFieldController textFileInput;

    private void Start()
    {
        inputFieldContainer.AddMenuButton();

        textFileInput = inputFieldContainer.AddInputField("Plain Text File", "lorem.txt");

        inputFieldContainer.AddButton("Run Ciphers").OnClickAsObservable().
            Subscribe(_ =>
            {
                StartCoroutine(CipherComparison());
            }).AddTo(this);

    }

    private IEnumerator CipherComparison()
    {
        string path = Path.Combine(Application.streamingAssetsPath, textFileInput.Value);
        var plainText = File.ReadAllText(path);
        outputFieldContainer.Clear();

        string message;
        foreach (CipherMode cipherMode in Enum.GetValues(typeof(CipherMode)))
        {
            try
            {
                var startTime = DateTime.Now;
                var encryptedtext = Cryptography.Encrypt(plainText, cipherMode);
                var decryptedtext = Cryptography.Decrypt(encryptedtext, cipherMode);
                message = DateTime.Now.Subtract(startTime).TotalMilliseconds.ToString("0.##") + " ms";
                textFieldContainer.Clear();
                textFieldContainer.AddLongOutputField($"Decrypted Text with {Enum.GetName(typeof(CipherMode), cipherMode)}").Value = decryptedtext.Substring(0, 2000);
            }
            catch
            {
                message = "Not Supported";
            }
            outputFieldContainer.AddOutputField(Enum.GetName(typeof(CipherMode), cipherMode)).Value = message;
            yield return null;
        }
    }
}
