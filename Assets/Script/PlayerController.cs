using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance {
        get {
            return GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
    }

    enum MoveDirection { Left = -1, Right = 1, Straight = 0 }

    public TrackView trackView = new SimpleTrackView();

    [HideInInspector]
    public float timeSinceLastBeat;

    [HideInInspector]
    int track = Beatmap.Tracks / 2;
    
    [HideInInspector]
    public int time = 0;
    public int waveInterval = 4;
    
    public AudioSource bgm;
    BeatmapGenerator beatmapGen;

    bool collisionJudged = false;
    bool waved = false;

    MoveDirection curMoveDirection = MoveDirection.Straight;
    MoveDirection nextMoveDirection = MoveDirection.Straight;

    Animator animator;

    [HideInInspector] 
    public float timeSinceWave = 0.3f;

    public int bpm = 110;
    public float beatLength {
        get {
            return 60.0f / bpm;
        }
    }

    public Queue<NodeType[]> gridQueue = new Queue<NodeType[]>();

    void Start () {
        transform.position = new Vector3(trackView.getX(track), 0, 0);
        animator = GetComponent<Animator>();
        beatmapGen = GameObject.Find("BeatmapView").GetComponent<BeatmapGenerator>();

        var jumpOriginalTime = 38f / 12f;
        var timeScale = jumpOriginalTime / beatLength;
        animator.speed = timeScale;

        animator.Play("Jump");
        beatmapGen.NextSong();
    }
	
    void Update () {
        var dt = Time.deltaTime;

        timeSinceLastBeat += dt;
        timeSinceWave += dt;

        transform.position = new Vector3(
            Mathf.Lerp(trackView.getX(track), 
                       trackView.getX(track + (int) curMoveDirection), 
                       Mathf.SmoothStep(0, 1, 1.2f * timeSinceLastBeat /beatLength)),
            0, 0
        );

        if (timeSinceLastBeat >= beatLength * 0.5f && !collisionJudged) {
            var head = gridQueue.Peek();
            if (head[track + (int) curMoveDirection] == NodeType.Enemy) { // Die
                animator.speed = 1.0f;
                timeSinceWave = 0.3f;
                animator.Play("Die");
            }
        
            collisionJudged = true;
        }

        if ((time + 1) % waveInterval == 0 && timeSinceLastBeat >= beatLength * 0.95f && !waved) {
            WaveOnce();
            waved = true;
        }

        if (timeSinceLastBeat >= beatLength) {
            timeSinceLastBeat -= beatLength;

            track = track + (int) curMoveDirection;
            ++time;

            curMoveDirection = nextMoveDirection;
            nextMoveDirection = MoveDirection.Straight;
            collisionJudged = false;
            waved = false;

            gridQueue.Dequeue();

            animator.Play("Jump", 0, 0f);
        }
    }

    void WaveOnce() {
        timeSinceWave = 0f;
        if (!bgm.isPlaying) {
            beatmapGen.NextSong();
        }
    }

    void OnGUI() {
        var evt = Event.current;
        if (evt.type == EventType.MouseDown) {
            OnTouch(evt.mousePosition);
        }
    }

    void OnTouch(Vector3 mouseWorldPos) {
        mouseWorldPos.y = Screen.height - mouseWorldPos.y;

        var screenPos = Camera.main.ScreenToWorldPoint(mouseWorldPos);
        screenPos.z = 0;
        
        // Check whether we accept input now
        if (timeSinceLastBeat >= beatLength * 0f) {
            var targetTrack = trackView.getTrack(screenPos.x);
            var nextTrack = track + (int) curMoveDirection;

            MoveDirection dir;
            if (targetTrack > nextTrack) {
                dir = MoveDirection.Right;
            } else if (targetTrack < nextTrack) {
                dir = MoveDirection.Left;
            } else {
                dir = MoveDirection.Straight;
            }

            nextMoveDirection = dir;
        }
    }

    public void PlayWithdraw() {
        // GetComponent<AudioSource>().Play();
    }

    public void PlayDissolve() {
        transform.FindChild("Die1").GetComponent<AudioSource>().Play();
    }
    
}
