using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{
    public int arrowCount;
    private int _lastArrowCount;
    public Transform arrowsParent;
    public Text arrowCountText;
    ObjectPooler pooler;
    private void Awake()
    {
        pooler = ObjectPooler.Instance;
    }
    void Start()
    {
        foreach (Transform child in arrowsParent)
        {
            Destroy(child.gameObject);
        }
        _lastArrowCount = arrowCount;
        UpdateArrowCircle(arrowCount);
    }

    private void Update()
    {
        if (arrowCount != _lastArrowCount)
        {
            var count = UpdateArrowCircle(arrowCount);
            arrowCountText.text = count.ToString();
            _lastArrowCount = arrowCount;
        }
    }

    public void ProcessTrigger(Sign sign, int number)
    {
        switch (sign)
        {
            case Sign.Addition:
                arrowCount += number;
                break;
            case Sign.Subtraction:
                arrowCount -= number;
                if (arrowCount < 0)
                    arrowCount = 1;
                break;
            case Sign.Multiplication:
                arrowCount *= number;
                break;
            case Sign.Division:
                arrowCount /= number;
                if (arrowCount < 1)
                    arrowCount = 1;
                break;
        }
    }
    int UpdateArrowCircle(int c)
    {
        const float r = .1f;
        var pos = transform.position;
        foreach (Transform child in arrowsParent)
        {
            pooler.ReturnToPool("arrow", child.gameObject);
        }

        int instantiatedCount = 0;
        for (float i = 0.1f; i < 1.8f; i += r) // diameter
        {
            var angle = 0f;
            var perimeter = 2 * Mathf.PI * i;
            int count = (int)(perimeter / r);
            float increaseAngle = 360 / (float)count;
            for (int j = 0; j <= count; j++) //points at circle
            {
                var appliedAngle = angle * Mathf.Deg2Rad;
                var x = Mathf.Cos(appliedAngle) * i;
                var y = Mathf.Sin(appliedAngle) * i;
                var z = pos.z;
                x += pos.x;
                y += pos.y;
                pooler.InstantiateFromPool("arrow", new Vector3(x, y, z), Quaternion.identity);
                angle += increaseAngle;
                instantiatedCount++;
                if (instantiatedCount >= c)
                {
                    return instantiatedCount;
                }
            }
        }
        return instantiatedCount;
    }
}