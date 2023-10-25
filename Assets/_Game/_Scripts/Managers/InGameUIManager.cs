using HyperConnect;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    #region Varibales
    [SerializeField] private TextMeshProUGUI _levelTxt;
    #endregion Varibales

    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.OnNewLevelStarted += Callback_On_New_Level_Started;
    }
    private void OnDisable()
    {
        GlobalEventHandler.OnNewLevelStarted -= Callback_On_New_Level_Started;
    }
    #endregion Unity Methods

    #region Public Methods
    #endregion Public Methods

    #region Private Methods
    #endregion Private Methods

    #region Callbacks
    private void Callback_On_New_Level_Started(int level)
    {
        _levelTxt.SetText($"Level {level}");
    }
    #endregion Callbacks
}
