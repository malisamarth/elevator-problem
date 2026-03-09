using UnityEngine;
using static FloorCallButton;

public class Testing : MonoBehaviour {

    private ElevatorSystemManager elevatorSystemManager;

    [SerializeField] private Floors[] testArray;

    private void Start() {
        elevatorSystemManager = ElevatorSystemManager.Instance;
        //TestCase();
    }

    private void TestCase() {

        foreach (Floors floor in testArray) {
            elevatorSystemManager.RequestedFloorHandler(floor);
        }

    }

}
