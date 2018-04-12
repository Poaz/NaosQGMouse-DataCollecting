﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public InputField IDInput;
    public string ID;
    public GameObject IDWarning, b1G, b2G, input;
    public ExcelStore data;
    public ReceiveLiveStream rls;
    public GameObject red, green;
    public RestartData RD;
    public Button b1, b2,b3;
    public float conationValue;
    public Text conationTextValue;

    public Slider slider, conationSlider;


    public Image calibrationImage, knob;

    public void Start()
    {
        RD = GetComponent<RestartData>();   
        rls =  GameObject.FindGameObjectWithTag("Data").GetComponent<ReceiveLiveStream>();
        //data = GameObject.FindGameObjectWithTag("Data").GetComponent<ExcelStore>();
        slider.maxValue = (float) data.BaseLineTime;
    }
    void Update()
    {
        slider.value = data.loading;
    }

    public void SetID()
    {
        data = GameObject.FindGameObjectWithTag("Data").GetComponent<ExcelStore>();
        

       if (IDInput.text != PlayerPrefs.GetString("ID"))
        {
            PlayerPrefs.SetString("ID", IDInput.text);
            PlayerPrefs.Save();
            data.ID = IDInput.text;
            ID = IDInput.text;
            IDWarning.SetActive(false);
            print("make participant");
        }
        else
        {
            IDWarning.SetActive(true);
        }

        if (IDInput.text == null)
        {
            b1.interactable = false;
            b2.interactable = false;
        } else
        {
            b1.interactable = true;
            b3.interactable = true;
        }
    }

    public void RecordOn()
    {
        red.SetActive(false);
        green.SetActive(true);
        input.SetActive(false);
        b1.interactable = false;
        b2.interactable = true;
        data.RecordOn();
    }

    public void RecordOff()
    {
        data.PlaceLabels(conationValue);
        data.RecordOff();
        red.SetActive(true);
        green.SetActive(false);
        StartCoroutine(RestartData());
        input.SetActive(true);
        b1.interactable = true;
        b2.interactable = false;
    }

    public void Calibrate()
    {
        rls.StartCalibration();
        calibrationImage.gameObject.SetActive(true);
        CalibrationSucceful();
    }

    public void CalibrationSucceful()
    {
        b3.GetComponentInChildren<Text>().text = "Calibration Complete";
        b3.interactable = false;
    }

    IEnumerator RestartData()
    {
        yield return new WaitForSeconds(2f);
        RD.DestroyData();
    }

    public void RecordBaseline()
    {
        data.ObtainBaseline();
        knob.color = Color.white;
        b3.GetComponentInChildren<Text>().text = "Keep hand on mouse";
        b3.interactable = false;

    }

    public void BaseLineObtained()
    {
        knob.color = Color.green;
    }

    public void StartCapture()
    {
        data.PlaceLabels(conationValue);
        data.running = true;
    }

    public void StopCapture()
    {
        data.running = false;
    }

    public void ConationLevel()
    {
       conationValue = conationSlider.value;
       conationTextValue.text = conationValue.ToString();
    }
    
}
