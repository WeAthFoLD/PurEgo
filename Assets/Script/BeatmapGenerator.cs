using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapGenerator : MonoBehaviour {
    public Song[] songs;
    public TextAsset init;
    public GenGroup genGroup;
    public AudioSource audioSource;

    public bool firstGenerate = true;

    PlayerController player;
    int generatedTime = 0;

    int songIndex = -1;

    Song song {
        get {
            return songs[songIndex];
        }
    }

    void Start () {
        player = PlayerController.instance;
    }

    public void NextSong() {
        songIndex = Mathf.Min(songIndex + 1, songs.Length - 1);

        audioSource.clip = song.clip;
        audioSource.PlayScheduled(song.beginPos);

        player.bpm = song.bpm;
        player.waveInterval = song.waveInterval;
        // firstGenerate = true;
    }
	
    void Update () {
        if (generatedTime < player.time + 8) {
            var content = firstGenerate || songIndex == -1 ? init : Util.RandomItem(song.beatmaps);
            firstGenerate = false;

            var t = transform;

            var beatmap = new Beatmap(content.text, genGroup);
            foreach (var obj in beatmap.items) {
                var position = new Vector3(1.2f * (obj.track - 2), 1.2f * (generatedTime + obj.time + 1), 0);
                var instance = Instantiate(obj.prefab, t) as GameObject;
                instance.transform.localPosition = position;
            }
            foreach (var line in beatmap.grid) {
                player.gridQueue.Enqueue(line);
            }

            generatedTime += beatmap.Length;
        }
    }
}
