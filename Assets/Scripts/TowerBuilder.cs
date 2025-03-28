using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerBuilder : MonoBehaviour
{
    [Header("Builder References")]
    [SerializeField] private Camera _playerCamera; // Reference to the player's camera
    [SerializeField] private TowerController _selectedTower; // Default tower prefab
    [SerializeField] private Material _hologramMaterial; // Material for the hologram effect
    [SerializeField] private Transform _buildingRayStartPos;
    [SerializeField] private TowerController _hologramInstance; // Instance of the hologram
    [SerializeField] private LayerMask _tileLayerMask; // Layer mask to ensure the ray hits tiles only
    [SerializeField] private TMPro.TMP_Text _currentUIAmountText;
    [Header("Builder Settings")]
    [SerializeField] private int _requiredWoodAmount;
    private Tile _lastHoveredTile; // The last tile the ray hit
    private Tile _currentHoveredTile;
    int currentWoodAmount;
    public int CurrentWoodAmount {  get => currentWoodAmount; private set { currentWoodAmount = value; _currentUIAmountText.text = $"{currentWoodAmount}x"; } }
    bool isBuilding;

    public InputActionProperty _activateBuildingTowerModeAction;
    public InputActionProperty _activateTowerBuiltAction;
    public void AddWood(int woodAmount)
    {
        CurrentWoodAmount += woodAmount;
    }
    private void Start()
    {
        _activateBuildingTowerModeAction.action.started += OnBuildingTowerPerformed;
        _activateTowerBuiltAction.action.started += OnBuiltTowerPerformed;
        if(_selectedTower != null)//in case there is a default.
            SetSelectTower(_selectedTower);
        HideHologram();
    }

    private void OnDestroy()
    {
        _activateBuildingTowerModeAction.action.started -= OnBuildingTowerPerformed;
        _activateTowerBuiltAction.action.started -= OnBuiltTowerPerformed;
    }
    private void OnApplicationQuit()
    {
        _activateBuildingTowerModeAction.action.started -= OnBuildingTowerPerformed;
        _activateTowerBuiltAction.action.started -= OnBuiltTowerPerformed;
    }

    private void OnBuildingTowerPerformed(InputAction.CallbackContext context)
    {
        isBuilding = !isBuilding;
        if (CurrentWoodAmount < _requiredWoodAmount)
        {
            isBuilding = false;
        }
        if (!isBuilding)
            HideHologram();
        else if (_selectedTower == null)//is building true, but didnt select a tower
        {
            //cant build if selected tower is null
            isBuilding = false;
        }
        //Debug.Log("built a tower mode");
    }
    private void OnBuiltTowerPerformed(InputAction.CallbackContext context)
    {
        // Place the tower on the tile when the player clicks
        if(isBuilding && _currentHoveredTile != null)
            PlaceTower(_currentHoveredTile);
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
            Ray ray = new Ray(_buildingRayStartPos.position, _buildingRayStartPos.forward);
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
                    _currentHoveredTile = hitTile;
                }
            }
            else
            {
                // If the ray doesn't hit a tile, hide the hologram
                HideHologram();
                _currentHoveredTile = null;
            }
        }
    }
    private void HandleObjectRotation(TowerController rotatedObject, float angle)
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

    private void RotateObjectByDegrees(TowerController rotatedObject, float angle)
    {
        rotatedObject.transform.Rotate(0, 0, angle);
        //Debug.Log($"<color=lightblue>{rotatedObject.name} rotated by {angle} degrees.</color>");
    }
    private void UpdateHologram(Tile tile)
    {
        _lastHoveredTile = tile;
        // Position the hologram at the center of the tile
        _hologramInstance.transform.position = tile.transform.position + Vector3.up * 0.5f; // Adjust height as needed
        _hologramInstance.gameObject.SetActive(true);
    }

    private void HideHologram()
    {
        if (_hologramInstance != null)
        {
            _hologramInstance.gameObject. SetActive(false);
        }
        _lastHoveredTile = null;
    }

    private void PlaceTower(Tile tile)
    {
        // Instantiate a real tower on the tile
        TowerController tower = Instantiate(_selectedTower, tile.transform.position + Vector3.up * 0.5f, _hologramInstance.transform.rotation,tile.transform);
        tower.IsPlaced = true;
        tile.IsOccupied = true;
        CurrentWoodAmount -= _requiredWoodAmount;
        if (CurrentWoodAmount < _requiredWoodAmount)
        {
            isBuilding = false;
        }
        if (!isBuilding)
            HideHologram();
        //Debug.Log("<color=orange>built a tower action performed!</color>");
        //Debug.Log($"<color=orange>Tower placed on tile at position: {tile.transform.position}</color>");
    }

    private void SetHologramMaterial(TowerController hologram)
    {
        Renderer[] renderers = hologram.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = _hologramMaterial;
        }
    }
    public void SetSelectTower(TowerController selectedTower)
    {
        _selectedTower = selectedTower;
        if (_hologramInstance != null)//tmp
            Destroy(_hologramInstance);
        _hologramInstance = Instantiate(_selectedTower);
        SetHologramMaterial(_hologramInstance);
    }
}
