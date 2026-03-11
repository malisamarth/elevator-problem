using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorDialogBoxUI : MonoBehaviour {

    [SerializeField] private Elevator elevator;

    [SerializeField] private TextMeshProUGUI elevatorMovementQueue;

    [SerializeField] private TextMeshProUGUI currentTargetFloorText;

    private void Update() {

        List<Floors> queue = elevator.GetMovementData();

        SetDialogBoxRequestQueue(queue);

        SetCurrentTargetFloorText();
    }

    private void SetDialogBoxRequestQueue(List<Floors> floors) {

        string display = string.Empty;

        for (int index = 0; index < floors.Count; index++) {

            if (index == floors.Count - 1) {
                display += (int)floors[index];
            } else {
                display += (int)floors[index] + ", ";
            }
        }

        elevatorMovementQueue.text = "[" + display + "]";

    }

    private void SetCurrentTargetFloorText() {

        elevator.GetCurrentTargetFloor();

        currentTargetFloorText.text = elevator.GetCurrentTargetFloor().ToString();
    }

}