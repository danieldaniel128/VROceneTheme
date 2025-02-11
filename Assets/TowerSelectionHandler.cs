using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TowerSelectionHandler : MonoBehaviour
{
    [SerializeField] GameObject _selectionPanel;
    //tmp for testing
    [SerializeField] GameObject[] _towerprefabs;
    public Action<GameObject> OnTowerSelectionSucceed;

    //private void OnActivatedSelectionPanelPerformed(InputAction.CallbackContext context)
    //{
    //    if (!context.performed)
    //        return;
    //    _selectionPanel.SetActive(!_selectionPanel.activeSelf);
//        if(_selectionPanel.activeSelf)
//            Cursor.lockState = CursorLockMode.None;
//        else
//            Cursor.lockState = CursorLockMode.Locked;
    //}
    //    private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        _selectionPanel.SetActive(!_selectionPanel.activeSelf);
    //        if(_selectionPanel.activeSelf)
    //            Cursor.lockState = CursorLockMode.None;
    //        else
    //            Cursor.lockState = CursorLockMode.Locked;
    //    }
    //}
    public void SelectTower(int i)
    {
        //try select

        //trying succeed

        //selecting
        OnTowerSelectionSucceed?.Invoke(_towerprefabs[i]);
    }
    private void OnDestroy()
    {
        OnTowerSelectionSucceed = null;
    }
    private void OnApplicationQuit()
    {
        OnTowerSelectionSucceed = null;
    }
}
