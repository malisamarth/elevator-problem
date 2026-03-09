using UnityEngine;

[System.Serializable]
public class FloorData {

    public Floors floor;
    public Vector3 position;
    public Transform floorPosition;


    public void SetPosition() {
        position = floorPosition.position;
    }

}