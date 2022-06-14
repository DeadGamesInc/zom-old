using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseMap : MonoBehaviour {
    [SerializeField] public int Length;
    [SerializeField] public int Width;
    [SerializeField] public float CellSize;

    public MapPath[] paths = Array.Empty<MapPath>();
    public MapGrid grid { get; private set; }

    public Vector3 GetNodeWorldPosition(MapNode node) {
        return grid.GetWorldPosition(node.x, node.z);
    }

    // Returns MapPath containing the specified nodes or null if one does not exist
    public MapPath? GetPathByNodes(params MapNode[] nodes) {
        foreach (MapPath mapPath in paths) {
            if (nodes.All(node => mapPath.path.Contains(node))) {
                return mapPath;
            }
        }

        return null;
    }

    private void DrawPaths() {
        foreach (MapPath mapPath in paths) {
            for (int i = 0; i < mapPath.path.Length; i++) {
                if (i == mapPath.path.Length - 1) break;
                MapNode node = mapPath.path[i];
                MapNode nextNode = mapPath.path[i + 1];

                Debug.DrawLine(
                    GetNodeWorldPosition(node),
                    GetNodeWorldPosition(nextNode),
                    Color.red, 100f);
            }
        }
    }

    protected MapPath toPath(params (int, int)[] coords) {
        MapNode[] nodes = coords.Select(coord => new MapNode(coord.Item1, coord.Item2)).ToArray();
        return new MapPath(nodes);
    }

    // Start is called before the first frame update
    public void Start() {
        Vector3 globalPosition = gameObject.transform.position;
        grid = new MapGrid(Width, Length, CellSize, globalPosition);
        DrawPaths();
    }
}