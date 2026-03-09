using System;
using UnityEngine;

public class SetElevatorToFloor : MonoBehaviour {

    public static SetElevatorToFloor Instance { get; private set; }

    public event EventHandler<OnSetElevatorToFloorEventArgs> OnSetElevatorToFloor;
    public class OnSetElevatorToFloorEventArgs : EventArgs {
        public GameObject moveElevatorObject;

        public OnSetElevatorToFloorEventArgs(GameObject moveElevatorObject) {
            this.moveElevatorObject = moveElevatorObject;
        }

    }

    [SerializeField] private GameObject elevatorObject;

    private void Awake() {
        
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        SetButtonToFloor();
    }

    private void SetButtonToFloor() {
        OnSetElevatorToFloor?.Invoke(this, new OnSetElevatorToFloorEventArgs(elevatorObject));
    }
     public void SetElevatorToFloorButton() {
        SetButtonToFloor();
     }


}