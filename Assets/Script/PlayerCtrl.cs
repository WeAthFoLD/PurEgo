using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour {
    public enum State {
        Landing, Jumping
    }

    public float jumpTime;

    public float landTime;

    public float jumpScale = 0.6f;
    public float normalScale = 0.5f;

    public float forwardSpeed = 5.0f;
    public float jumpSpeedScale = 1.2f;

    public SpriteRenderer debugSprite;

    Transform mainCamera;

    Animator animator;

    Rigidbody2D rb;

    float jumpTargetX;
    float jumpBeginX;
    float cachedJumpTargetX;
    bool jumpPressedWhenAir = false;

    State state = State.Landing;

    public State pState {
        get {
            return state;
        }
    }

    float timeLeft;

    public float pStateTime {
        get {
            return pStateMaxTime - timeLeft;
        }
    }

    public float pStateMaxTime {
        get {
            return state == State.Landing ? landTime : jumpTime;
        }
    }

    public float pWaveTime {
        get {
            return waveTime;
        }
    }

    public Vector2 pWavePosition {
        get {
            return new Vector2(wavePosition.x, transform.position.y);
        }
    }

    float ySpeed = 0f;

    float waveTime = 0;
    Vector2 wavePosition;

    void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        timeLeft = landTime;

        // Time.timeScale = 0.7f;
    }
	
    void OnTouch(Vector3 mouseWorldPos) {
        mouseWorldPos.y = Screen.height - mouseWorldPos.y;

        var screenPos = Camera.main.ScreenToWorldPoint(mouseWorldPos);
        screenPos.z = 0;
        
        if (state == State.Landing) {
            jumpTargetX = screenPos.x;
        } else {
            cachedJumpTargetX = screenPos.x;
            jumpPressedWhenAir = true;
        }
    }

    void FixedUpdate () {
        // Update game logic
        var dt = Time.fixedDeltaTime;
        var npos = rb.position;
        var nscale = normalScale;

        float targetYSpeed = 0f;

        // Perform normal update
        switch (state) {
            case State.Landing: {
                targetYSpeed = forwardSpeed;
            } break;

            case State.Jumping: {
                targetYSpeed = forwardSpeed * jumpSpeedScale;

                var progress = 1 - (timeLeft / jumpTime);
                npos.x = Mathf.Lerp(jumpBeginX, jumpTargetX, Mathf.SmoothStep(0, 1, progress));

                nscale = normalScale + (jumpScale - normalScale) * Mathf.Sin(progress * Mathf.PI);
            } break;
        }

        ySpeed = Mathf.MoveTowards(ySpeed, targetYSpeed, dt * 10f);

        timeLeft -= dt;
        if (timeLeft < 0) { // Perform state switch
            switch (state) {
                case State.Jumping: {
                    npos.x = jumpTargetX;
                    state = State.Landing;
                    timeLeft = landTime;

                    debugSprite.color = Color.white;
                    waveTime = 0;
                    wavePosition = transform.position;

                    animator.Play("Idle");

                    if (jumpPressedWhenAir) {
                        jumpPressedWhenAir = false;
                        jumpTargetX = cachedJumpTargetX;
                    }

                } break;
                case State.Landing: {
                    
                    state = State.Jumping;
                    timeLeft = jumpTime;
                    jumpBeginX = npos.x;

                    debugSprite.color = Color.cyan;

                    animator.Play("Jump");

                    

                } break;
            }
        }
        
        npos.y += dt * ySpeed;
        rb.position = npos;
        transform.position = npos;
        waveTime += dt;
        
        debugSprite.transform.localScale = new Vector3(nscale, nscale, 1);
    }

    void OnGUI() {
        var evt = Event.current;
        if (evt.type == EventType.MouseDown) {
            OnTouch(evt.mousePosition);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (state == State.Landing) {
            // Die
            SceneManager.LoadScene("default");
        }
    }

}
