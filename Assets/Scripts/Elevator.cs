using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Elevator : MonoBehaviour {

    [SerializeField] private ElevatorDataSO elevatorDataSO;

    [SerializeField] private float elevatorMoveSpeed = 2f;

    [SerializeField] private ElevatorFloorRestPoints elevatorFloorRestPoints;

    List<Floors> movementQueue = new List<Floors>();

    private Vector3 currentTargetFloorPosition;

    private ElevatorState currentElevatorState;

    private Floors currentElevatorFloor = Floors.GroundFloor;
    private Floors newTargetFloor = Floors.None;
    private Floors lastTargetFloor;

    private bool isWaitingAtFloor = false;

    [SerializeField] private List<Floors> testingArray;

    private bool startMotion = false;

    private void Start() {

        SetInActiveRestFloor();
        SetCurrentElevatorFloor(Floors.GroundFloor);
        ChangeCurrentElevatorState(ElevatorState.Idle);
        SetLastTargetFloor(currentElevatorFloor);


        ExecuteMovementQueue();
    }


    private void Update() {
        if (currentElevatorState == ElevatorState.Idle || isWaitingAtFloor) {

            return;
        }

        SetElevatorFloorPosition();

        if (HasElevatorReachedTargetFloor(newTargetFloor) && !isWaitingAtFloor) {
            StartCoroutine(WaitForTime(2f));
        }
    }

    IEnumerator WaitForTime(float time) {
        isWaitingAtFloor = true;

        yield return new WaitForSeconds(time);

        isWaitingAtFloor = false;
        RemoveRequest();
        OnElevatorReachedFloor();
    }

    //Data from Manager

    public void LoadNewRequestFromSystem(Floors targetFloor) {
        InsertNewRequestIntoQueue(targetFloor);



        if (currentElevatorState == ElevatorState.Idle && movementQueue.Count > 0 && startMotion) {
            ExecuteMovementQueue();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////


    private void InsertNewRequestIntoQueue(Floors givenTargetFloor) {
        
        movementQueue.Add(givenTargetFloor);



            
            startMotion = true;
      
        


    }

    ////////////////////////////////////////////////////////////////////////////////////

    private void ExecuteMovementQueue() {
        if (movementQueue.Count == 0) {
            Debug.Log(gameObject.name + " :- completed all requests");
            return;
        }
        SortAscending();

        Floors nextFloor = movementQueue[0];

        //RemoveRequest();

        MovingToFloor(nextFloor);

        Debug.Log(gameObject.name + " :- moving to " + nextFloor);
    }

    private void AddRequestToMovementQueue(int queueIndexPosition, Floors floorToInsert) {

        movementQueue.Insert(queueIndexPosition, floorToInsert);

    }

    private void RemoveRequest() {
        
        movementQueue.RemoveAt(0);

    }

    private void SortAscending() {
        movementQueue.Sort((floorA, floorB) => floorA.CompareTo(floorB));
    }

    private void MovingToFloor(Floors targetFloor) {

        newTargetFloor = targetFloor;

        currentElevatorState = GetLiftDirection(lastTargetFloor, targetFloor);

        if (currentElevatorState != ElevatorState.Idle) {
            OnElevatorMoving();
        }

        currentTargetFloorPosition = GetFloorPosition(targetFloor);
    }

    private void OnElevatorReachedFloor() {
        transform.position = currentTargetFloorPosition;
        SetCurrentElevatorFloor(newTargetFloor);
        SetLastTargetFloor(newTargetFloor);

        ChangeCurrentElevatorState(ElevatorState.Idle);

        Debug.Log(gameObject.name + " reached floor: " + newTargetFloor);

        ExecuteMovementQueue();
    }

    private Vector3 MoveTowardsThisPosition(Vector3 targetPosition) {

        Vector3 changinigTargetPosition = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * elevatorMoveSpeed);

        return changinigTargetPosition;
    }
    
    private void SetInActiveRestFloor() {

        currentTargetFloorPosition = GetFloorPosition(Floors.GroundFloor);
        transform.position = currentTargetFloorPosition;

    }

    private Vector3 GetFloorPosition(Floors floorType) {
        return elevatorFloorRestPoints.GetFloorPosition(floorType);
    }

    private void SetElevatorFloorPosition() {
        transform.position = MoveTowardsThisPosition(currentTargetFloorPosition);
    }

    public ElevatorDataSO GetElevatorDataSO() {
        return elevatorDataSO;
    }

    private void ChangeCurrentElevatorState(ElevatorState elevatorState) {
        currentElevatorState = elevatorState;
    }

    private ElevatorState GetLiftDirection(Floors currentFloor, Floors targetFloor) {

        if ((int)currentFloor < (int)targetFloor) {
            //OnElevatorMoving();
            return ElevatorState.MovingUp;
        } 
        
        if ((int)currentFloor > (int)targetFloor) {
            //OnElevatorMoving();
            return ElevatorState.MovingDown;
        }

        return ElevatorState.Idle;

    }

    private void SetCurrentElevatorFloor(Floors setFloors) {
        currentElevatorFloor = setFloors;
    }

    private Floors GetCurrentElevatorFloor() {
        return currentElevatorFloor;
    }

    private void OnElevatorMoving() {
        currentElevatorFloor = Floors.InBetween;
    }

    private void SetLastTargetFloor(Floors floors) {
        lastTargetFloor = floors;
    }

    private bool HasElevatorReachedTargetFloor(Floors targetFloor) {

        Vector3 targetPosition = elevatorFloorRestPoints.GetFloorPosition(targetFloor);

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= 0.01f) {
            Debug.Log(gameObject.name + " reached " + targetFloor);
            return true;
        } else {
            //Debug.Log(gameObject.name + " :- has NOT reached " + targetFloor);
            return false;
        }
    }

    public List<Floors> GetMovementQueue() {
        return movementQueue;
    }

    public bool IsElevatorIdle() {
        return currentElevatorState == ElevatorState.Idle;
    }

    private int GetElevatorIndex(Floors floor) {
            
        return (int)floor;

    }

} 