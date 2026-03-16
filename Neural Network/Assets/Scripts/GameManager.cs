using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Phase { DataCollection, Training, Autonomous }
    public Phase currentPhase = Phase.DataCollection;

    [Header("References (Auto-Assign if empty)")]
    public CarController playerCar;
    public DataRecorder dataRecorder;
    public Trainer trainer;
    public AIDriver aiDriver;

    void Start()
    {
        AutoAssign();
        ResetDataset();

        // Phase 0: Player drives
        currentPhase = Phase.DataCollection;
        playerCar.enabled = true;
        dataRecorder.enabled = true;

        trainer.enabled = false;
        aiDriver.enabled = false;

        Debug.Log("Phase 0: Data Collection started");
    }

    void Update()
    {
        switch (currentPhase)
        {
            case Phase.DataCollection:
                // Press T to start training
                if (Input.GetKeyDown(KeyCode.T))
                    StartTraining();
                break;

            case Phase.Training:
                if (trainer.IsTrainingDone)
                    StartAutonomous();
                break;

            case Phase.Autonomous:
                // AI drives automatically
                break;
        }
    }

    void StartTraining()
    {
        currentPhase = Phase.Training;
        Debug.Log("Phase 1: Training started");

        playerCar.enabled = false;
        dataRecorder.enabled = false;

        trainer.enabled = true;
        trainer.StartTraining();
    }

    void StartAutonomous()
    {
        currentPhase = Phase.Autonomous;
        Debug.Log("Phase 2: Autonomous AI started");

        aiDriver.net = trainer.GetTrainedNetwork();
        aiDriver.enabled = true;
    }

    void ResetDataset()
    {
        if (dataRecorder != null)
            dataRecorder.ResetData();
    }

    void AutoAssign()
    {
        if (playerCar == null) playerCar = FindObjectOfType<CarController>();
        if (dataRecorder == null && playerCar != null) dataRecorder = playerCar.GetComponent<DataRecorder>();
        if (trainer == null) trainer = FindObjectOfType<Trainer>();
        if (aiDriver == null) aiDriver = FindObjectOfType<AIDriver>();
    }
}