
using System;
using System.Net.NetworkInformation;
using UnityEngine;

public class FloorCallButton : MonoBehaviour {

    [SerializeField] private Floors floorType;


    public static event EventHandler<OnRequestCallElevatorEventArgs> OnRequestCallElevator;
    public class OnRequestCallElevatorEventArgs : EventArgs {

        public Floors requestFromFloor;


        public OnRequestCallElevatorEventArgs(Floors floorType) {
            this.requestFromFloor = floorType;
        }
    }

    public void RequestCallElevator() {

        OnRequestCallElevator?.Invoke(this, new OnRequestCallElevatorEventArgs(GetFloorType()));
    }

    private Floors GetFloorType() {
        return floorType;
    }

}