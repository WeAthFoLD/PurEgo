using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TrackView {

    float getX(int trackID);
    int getTrack(float x);

}

public class SimpleTrackView : TrackView {

    public float getX(int trackID) {
        return (trackID - 2) * 1.2f;
    }

    public int getTrack(float x) {
        return Mathf.Clamp(Mathf.CeilToInt((x + 2.4f) / 1.2f), 0, 4);
    }

}