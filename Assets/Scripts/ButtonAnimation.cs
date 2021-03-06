﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour {

    Sprite customImage;
    void Start() {
        customImage = GetComponentInChildren<Image>().sprite;
    }

    public void ButtonPressed(Sprite image) {
        GetComponentInChildren<Image>().sprite = image;
    }

    public void ButtonReleased()
    {
        GetComponentInChildren<Image>().sprite = customImage;
    }
}
