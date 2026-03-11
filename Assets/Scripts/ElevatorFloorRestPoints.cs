using System.Collections.Generic;
using UnityEngine;

public class ElevatorFloorRestPoints : MonoBehaviour {

    [SerializeField] private List<FloorData> floorRestPoints;

    private void Awake() {
        foreach (FloorData floorPoint in floorRestPoints) {
            floorPoint.SetPosition();
        }
    }

    public Vector3 GetPositionByFloor(Floors requiredFloor) {

        foreach (FloorData floorData in floorRestPoints) {
            
            if (floorData.floor == requiredFloor) {
                return floorData.position;
            }

        }

        return Vector3.zero;

    }

    public Floors GetFloorByPosition(Vector3 currentPosition) {

        foreach(FloorData floorData in floorRestPoints) {

            if (floorData.position == currentPosition) {
                return floorData.floor;
            }

        }

        Debug.LogError(currentPosition + " :- This position doesn't match any floor positions.");
        return Floors.None;

    }

}