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
    public TMP_InputField TextField;
    public TMP_Dropdown C1;
    public TMP_Dropdown C2;
    float s;
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
        TextField.text = "";

        if (Currency1.text == Currency2.text)
            TextField.text = "Валюты одинаковые";
        else if (float.TryParse(InputField.text.Replace('.', ','), out s) == true && s > 0)
        {
            StartCoroutine(Request());
        }
        else
            TextField.text = "Неверная сумма";
    }

    public IEnumerator Request()
    {
        WWWForm form = new WWWForm();
        form.AddField("before", Time.frameCount.ToString());
        form.AddField("after", Time.frameCount.ToString());
        form.AddField("value", Time.frameCount.ToString());

        string url = "localhost:8000/course/?before=" + Currency1.text + "&after=" + Currency2.text +
                     "&value=" + InputField.text.Replace(',', '.');

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            Responce responce = JsonUtility.FromJson<Responce>(request.downloadHandler.text);

            float tmp = float.Parse(responce.result.Replace('.', ','));

            TextField.text = tmp.ToString("F4");
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

public class RequestJson
{
    public string before;
    public string after;
    public string value;
}
