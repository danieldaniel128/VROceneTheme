using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuilder : MonoBehaviour
{
    [Header("Builder Settings")]
    [SerializeField] private Camera _playerCamera; // Reference to the player's camera
    [SerializeField] private GameObject _selectedTower; // Default tower prefab
    [SerializeField] private LayerMask _tileLayerMask; // Layer mask to ensure the ray hits tiles only
    [SerializeField] private Material _hologramMaterial; // Material for the hologram effect

    private GameObject _hologramInstance; // Instance of the hologram
    private Tile _lastHoveredTile; // The last tile the ray hit

    

    bool isBuilding;

    public InputActionProperty _buildingTowerAction;

    private void Start()
    {
        _buildingTowerAction.action.started += OnBuildingTowerPerformed;
    }

    private void OnDestroy()
    {
        _buildingTowerAction.action.started -= OnBuildingTowerPerformed;
    }
    private void OnApplicationQuit()
    {
        _buildingTowerAction.action.started -= OnBuildingTowerPerformed;
    }

    private void OnBuildingTowerPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        isBuilding = !isBuilding;
        if (!isBuilding)
            HideHologram();
        else if (_selectedTower == null)//is building true, but didnt select a tower
        {
            //cant build if selected tower is null
            isBuilding = false;
        }
        Debug.Log("built a tower mode");
    }
    void Update()
    {
        HandleBuilding();
    }
    private void HandleBuilding()
    {
        if (isBuilding)
        {
            // Cast a ray from the camera to detect tiles
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _tileLayerMask))
            {
                Tile hitTile = hit.collider.transform.parent.GetComponent<Tile>();
                if (hitTile != null && !hitTile.IsOccupied)//not null and not occupied
                {
                    // If the hit tile changes, update the hologram's position
                    if (_lastHoveredTile != hitTile)
                    {
                        //Debug.Log("hit new tile");
                        UpdateHologram(hitTile);
                    }
                    HandleObjectRotation(_hologramInstance, 90);
                    // Place the tower on the tile when the player clicks
                    if (Input.GetMouseButtonDown(0)) // Left mouse button
                    {
                        PlaceTower(hitTile);
                    }
                }
            }
            else
            {
                // If the ray doesn't hit a tile, hide the hologram
                HideHologram();
            }
        }
    }
    private void HandleObjectRotation(GameObject rotatedObject, float angle)
    {
        // Rotate -90 degrees when pressing Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateObjectByDegrees(rotatedObject, -angle);
        }
        // Rotate +90 degrees when pressing E
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateObjectByDegrees(rotatedObject, angle);
        }
    }

    private void RotateObjectByDegrees(GameObject rotatedObject, float angle)
    {
        rotatedObject.transform.Rotate(0, 0, angle);
        Debug.Log($"<color=lightblue>{rotatedObject.name} rotated by {angle} degrees.</color>");
    }
    private void UpdateHologram(Tile tile)
    {
        _lastHoveredTile = tile;


        // Position the hologram at the center of the tile
        _hologramInstance.transform.position = tile.transform.position + Vector3.up * 0.5f; // Adjust height as needed
        _hologramInstance.SetActive(true);
    }

    private void HideHologram()
    {
        if (_hologramInstance != null)
        {
            _hologramInstance.SetActive(false);
        }
        _lastHoveredTile = null;
    }

    private void PlaceTower(Tile tile)
    {
        // Instantiate a real tower on the tile
        GameObject tower = Instantiate(_selectedTower, tile.transform.position + Vector3.up * 0.5f, _hologramInstance.transform.rotation,tile.transform);
        tile.IsOccupied = true;
        Debug.Log("<color=orange>built a tower action performed!</color>");
        Debug.Log($"<color=orange>Tower placed on tile at position: {tile.transform.position}</color>");
    }

    private void SetHologramMaterial(GameObject hologram)
    {
        Renderer[] renderers = hologram.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = _hologramMaterial;
        }
    }
    public void SetSelectTower(GameObject selectedTower)
    {
        _selectedTower = selectedTower;
        if (_hologramInstance != null)//tmp
            Destroy(_hologramInstance);
        _hologramInstance = Instantiate(_selectedTower);
        SetHologramMaterial(_hologramInstance);
    }
}
