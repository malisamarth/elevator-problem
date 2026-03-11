using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogBoxUI : MonoBehaviour {

    public static DialogBoxUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI queueText;
    [SerializeField] private TextMeshProUGUI elevatorSpeedText;

    private ElevatorSystemManager elevatorSystemManager;

    private List<int> convertedList = new List<int>();
    private int elevatorSpeedUI = 3;


    public event EventHandler OnResetButtonClicked;
    public event EventHandler<OnElevatorSpeedChangedEventArgs> OnElevatorSpeedChanged;
    public class OnElevatorSpeedChangedEventArgs : EventArgs {

        public int elevatorSpeed;

        public OnElevatorSpeedChangedEventArgs(int elevatorSpeed) {
            this.elevatorSpeed = elevatorSpeed;
        }

    }


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        elevatorSystemManager = ElevatorSystemManager.Instance;
        elevatorSystemManager.OnResetAll += ElevatorSystemManager_OnResetAll1;
    }


    private void Update() {

        //queueText.text = string.Join(", ", GetFloorsRequestsList().ToArray());

        SetDialogBoxRequestQueue(GetFloorsRequestsList().ToArray());
        SetElevatorSpeedUIText();

    }

    private void SetDialogBoxRequestQueue(int[] floorRequestArray) {

        if (floorRequestArray.Length == 0) {
            return;
        }

        if (convertedList.Count == 0 || floorRequestArray.Last() != convertedList.Last()) {
            convertedList = floorRequestArray.ToList();
            DisplayFloorRequests(convertedList);
        }
    }


    private List<int> GetFloorsRequestsList() {

        List<Floors> floorsRequestsQueue = elevatorSystemManager.GetFloorsRequestsQueue().ToList();
        List<int> floorsAsInt = floorsRequestsQueue.Select(floor => (int)floor).ToList();

        return floorsAsInt;


    }

    public void OnIncreaseElevatorSpeed() {

        if (elevatorSpeedUI < 8) {
            elevatorSpeedUI++;

            TriggerOnSpeedChange(elevatorSpeedUI);
        }

        
    }

    public void OnDecreaseElevatorSpeed() {
        if (elevatorSpeedUI > 1) {
            elevatorSpeedUI--;

            TriggerOnSpeedChange(elevatorSpeedUI);

        }
    }

    private void EmptyAll() {
        queueText.text = "[  ]";
    }

    public void OnResetButtonPressed() {
        OnResetButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnQuitGame() {
        GameUtilities.QuitGame();
    }

    private void DisplayFloorRequests(List<int> list) {
        queueText.text = "[ " + string.Join(", ", list) + " ]";
    }

    private void ElevatorSystemManager_OnResetAll1(object sender, EventArgs e) {
        convertedList.Clear();
        EmptyAll();
    }

    private void SetElevatorSpeedUIText() {
        elevatorSpeedText.text = elevatorSpeedUI.ToString();
    }

    public int GetElevatorSpeedUI() {
        return elevatorSpeedUI;
    }

    private void TriggerOnSpeedChange(int elevatorSpeedUI) {
        OnElevatorSpeedChanged?.Invoke(this, new OnElevatorSpeedChangedEventArgs(elevatorSpeedUI));
    }

}