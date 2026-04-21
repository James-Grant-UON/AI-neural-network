using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Trainer trainer; // neural network trainer
    public AIDriver ai; // AI driving script
    public PlayerInput player; // player controller
    public DataRecorder recorder; // data storage

    public string datasetFile = "training_data.csv";
    public int epochsToTrain = 50; // number of training iterations

    void Start()
    {
        // give AI access to the trained network but disable it initially
        ai.net = trainer.net;
        ai.canDrive = false;

        // player starts in control
        player.isActive = true;
    }

    void Update()
    {
        // reset dataset (clear file)
        if (Input.GetKeyDown(KeyCode.C))
            ResetDataset();

        // train AI and switch control to it
        if (Input.GetKeyDown(KeyCode.T))
            TrainAndStartAI();

        // switch back to player control
        if (Input.GetKeyDown(KeyCode.P))
            UsePlayer();
    }

    // train neural network using recorded data, then enable AI driving
    public void TrainAndStartAI()
    {
        DatasetLoader loader = new DatasetLoader();
        string path = Application.persistentDataPath + "/" + datasetFile;

        loader.Load(path);
        trainer.LoadDataset(loader);

        Debug.Log("training started...");
        trainer.TrainMultipleEpochs(epochsToTrain);

        // disable player and enable AI
        player.isActive = false;
        ai.canDrive = true;

        Debug.Log("AI enableed after training");
    }

    // switch back to manual player driving
    public void UsePlayer()
    {
        ai.canDrive = false;
        player.isActive = true;

        Debug.Log("player control");
    }

    // deletes and recreates dataset file
    public void ResetDataset()
    {
        recorder.ResetData();
        Debug.Log("dataset file reset.");
    }
}