using System;
using JetBrains.Annotations;
using UnityEngine;

public class MapNode : MonoBehaviour {
    public const int SIZE = 15;
    public const int MAP_GRID_SIZE = 4;
    public int x, z;
    [SerializeField] public MapGrid PlayerGrid;
    [SerializeField] public GameObject location;

    public static MapNode Create(int x, int z, MapGrid grid = null, bool draw = false) {
        GameObject newGameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        MapNode node = newGameObject.AddComponent<MapNode>();
        node.x = x;
        node.z = z;
        if (draw) {
            newGameObject.transform.position = grid.GetWorldPosition(x, z);
            newGameObject.transform.localScale = new Vector3(SIZE, 0, SIZE);
        } else {
            newGameObject.transform.localScale = Vector3.zero;
        }

        node.PlayerGrid = new MapGrid(MAP_GRID_SIZE, MAP_GRID_SIZE, 10, newGameObject.transform.position);
        return node;
    }

    public void Start() {
    }

    public void OnMouseEnter() {
        var renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_Color", Color.green);
    }

    public void OnMouseExit() {
        var renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_Color", Color.white);
    }

    public void OnMouseDown() {
        LevelController.Get().QueueCommand(PlayerCommand.MoveCharacter, gameObject);
    }

    public override bool Equals(object? obj) => obj is MapNode other && equals(other);
    private bool equals(MapNode n) => x == n.x && z == n.z;
    public override int GetHashCode() => (x, z).GetHashCode();
}