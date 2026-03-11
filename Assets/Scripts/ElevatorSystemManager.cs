using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ElevatorSystemManager : MonoBehaviour {

    public static ElevatorSystemManager Instance { get; private set; }

    [SerializeField] private List<Elevator> elevators = new List<Elevator>();
    
    public event EventHandler OnResetAll;
    private DialogBoxUI dialogBoxUI;
    private Queue<Floors> floorsRequests = new Queue<Floors>();

    private void OnEnable() {

        FloorCallButton.OnRequestCallElevator += FloorCallButton_OnRequestCallElevator;
    }

    private void OnDisable() {

        FloorCallButton.OnRequestCallElevator -= FloorCallButton_OnRequestCallElevator;

    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(Instance);
        }
    }

    private void Start() {
        dialogBoxUI = DialogBoxUI.Instance;
        dialogBoxUI.OnResetButtonClicked += DialogBoxUI_OnResetButtonClicked;
        dialogBoxUI.OnElevatorSpeedChanged += DialogBoxUI_OnElevatorSpeedChanged;
    }

    public void RequestedFloorHandler(Floors newRequestedFloor) {

        if (!CheckDuplicateRequests(newRequestedFloor)) {
            return;
        }
        
        floorsRequests.Enqueue(newRequestedFloor);
        SendRequestToElevator(GetBestElevatorForRequest(newRequestedFloor), newRequestedFloor);
    }

    private ElevatorID GetBestElevatorForRequest(Floors requestedFloor) {

        Elevator bestElevator = null;
        int bestScore = int.MaxValue;

        foreach (Elevator elevator in elevators) {

            Floors currentFloor = elevator.GetCurrentFloor();
            ElevatorState currentState = elevator.GetElevatorState();


            if (currentState == ElevatorState.MovingUp && requestedFloor < currentFloor) {

                continue;
            }

            if (currentState == ElevatorState.MovingDown && requestedFloor > currentFloor) {

                continue;
            }
            
            int distance = Mathf.Abs((int)currentFloor - (int)requestedFloor);
            int score = distance;

            if (currentState == ElevatorState.Idle) {

                score -= 2;
            }

            if (score < bestScore) {

                bestScore = score;
                bestElevator = elevator;
            }
        }

        return bestElevator.GetElevatorDataSO().elevatorID;
    }

    private void SetAllElevatorsSpeed(int elevatorSpeed) {

        foreach (Elevator elevator in elevators) {

            elevators[GetElevatorIndex(elevator.GetElevatorDataSO().elevatorID)].SetElevatorMoveSpeed(elevatorSpeed);
        }

    }

    private void ResetAllElevator() {

        foreach (Elevator elevator in elevators) {

            elevators[GetElevatorIndex(elevator.GetElevatorDataSO().elevatorID)].ResetElevator();
        }

    }
    private void DialogBoxUI_OnResetButtonClicked(object sender, EventArgs e) {
        OnResetAll?.Invoke(this, EventArgs.Empty);
        ResetAllElevator();
        RestFloorQueue();
    }

    private void SendRequestsToAllElevators(Floors newRequestedFloor) {
        SendRequestToElevator(ElevatorID.Elevator1, newRequestedFloor);
        SendRequestToElevator(ElevatorID.Elevator2, newRequestedFloor);
        SendRequestToElevator(ElevatorID.Elevator3, newRequestedFloor);
    }

    private void FloorCallButton_OnRequestCallElevator(object sender, FloorCallButton.OnRequestCallElevatorEventArgs e) {
        RequestedFloorHandler(e.requestFromFloor);
    }

    private void DialogBoxUI_OnElevatorSpeedChanged(object sender, DialogBoxUI.OnElevatorSpeedChangedEventArgs e) {
        SetAllElevatorsSpeed(e.elevatorSpeed);
    }

    private void SendRequestToElevator(ElevatorID receiverElevatorID, Floors receiverFloor) {
        elevators[GetElevatorIndex(receiverElevatorID)].LoadNewRequestFromSystem(receiverFloor);
    }

    private bool CheckDuplicateRequests(Floors newRequestedFloor) {

        if (CheckDuplicateCalling(newRequestedFloor)) {
            Debug.Log(newRequestedFloor + " is already LAST added!!");
            return false;
        }

        //floorsRequests.Enqueue(newRequestedFloor);

        return true;
        //Debug.Log("Request queue count: " + floorsRequests.Count);
    }

    private bool CheckDuplicateCalling(Floors newRequestedFloor) {
        return floorsRequests.Count > 0 && floorsRequests.Last() == newRequestedFloor;
    }

    private int GetElevatorIndex(ElevatorID elevatorId) {

        for (int currentElevatorIndex = 0; currentElevatorIndex < elevators.Count; currentElevatorIndex++) {

            if (elevators[currentElevatorIndex].GetElevatorDataSO().elevatorID == elevatorId) {
                return currentElevatorIndex;
            }

        }

        Debug.LogError(elevatorId + " :- elevator index doesn't exists!!!");

        return -1;
    }

    public Queue<Floors> GetFloorsRequestsQueue() {
        return floorsRequests;
    }

    private void RestFloorQueue() {

        floorsRequests.Clear();
        
    }

    public void MoveElevatorsDefaultFloor() {

        foreach (Elevator elevator in elevators) {

            SendRequestToElevator(GetElevatorId(elevator), elevator.GetElevatorDataSO().defaultFloorPosition);

        }

    }

    private ElevatorID GetElevatorId(Elevator elevator) {
        return elevator.GetElevatorDataSO().elevatorID;
    }

}

