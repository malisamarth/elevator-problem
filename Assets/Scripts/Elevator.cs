using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    [SerializeField] private ElevatorDataSO elevatorDataSO;

    [SerializeField] private float elevatorMoveSpeed = 2f;

    [SerializeField] private ElevatorFloorRestPoints elevatorFloorRestPoints;

    List<Floors> movementQueue = new List<Floors>();

    private event EventHandler<RequestDataEventHandler> OnRequestLoaded;
    public class RequestDataEventHandler : EventArgs {
        public Floors floorRequest;

        public RequestDataEventHandler(Floors floorRequest) {
            this.floorRequest = floorRequest;
        }

    }

    private Floors currentActiveFloor;
    private Floors lastElevatorFloor;

    private Vector3 currentTargetFloorPosition;

    private ElevatorState currentElevatorState;

    private bool isWaitingAtFloor = false;
    private bool startMotion = false;

    private ElevatorDirectionLock activeElevatorDirectionLock;

    [SerializeField] private List<Floors> testingArray;


    private void Start() {
        DefaultMode();
        OnRequestLoaded += SortRequestQueue;
    }


    private void Update() {

        if (movementQueue.Count > 0 && !isWaitingAtFloor) {
            Floors nextFloor = movementQueue[0];

            Vector3 nextPosition = GetPositionByFloor(nextFloor);

            if (currentTargetFloorPosition != nextPosition) {
                SetCurrentFloorTarget(nextFloor);
            }
        }

        UpdateElevatorMovement();
        SetElevatorStatus();

        //Debug.Log(gameObject.name + " = " + currentElevatorState);
    }

    public void LoadNewRequestFromSystem(Floors targetFloor) {

        OnRequestLoaded?.Invoke(this, new RequestDataEventHandler(targetFloor));

        Debug.Log(targetFloor + " :- New floor request - " + gameObject.name);
    }

    private void SortRequestQueue(object sender, RequestDataEventHandler e) {

        Debug.Log(e.floorRequest + " :- new request for + " + gameObject.name);

        SortQueue(e.floorRequest);

    }

    private void SortQueue(Floors requestedFloor) {
        int requestedFloorIndex = (int)requestedFloor;

        for (int currentMovementQueueIndex = 0; currentMovementQueueIndex < movementQueue.Count; currentMovementQueueIndex++) {
            int queueFloorIndex = (int)movementQueue[currentMovementQueueIndex];

            if (requestedFloorIndex < queueFloorIndex) {
                movementQueue.Insert(currentMovementQueueIndex, requestedFloor);
                Debug.Log("Added new element :- " + requestedFloor);
                return;
            }
        }

        movementQueue.Add(requestedFloor);

        Debug.Log("Added new element at end :- " + requestedFloor);
    }

    private void SetCurrentFloorTarget(Floors floor) {
        lastElevatorFloor = currentActiveFloor;
        currentTargetFloorPosition = GetPositionByFloor(floor);
    }

    public void ResetElevator() {
        
        movementQueue.Clear();
        currentTargetFloorPosition = GetPositionByFloor(Floors.GroundFloor);

        transform.position = currentTargetFloorPosition;

        currentActiveFloor = Floors.GroundFloor;
        lastElevatorFloor = Floors.GroundFloor;

        currentElevatorState = ElevatorState.Idle;
    }

    private void DefaultMode() {

        currentTargetFloorPosition = GetPositionByFloor(Floors.GroundFloor);

        transform.position = currentTargetFloorPosition;

        currentActiveFloor = Floors.GroundFloor;
        lastElevatorFloor = Floors.GroundFloor;

        currentElevatorState = ElevatorState.Idle;
    }

    private void UpdateElevatorMovement() {
        if (currentElevatorState == ElevatorState.Idle) {

            if (movementQueue.Count != 0) {
                //movementQueue.RemoveAt(0);
            }

            return;
        }

        transform.position = MoveTowardsThisPosition(currentTargetFloorPosition);
    }

    private void SetElevatorStatus() {

        float distance = Vector3.Distance(transform.position, currentTargetFloorPosition);

        if (distance <= 0.01f) {
            transform.position = currentTargetFloorPosition;

            currentElevatorState = ElevatorState.Idle;
            currentActiveFloor = GetFloorByPosition(currentTargetFloorPosition);

            if (movementQueue.Count > 0 && !isWaitingAtFloor) {
                movementQueue.RemoveAt(0);
                StartCoroutine(WaitAndStartNextFloor(2f));
            }

            return;
        }

        if (transform.position.y < currentTargetFloorPosition.y) {
            currentElevatorState = ElevatorState.MovingUp;
        }
        else {
            currentElevatorState = ElevatorState.MovingDown;
        }
    }

    public List<Floors> GetMovementData() {
        return movementQueue;
    }

    private Vector3 MoveTowardsThisPosition(Vector3 targetPosition) {
        return Vector3.MoveTowards(
            transform.position,
            targetPosition,
            Time.deltaTime * elevatorMoveSpeed
        );
    }

    private Floors GetFloorByPosition(Vector3 floorPosition) {
        return elevatorFloorRestPoints.GetFloorByPosition(floorPosition);
    }

    private Vector3 GetPositionByFloor(Floors floor) {
        return elevatorFloorRestPoints.GetPositionByFloor(floor);
    }

    public ElevatorDataSO GetElevatorDataSO() {
        return elevatorDataSO;
    }

    public ElevatorState GetElevatorState() {
        return currentElevatorState;
    }

    public Floors GetCurrentFloor() {
        return currentActiveFloor;
    }

    IEnumerator WaitAndStartNextFloor(float waitTime) {
        isWaitingAtFloor = true;

        yield return new WaitForSeconds(waitTime);

        isWaitingAtFloor = false;
    }

    public int GetCurrentTargetFloor() {

        Floors floor = GetFloorByPosition(currentTargetFloorPosition);

        return (int)floor;
    }

    public void SetElevatorMoveSpeed(int moveSpeed) {
        elevatorMoveSpeed = moveSpeed;
    }

} 