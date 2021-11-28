using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum GameState
    {
        playing,
        scoring,
        end
    }
    private bool affected = false;
    private Vector3 appliedPos;
    private float _speed;
    GameState current;
    public ArrowController arrowController;
    public Text scoreText;
    //panel
    public GameObject panel;
    public Text panelScoreText;
    private void Awake()
    {
        _speed = 10;
        Cursor.lockState = CursorLockMode.Locked;
        current = GameState.playing;
    }
    private void Start()
    {
    }

    void Update()
    {
        if (current == GameState.end)
        {
            if (!panel.activeSelf)
            {
                panel.SetActive(true);
                panelScoreText.text = "Score: " + score;
                scoreText.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined;
            }
            return;
        }
        if (current == GameState.scoring)
        {

            appliedPos += Vector3.forward * (Time.deltaTime * _speed * 1.2f);
            appliedPos.x = 0;
            transform.position = appliedPos;
            return;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            _speed += .1f;
        }

        appliedPos = transform.position;
        if (Input.GetAxis("Mouse X") > 0)
        {
            appliedPos += Vector3.right * (Time.deltaTime * _speed);
            if (appliedPos.x > 2.3)
            {
                appliedPos.x = 2.3f;
            }
        }
        else if (Input.GetAxis("Mouse X") < 0)
        {
            appliedPos += Vector3.left * (Time.deltaTime * _speed);
            if (appliedPos.x < -2.3f)
            {
                appliedPos.x = -2.3f;
            }
        }
        appliedPos += Vector3.forward * (Time.deltaTime * _speed);
        transform.position = appliedPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        var gateController = other.GetComponent<GateController>();
        if (gateController != null && !affected)
        {
            var arrowController = GetComponent<ArrowController>();
            arrowController.ProcessTrigger(gateController.sign, gateController.number);
            affected = true;
        }
        if (other.CompareTag("Finish"))
        {
            current = GameState.scoring;
            StartCoroutine(Scoring());
        }
    }
    float score;
    IEnumerator Scoring()
    {
        float time = 0;
        while (arrowController.arrowCount > 0)
        {
            time += Time.deltaTime;
            if (time > 1 / 30)
            {
                score++;
                arrowController.arrowCount--;
                scoreText.text = "Score: " + score;
                time = 0;
                yield return null;
            }
        }
        current = GameState.end;
    }
    private void OnTriggerExit(Collider other)
    {
        var gateController = other.GetComponent<GateController>();
        if (gateController != null)
        {
            affected = false;
        }
    }
}