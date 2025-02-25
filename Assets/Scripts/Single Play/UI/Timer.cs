using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;
    [SerializeField] private float timeLimit = 120f;

    private float _maxTime;
    private float _currentTime = 0f;

    private void Awake()
    {
        InitComponents();
        InitTimeValue();
    }

    // Update is called once per frame
    void Update()
    {
        FlowTime();
    }

    #region Init Methods

    private void InitComponents()
    {       
        if(timerSlider != null)
        {
            timerSlider.maxValue = 1f;
            timerSlider.value = 1f;
        }
    }
    private void InitTimeValue()
    {
        _maxTime = timeLimit;
        _currentTime = _maxTime;
    }

    #endregion

    #region Timer Methods

    private void FlowTime()
    {
        if (_currentTime > 0f)
        {
            timerSlider.value = _currentTime / _maxTime;
            _currentTime = Mathf.Max(0f, _currentTime - Time.deltaTime);
        }
    }

    #endregion
}
