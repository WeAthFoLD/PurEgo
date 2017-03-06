using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum NodeType {
    Enemy, Empty
}


public class Beatmap {
    readonly GenGroup genGroup;
    
    public Beatmap(string s, GenGroup group) {
        genGroup = group;
        Parse(s);
    }

    public struct Obj {
        public GameObject prefab;
        public int time, track;
        public int width, height;
    }
    
    public const int Tracks = 5;

    // Runtime
    public List<Obj> items = new List<Obj>();
    [HideInInspector]
    public NodeType[][] grid; // grid[time][track]

    public int Length {
        get { return grid.Length; }
    }

    void Parse(string input) {
        List<string> lines = new List<string>();
        lines.AddRange(input.Split('\n'));
        lines.Reverse();

        items.Clear();

        for (int time = 0; time < lines.Count; ++time) {
            var line0 = lines[time];
            var line = line0.Where(ch => ch != ' ').ToArray();
            
            for (int track = 0; track < Beatmap.Tracks && track < line.Length; ++track) {
                var ch = line[track];
                var obj = genGroup.Find(ch);
                if (obj.HasValue) {
                    var it = obj.Value;
                    items.Add(new Obj {
                        prefab = it.prefab,
                        time = time,
                        track = track,
                        width = it.width,
                        height = it.height
                    });
                }
            }
        }

        grid = new NodeType[lines.Count][];
        for (int i = 0; i < grid.Length; ++i) {
            grid[i] = new NodeType[Tracks];
            for (int j = 0; j < Tracks; ++j) {
                grid[i][j] = NodeType.Empty;
            }
        }

        foreach (var obj in items) {
            for (int dx = 0; dx < obj.width; ++dx) {
                for (int dy = 0; dy < obj.height; ++dy) {
                    var time = dy + obj.time;
                    var track = dx + obj.track;
                    if (time < grid.Length && track < Tracks) {
                        grid[time][track] = NodeType.Enemy;
                    }
                }
            }
        }
    }

}
