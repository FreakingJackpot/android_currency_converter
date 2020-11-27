using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class main : MonoBehaviour
{
    public TextMeshProUGUI Currency1;
    public TextMeshProUGUI Currency2;
    public TMP_InputField InputField;
    public TextMeshProUGUI TextField;
    public TMP_Dropdown C1;
    public TMP_Dropdown C2;
    float s;
    readonly string Uri_base = "localhost:8000/convert/";
    readonly List<string> currencies =
        new List<string> { "RUB", "USD", "EUR", "AUD", "AZN", "GBP", "AMD", "BYN", "BGN", "BRL", "HUF", "HKD",
                           "DKK", "INR", "KZT", "CAD", "KGS", "CNY", "MDL", "NOK", "PLN", "RON", "XDR", "SGD",
                           "TJS", "TRY", "TMT", "UZS", "UAH", "CZK", "SEK", "CHF", "ZAR", "KRW", "JPY" };
    private void Start()
    {

        C1.AddOptions(currencies);
        C2.AddOptions(currencies);
    }

    public void Submit()
    {

        if (Currency1.text == Currency2.text)
            TextField.text = "Обе валюты одинаковые";
        else if (float.TryParse(InputField.text, out s) == true && s > 0)
        {
            string url =
                string.Format("localhost:8000/convert/{0}/{1}/{2}", Currency1.text, Currency2.text, InputField.text);
            StartCoroutine(Request(url));
        }
        else
            TextField.text = "Введена неверная сумма";
    }

    public IEnumerator Request(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            Responce responce = JsonUtility.FromJson<Responce>(request.downloadHandler.text);
            TextField.text = "Результат:" + responce.result;
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}

public class Responce
{
    public string currency_before;
    public string currency_after;
    public string result;
}
