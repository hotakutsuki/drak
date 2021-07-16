using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    
    static ArrayList Deck = new ArrayList();
    [SerializeField]
    ArrayList hand = new ArrayList();
    [SerializeField]
    Transform Canvas;

    public GameObject PrefabTile;
    public Transform GameTiles;
    public Transform DeckTransform;
    [SerializeField]
    public GameObject selectedGameObject;
    private Vector3 prePos;

    private void Start()
    {
        CreateDeck();
        RefillCardsInHand();
    }
    public void RefillCardsInHand()
    {
        while (hand.Count < 5)
        {
            int pos = Random.Range(0, Deck.Count);
            GameObject card = (GameObject)Deck[pos];
            card.transform.SetParent(Canvas, false);
            card.transform.localScale = new Vector3(50, 50, 1);
            card.transform.localPosition = new Vector3(Random.Range(-400, -250f), Random.Range(-200f, 200f), -1f);
            card.transform.rotation = Camera.main.transform.rotation;
            hand.Add(Deck[pos]);
            Deck.Remove(card);
        }
    }

    private void CreateDeck()
    {
        for (int i = 0; i < 50; i++)
            Deck.Add(Instantiate(PrefabTile, DeckTransform.position, DeckTransform.rotation, DeckTransform));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseDown();
        
        if (Input.GetMouseButton(0))
            OnMouseDrag();

        if (Input.GetMouseButtonUp(0)){
            OnMouseUp();
        }
    }

    private void OnMouseUp()
    {
        if (selectedGameObject != null){
            TileProperties tileProperties = selectedGameObject.GetComponent<TileProperties>();
            if (tileProperties.state == GameVariables.TileStates.Positioning)
                if (tileProperties.ValidatePosition())
                {
                    tileProperties.state = GameVariables.TileStates.InGame;
                    hand.Remove(selectedGameObject);
                    RefillCardsInHand();
                } else {
                    selectedGameObject.transform.SetParent(Canvas, false);
                    selectedGameObject.transform.SetPositionAndRotation(prePos, Camera.main.transform.rotation);
                    selectedGameObject.transform.localScale = new Vector3(50, 50, 1);
                    tileProperties.state = GameVariables.TileStates.InCanvas;
                }

            //selectedGameObject = null;
        }
    }

    private void OnMouseDrag()
    {
        if (selectedGameObject != null)
            SetPositionAndParent(selectedGameObject);
    }

    private void SetPositionAndParent(GameObject gameObject)
    {
    
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("UI"));
        if (hit.distance>0){
            gameObject.transform.SetParent(Canvas, false);
            gameObject.transform.SetPositionAndRotation(hit.point, Camera.main.transform.rotation);
            gameObject.transform.localScale = new Vector3(50,50,1);
            selectedGameObject.GetComponent<TileProperties>().state = GameVariables.TileStates.InCanvas;
        } else {
            Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Terrain"));
            gameObject.transform.SetParent(GameTiles, false);
            gameObject.transform.SetPositionAndRotation(new Vector3(Mathf.Round(hit.point.x), 0, Mathf.Round(hit.point.z)), Quaternion.Euler(90, 0, 0));
            gameObject.transform.localScale = Vector3.one;
            selectedGameObject.GetComponent<TileProperties>().state = GameVariables.TileStates.Positioning;
        }
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.transform.gameObject.tag == "Tile"){
            selectedGameObject = hit.transform.gameObject;
            prePos = hit.transform.position;
        }
            
        else
            selectedGameObject = null;

        Debug.Log("This hit at " + hit.transform.gameObject.name);
    }
}
