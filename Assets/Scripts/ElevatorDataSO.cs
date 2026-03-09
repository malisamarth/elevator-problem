using UnityEngine;


[CreateAssetMenu(fileName = "ElevatorData", menuName = "ScriptableObjects/ElevatorData")]
public class ElevatorDataSO : ScriptableObject {

    public ElevatorID elevatorID;
    public ElevatorName elevatorName;
    public Floors defaultFloorPosition;
    public Floors elevatorCurrentFloorPosition;


}