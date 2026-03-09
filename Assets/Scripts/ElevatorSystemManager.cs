using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ElevatorSystemManager : MonoBehaviour {

    public static ElevatorSystemManager Instance { get; private set; }

    public event EventHandler OnResetAll;

    [SerializeField] private List<Elevator> elevators = new List<Elevator>();

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

    private void FloorCallButton_OnRequestCallElevator(object sender, FloorCallButton.OnRequestCallElevatorEventArgs e) {
        RequestedFloorHandler(e.requestFromFloor);
    }

    public void RequestedFloorHandler(Floors newRequestedFloor) {

        AddToFloorsRequestQueue(newRequestedFloor);

        SendRequestToElevator(GetBestElevatorForRequest(), newRequestedFloor);

    }

    private ElevatorID GetBestElevatorForRequest() {

        return ElevatorID.Elevator1;
    }

    private void SendRequestToElevator(ElevatorID receiverElevatorID, Floors receiverFloor) {
        elevators[GetElevatorIndex(receiverElevatorID)].LoadNewRequestFromSystem(receiverFloor);
    }

    private void AddToFloorsRequestQueue(Floors newRequestedFloor) {

        if (CheckDuplicateCalling(newRequestedFloor)) {
            Debug.Log(newRequestedFloor + " is already LAST added!!");
            return;
        }

        floorsRequests.Enqueue(newRequestedFloor);

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

    public void RestAll() {

        floorsRequests.Clear();
        MoveElevatorsDefaultFloor();
        OnResetAll?.Invoke(this, EventArgs.Empty);
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

