using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{

    [SerializeField] RectTransform healthLine;
    [SerializeField] Vector3 offset;

    RectTransform rect;

    public Transform target;
    public float ratio;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (target == null) return;

        healthLine.localScale = new Vector3(ratio, 1, 1);
        rect.position = Camera.main.WorldToScreenPoint(target.position) + offset;
    }

    public void SetTarget(Transform target, float ratio)
    {
        this.target = target;
        this.ratio = ratio;
    }
}

