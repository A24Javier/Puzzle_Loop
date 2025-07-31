using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    private List<FrameRecorder> playerRecorder, lastLoopRecorder;
    [SerializeField] private GameObject playerPrefab, phantomPlayerPrefab;
    private Transform playerTransf;
    private Rigidbody2D phantomRb;
    public bool isRecording = false;
    private int actualLevel = 0, actualLoop = 0, frame = 0;
    [SerializeField] private Vector2[] spawnLevels = new Vector2[5];

    // UI
    private float actualTime;
    [SerializeField] private float[] timeLevels = new float[5];
    [SerializeField] private TMP_Text textTime;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    void Start()
    {
        playerRecorder = new List<FrameRecorder>();
        lastLoopRecorder = new List<FrameRecorder>();
        playerTransf = GameObject.Find("Player").GetComponent<Transform>();
        actualLevel = 0;
        actualTime = timeLevels[actualLevel];
    }

    void Update()
    {
        if (isRecording)
        {
            if(actualTime > 0)
            {
                actualTime -= Time.deltaTime;
            }
            else if(actualTime <= 0)
            {
                RestartLoop();
            }
            actualTime = Mathf.Clamp(actualTime, 0, timeLevels[actualLevel]); // Para que nunca tenga un valor inferior a 0 y lo muestre en textTime
            textTime.text = actualTime.ToString("Time left: 00");
        }
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            FrameRecorder actualFrame = new FrameRecorder();
            actualFrame.position = playerTransf.position;
            actualFrame.velocity = playerTransf.GetComponent<Rigidbody2D>().linearVelocity;
            playerRecorder.Add(actualFrame);
        }

        if(actualLoop >= 1 && isRecording)
        {
            if(frame < lastLoopRecorder.Capacity)
            {
                FrameRecorder phantomFrame = new FrameRecorder(lastLoopRecorder[frame].position, lastLoopRecorder[frame].velocity);
                phantomRb.position = phantomFrame.position;
                phantomRb.linearVelocity = phantomFrame.velocity;
                frame++;
                Debug.Log(frame);
            }
        }
    }

    private void RestartLoop()
    {
        isRecording = false;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("Phantom"));

        GameObject playerIns = Instantiate(playerPrefab);
        playerIns.transform.position = spawnLevels[actualLevel];
        playerTransf = playerIns.transform;

        GameObject phantomIns  = Instantiate(phantomPlayerPrefab);
        phantomIns.transform.position = spawnLevels[actualLevel];
        phantomRb = phantomIns.GetComponent<Rigidbody2D>();

        lastLoopRecorder.Clear();
        lastLoopRecorder.AddRange(playerRecorder); // Copiamos el contenido de playerRecorder a lastLoopRecorder
        playerRecorder.Clear();

        frame = 0;
        actualTime = timeLevels[actualLevel];

        actualLoop++;
        Debug.Log("Restart Loop finalizado");
    }
}

public class FrameRecorder
{
    public Vector2 position;
    public Vector2 velocity;

    public FrameRecorder() { } // Constructor vacio

    public FrameRecorder(Vector2 newPos, Vector2 newVel) // Constructor para asignar
    {
        position = newPos;
        velocity = newVel;
    }
}
