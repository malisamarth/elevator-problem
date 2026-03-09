using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogBoxUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI queueText;

    private ElevatorSystemManager elevatorSystemManager;

    private List<int> convertedList = new List<int>();

    private void Start() {
        elevatorSystemManager = ElevatorSystemManager.Instance;
        elevatorSystemManager.OnResetAll += ElevatorSystemManager_OnResetAll1;
    }

    private void ElevatorSystemManager_OnResetAll1(object sender, EventArgs e) {
        convertedList.Clear();
        EmptyAll();
    }

    private void Update() {

        //queueText.text = string.Join(", ", GetFloorsRequestsList().ToArray());

        SetDialogBoxRequestQueue(GetFloorsRequestsList().ToArray());

    }

    private void SetDialogBoxRequestQueue(int[] floorRequestArray) {

        if (floorRequestArray.Length == 0) {
            return;
        }

        if (convertedList.Count == 0 || floorRequestArray.Last() != convertedList.Last()) {
            convertedList = floorRequestArray.ToList();
            DisplayFloorRequests(convertedList);
        }
    }

    private void DisplayFloorRequests(List<int> list) {
        queueText.text = "[ " + string.Join(", ", list) + " ]";
    }

    private List<int> GetFloorsRequestsList() {

        List<Floors> floorsRequestsQueue = elevatorSystemManager.GetFloorsRequestsQueue().ToList();
        List<int> floorsAsInt = floorsRequestsQueue.Select(floor => (int)floor).ToList();

        return floorsAsInt;


    }

    private void EmptyAll() {
        queueText.text = "[  ]";
    }
}