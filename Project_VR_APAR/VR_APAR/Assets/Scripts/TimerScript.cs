using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField]private float elapsedTime;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject leftRay;
    [SerializeField] private GameObject rightRay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > 0)
        {
        elapsedTime -= Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
            endCanvas.SetActive(true);
            leftRay.SetActive(true);
            rightRay.SetActive(true);
        }
        //timerText.text = elapsedTime.ToString();
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{00:00}:{01:00}", minutes, seconds);
    }
}
