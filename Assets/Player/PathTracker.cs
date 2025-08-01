using System.Collections.Generic;
using UnityEngine;

public static class PathTracker
{
    private static List<Vector2> _playerPath;

    static PathTracker()
    {
        _playerPath = new List<Vector2>();
    }

    public static void Add(Vector2 pos)
    {
        _playerPath.Add(pos);
    }
    
    public static List<Vector2> GetPlayerPath() => _playerPath;

    public static void Reset()
    {
        _playerPath.Clear();
    }
}