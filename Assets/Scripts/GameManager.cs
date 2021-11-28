using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class Level
    {
        public int length;
        public int gateCount;
    }
    public List<Level> levels;
    public GameObject roadPrefab;
    public GameObject gatePrefab;
    public GameObject finishPrefab;
    public GameObject multiplierPrefab;
    public float gateUnitZ;
    int currentLevel;
    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("Level", 0);
    }
    private void Start()
    {
        CreateLevel(currentLevel);
    }
    //Dynamic random level generator
    void CreateLevel(int level)
    {
        var road = Instantiate(roadPrefab, Vector3.zero, Quaternion.identity);
        var localScale = road.transform.localScale;
        localScale.z *= levels[level].length;
        road.transform.localScale = localScale;
        CreateGates(road, levels[level].gateCount);
        CreateFinish(levels[level].length);
    }
    void CreateGates(GameObject parent, int gateCount)
    {
        for (int i = 0; i < gateCount; i++)
        {
            var instantiated = Instantiate(gatePrefab, parent.transform);
            var appliedPos = new Vector3(0, .6f, (i + 1));
            appliedPos.y *= parent.transform.localScale.y;
            appliedPos.z *= parent.transform.localScale.z / gateCount;
            instantiated.transform.position = appliedPos;
            if (i % 3 == 0)
            {
                var components = instantiated.GetComponentsInChildren<GateController>();
                foreach (var c in components)
                {
                    c.SetPositive();
                }
            }
            else if (i % 3 == 1)
            {
                var components = instantiated.GetComponentsInChildren<GateController>();
                components[0].SetNegative();
                components[1].SetPositive();
            }
            else
            {
                var components = instantiated.GetComponentsInChildren<GateController>();
                foreach (var c in components)
                {
                    c.SetNegative();
                }
            }
        }
    }
    void CreateFinish(int Z)
    {
        var parent = Instantiate(finishPrefab, new Vector3(0, 0, Z + 1), Quaternion.identity);
        float multiplyFactor = 1.0f;
        for (int i = 0; i < 100; i++)
        {
            var multiplier = Instantiate(multiplierPrefab, Vector3.zero, Quaternion.identity, parent.transform);
            multiplier.transform.localPosition = new Vector3(0, 0, (i + 1) * 2);
            var textComponent = multiplier.GetComponentInChildren<TextMesh>();
            textComponent.text = "x" + (multiplyFactor + (i + 1) * .1f);
        }
    }
    public void NextLevel()
    {
        if (currentLevel + 1 == levels.Count)
        {
            return;
        }
        currentLevel++;
        PlayerPrefs.SetInt("Level", currentLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Level", 0);
    }
}
