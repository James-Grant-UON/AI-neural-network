// Controls switching between player and AI, training, and dataset management
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Trainer trainer;          // Neural network trainer
    public AIDriver ai;              // AI driving script
    public PlayerInput player;       // Player controller
    public DataRecorder recorder;   // Data storage

    public string datasetFile = "training_data.csv";
    public int epochsToTrain = 50;   // Number of training iterations

    void Start()
    {
        // Give AI access to the trained network but disable it initially
        ai.net = trainer.net;
        ai.canDrive = false;

        // Player starts in control
        player.isActive = true;
    }

    void Update()
    {
        // Reset dataset (clear file)
        if (Input.GetKeyDown(KeyCode.C))
            ResetDataset();

        // Train AI and switch control to it
        if (Input.GetKeyDown(KeyCode.T))
            TrainAndStartAI();

        // Switch back to player control
        if (Input.GetKeyDown(KeyCode.P))
            UsePlayer();
    }

    // Train neural network using recorded data, then enable AI driving
    public void TrainAndStartAI()
    {
        DatasetLoader loader = new DatasetLoader();
        string path = Application.persistentDataPath + "/" + datasetFile;

        loader.Load(path);
        trainer.LoadDataset(loader);

        Debug.Log("Training started...");
        trainer.TrainMultipleEpochs(epochsToTrain);

        // Disable player and enable AI
        player.isActive = false;
        ai.canDrive = true;

        Debug.Log("AI ENABLED after training");
    }

    // Switch back to manual player driving
    public void UsePlayer()
    {
        ai.canDrive = false;
        player.isActive = true;

        Debug.Log("Player control");
    }

    // Deletes and recreates dataset file
    public void ResetDataset()
    {
        recorder.ResetData();
        Debug.Log("Dataset file reset.");
    }
}