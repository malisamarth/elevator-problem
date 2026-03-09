using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class ElevatorDialogBoxUI : MonoBehaviour {

    [SerializeField] private Elevator elevator;

    [SerializeField] private TextMeshProUGUI elevatorMovementQueue;

    private void Update() {

        List<Floors> queue = elevator.GetMovementQueue();

        SetDialogBoxRequestQueue(queue);
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

}