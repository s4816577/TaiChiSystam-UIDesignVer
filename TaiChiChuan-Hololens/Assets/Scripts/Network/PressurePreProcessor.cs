using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class PressurePreProcessor
{
    public delegate void OnPressureChangedHandler(float left, float right);
    public event OnPressureChangedHandler OnPressureChangedEvent;

    public delegate void OnFootLiftCompleteHandler();
    public event OnFootLiftCompleteHandler OnRightFootCompleteEvent;
    public event OnFootLiftCompleteHandler OnLeftFootCompleteEvent;

    private const int calibrationDataNumber = 100;
    private float startCalibrationTime;
    private bool isLeftFullCalibration = false;
    private bool isRightFullCalibration = false;
    private List<float> leftData;
    private List<float> rightData;
    private float leftEmpty = 0.0f;
    private float rightEmpty = 0.0f;
    private float leftFull = 0.0f;
    private float rightFull = 0.0f;
    private float multiply = 1.0f;
    private bool isCalibrated;

    private float currentLeft = 0.0f;
    private float currentRight = 0.0f;
    private float leftRatio = 0.5f;


    public PressurePreProcessor(UdpNetworkServer udpNetworkServer)
    {
        udpNetworkServer.OnPressureReceivedEvent += ProcessPressureData;
    }

    public void LeftFullCalibration()
    {
        isLeftFullCalibration = true;
        leftData = new List<float>();
        rightData = new List<float>();
    }

    public void RightFullCalibration()
    {
        isRightFullCalibration = true;
        leftData = new List<float>();
        rightData = new List<float>();

        startCalibrationTime = Time.time;
    }

    public void Reset()
    {
        multiply = 0.0f;
        isCalibrated = false;
    }

    public void Apply()
    {
        if (leftFull == 0.0f || rightFull == 0.0f)
            leftFull = rightFull = 1.0f;

        multiply = (rightFull - rightEmpty) / (leftFull - leftEmpty);
        isCalibrated = true;
    }

    private void ProcessPressureData(byte[] data)
    {
        string str = Encoding.UTF8.GetString(data);
        if (str.Length > 0)
        {
            if (str[0] == 'r')
            {
                currentRight = int.Parse(str.Substring(1));
                if (isLeftFullCalibration || isRightFullCalibration)
                {
                    rightData.Add(currentRight);
                }
            }
            else if (str[0] == 'l')
            {
                currentLeft = int.Parse(str.Substring(1));
                if (isLeftFullCalibration || isRightFullCalibration)
                {
                    leftData.Add(currentLeft);
                }
            }
            else
                return;

            if (isLeftFullCalibration && leftData.Count > calibrationDataNumber && rightData.Count > calibrationDataNumber)
            {
                leftFull = rightEmpty = 0.0f;

                foreach (float val in leftData)
                    leftFull += val;
                if (leftData.Count == 0)
                    leftFull = 0.0f;
                else
                    leftFull = leftFull / leftData.Count;

                foreach (float val in rightData)
                    rightEmpty += val;
                if (rightData.Count == 0)
                    rightEmpty = 0.0f;
                else
                    rightEmpty = rightEmpty / rightData.Count;

                isLeftFullCalibration = false;
                UserInterface.GetInstance().SetCommandQueue("Right Foot Lift Complete: " + leftData.Count.ToString() + ", " + rightData.Count.ToString());
                OnRightFootCompleteEvent.Invoke();
            }
            else if (isRightFullCalibration && leftData.Count > calibrationDataNumber && rightData.Count > calibrationDataNumber)
            {
                rightFull = leftEmpty = 0.0f;

                foreach (float val in leftData)
                    leftEmpty += val;
                if (leftData.Count == 0)
                    leftEmpty = 0.0f;
                else
                    leftEmpty = leftEmpty / leftData.Count;

                foreach (float val in rightData)
                    rightFull += val;
                if (rightData.Count == 0)
                    rightFull = 0.0f;
                else
                    rightFull = rightFull / rightData.Count;

                isRightFullCalibration = false;
                UserInterface.GetInstance().SetCommandQueue("Left Foot Lift Complete: " + leftData.Count.ToString() + ", " + rightData.Count.ToString());
                OnLeftFootCompleteEvent.Invoke();
            }

            if (currentLeft + currentRight == 0)
                leftRatio = 0.5f;
            else if (isCalibrated)
            {
                currentLeft -= leftEmpty;
                currentRight -= rightEmpty;
                leftRatio = currentLeft / (currentLeft * multiply + currentRight) * multiply;
                if (leftRatio < 0.0f)
                    leftRatio = 0.0f;
                if (leftRatio > 1.0f)
                    leftRatio = 1.0f;
            }
            else
            {
                leftRatio = currentLeft / (currentLeft + currentRight);
            }
            if (OnPressureChangedEvent != null)
                OnPressureChangedEvent.Invoke(leftRatio, 1.0f - leftRatio);
        }
    }
}
