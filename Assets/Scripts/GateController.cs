using UnityEngine;
using Random = UnityEngine.Random;

public class GateController : MonoBehaviour
{
    public Sign sign;
    public int number;
    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        sign = Util.GetRandomFromEnum<Sign>();
        var rendererComponent = GetComponent<Renderer>();
        if (sign == Sign.Addition || sign == Sign.Multiplication)
        {
            rendererComponent.material.SetColor("_Color", Color.cyan);
            if (sign == Sign.Addition)
            {
                number = Random.Range(1, 40);
                transform.GetChild(0).GetComponent<TextMesh>().text = "+" + number;
            }
            else
            {
                number = Random.Range(1, 4);
                transform.GetChild(0).GetComponent<TextMesh>().text = "x" + number;
            }
        }
        else
        {
            rendererComponent.material.SetColor("_Color", Color.red);
            if (sign == Sign.Subtraction)
            {
                number = Random.Range(1, 40);
                transform.GetChild(0).GetComponent<TextMesh>().text = "-" + number;
            }
            else
            {
                number = Random.Range(1, 4);
                transform.GetChild(0).GetComponent<TextMesh>().text = "÷" + number;
            }
        }
    }
    public void SetPositive()
    {
        sign = Util.GetRandomFromEnum<Sign>();
        while (sign != Sign.Addition && sign != Sign.Multiplication)
        {
            sign = Util.GetRandomFromEnum<Sign>();

        }
        var rendererComponent = GetComponent<Renderer>();
        rendererComponent.material.SetColor("_Color", Color.cyan);
        if (sign == Sign.Addition)
        {
            number = Random.Range(1, 40);
            transform.GetChild(0).GetComponent<TextMesh>().text = "+" + number;
        }
        else
        {
            number = Random.Range(1, 4);
            transform.GetChild(0).GetComponent<TextMesh>().text = "x" + number;
        }

    }
    public void SetNegative()
    {
        sign = Util.GetRandomFromEnum<Sign>();
        while (sign != Sign.Subtraction && sign != Sign.Division)
        {
            sign = Util.GetRandomFromEnum<Sign>();

        }
        var rendererComponent = GetComponent<Renderer>();
        rendererComponent.material.SetColor("_Color", Color.red);
        if (sign == Sign.Subtraction)
        {
            number = Random.Range(1, 30);
            transform.GetChild(0).GetComponent<TextMesh>().text = "-" + number;
        }
        else
        {
            number = Random.Range(1, 4);
            transform.GetChild(0).GetComponent<TextMesh>().text = "÷" + number;
        }
    }
}