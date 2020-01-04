using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig 
{
    public List<List<string>> Boards { get; private set; }
    public LevelConfig(string levelConfigText)
    {
        Boards = new List<List<string>>();
        var lines = levelConfigText.Split('\n');
        foreach(var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                List<string> col = new List<string>();
                var grids = line.Split(',');
                foreach (var grid in grids)
                {
                    grid.Replace("\r", "");
                    col.Add(grid);
                }
                Boards.Add(col);
            }
        }
    }
}
