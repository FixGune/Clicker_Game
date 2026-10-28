using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        [Header("Main Game - Top Panel")]
        [SerializeField] private Text currentCashLabel;
        [SerializeField] private Text currentPremiumCurrencyLabel;



        public static UIManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void SetCurrentCashLabel(double value)
        {
            string labelValue = FormatNumbers.Format(value);
            currentCashLabel.text = labelValue;
        }

        public void SetCurrentPremiumCurrencyLabel(double value)
        {
            string labelValue = FormatNumbers.Format(value);
            currentPremiumCurrencyLabel.text = labelValue;
        }
    }
}