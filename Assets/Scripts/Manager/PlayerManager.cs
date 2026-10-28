using UnityEngine;


namespace Manager
{
    public class PlayerManager : MonoBehaviour
    {
        private double currentCash;
        private double currentPremiumCurrency;

        public double CurrentCash 
        { get 
            { 
                return currentCash; 
            } 
            set 
            {  currentCash = value;
               UIManager.Instance.SetCurrentCashLabel(currentCash); 
            } 
        }
        public static PlayerManager Instance;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
    }
}
