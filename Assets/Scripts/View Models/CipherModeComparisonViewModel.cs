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

        foreach (CipherMode cipherMode in Enum.GetValues(typeof(CipherMode)))
        {
            string encryptedText;
            string cipherModeName = Enum.GetName(typeof(CipherMode), cipherMode);
            try
            {
                var startTime = DateTime.Now;
                encryptedText = Cryptography.Encrypt(plainText, cipherMode);
                string message = DateTime.Now.Subtract(startTime).TotalSeconds.ToString("0.##") + " s";
                outputFieldContainer.AddOutputField(cipherModeName + " encryption").Value = message;
            }
            catch
            {
                outputFieldContainer.AddOutputField(cipherModeName).Value = "Not Supported";
                continue;
            }
            yield return null;

            try
            {
                var startTime = DateTime.Now;
                var decryptedText = Cryptography.Decrypt(encryptedText, cipherMode);
                string message = DateTime.Now.Subtract(startTime).TotalSeconds.ToString("0.##") + " s";
                outputFieldContainer.AddOutputField(cipherModeName + " decryption").Value = message;

                textFieldContainer.Clear();
                textFieldContainer.AddLongOutputField($"Decrypted Text with {Enum.GetName(typeof(CipherMode), cipherMode)}").Value = decryptedText.Substring(0, 2000);
            }
            catch
            {

            }
            yield return null;
        }
    }
}
