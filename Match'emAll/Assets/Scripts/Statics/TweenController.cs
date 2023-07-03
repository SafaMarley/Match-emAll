using System;
using UnityEngine;

namespace Statics
{
    public static class TweenController
    {
        private const float OpenPanelSpeed = .1f;
        private const float ClosePanelSpeed = .1f;
        private const float GUIMoveTimer = .5f;
        
        public static void DisplayInGameGUI(GameObject transformToMove, float positionTo, bool onXAxis, Action chainAction, LeanTweenType easeType = LeanTweenType.linear, float delay = 0f)
        {
            if (onXAxis)
            {
                LeanTween.moveLocalX(transformToMove, positionTo, GUIMoveTimer).setEase(easeType).setDelay(delay).setOnComplete(chainAction);
            }
            else
            {
                LeanTween.moveLocalY(transformToMove, positionTo, GUIMoveTimer).setEase(easeType).setDelay(delay).setOnComplete(chainAction);
            }
        }
        
        public static void DisplayPanel(GameObject transformToDisplay, Action chainAction, float delay = 0.0f)
        {
            transformToDisplay.gameObject.SetActive(true);
            LeanTween.scale(transformToDisplay, Vector2.one, OpenPanelSpeed).setDelay(delay).setOnComplete(chainAction);
        }
        
        public static void DisplayPanel(GameObject transformToDisplay, float delay = 0.0f)
        {
            transformToDisplay.gameObject.SetActive(true);
            LeanTween.scale(transformToDisplay, Vector2.one, OpenPanelSpeed).setDelay(delay);
        }
        
        public static void HidePanel(GameObject transformToDisplay, Action chainAction, float delay = 0.0f)
        {
            transformToDisplay.gameObject.SetActive(true);
            LeanTween.scale(transformToDisplay, Vector2.zero, ClosePanelSpeed).setDelay(delay).setOnComplete( () =>
            {
                transformToDisplay.gameObject.SetActive(false);
                chainAction();
            });
        }
    }
}
