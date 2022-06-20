using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Character : MonoBehaviour {
    private BaseMap map;
    [SerializeField] public float MovementSpeed = 1f;
    [SerializeField] public CharacterState State;
    public MapNode MapPosition { get; private set; }
    public CharacterRoute Route { get; private set; }

    public static GameObject Create(MapNode node) {
        // Create Character
        GameObject newCharacterObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newCharacterObject.name = "Character"; // can remove once move logic is moved from MapNode to LevelController
        Character character = newCharacterObject.AddComponent<Character>();
        character.MapPosition = node;
        newCharacterObject.transform.localScale = new Vector3(10, 10, 10);
        // Create Character Camera
        CharacterCamera.Create(newCharacterObject);

        return newCharacterObject;
    }
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(setMap());
    }

    // Update is called once per frame
    void Update() {
        switch (State) {
            case CharacterState.InTransit:
                Move();
                break;
        }
    }

    public void MoveTowards(MapNode target) {
        Route = new CharacterRoute(map, MapPosition, target);
        State = CharacterState.InTransit;
    }

    private void Move() {
        transform.position = Vector3.MoveTowards(transform.position, Route.CurrentTargetWorldPos, MovementSpeed);
        if (transform.position == Route.CurrentTargetWorldPos) {
            MapPosition = Route.CurrentTarget;
            if (!Route.NextPosition()) {
                State = CharacterState.Idle;
                Route = null;
            }
        }
    }

    private IEnumerator setMap() {
        GameObject mapObject;
        while ((mapObject = BaseMap.Get()) == null) yield return null;
        map = mapObject.GetComponent<BaseMap>();
        setMapPosition(MapPosition);
    }

    private Vector3 setMapPosition(MapNode node) {
        MapPosition = node;
        return transform.position = map.GetNodeWorldPosition(node);
    }
}