using UnityEngine;
using UnityEngine.UI;

namespace ECS.Components
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private int _value;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                Text.text = _value.ToString();
            }
        }
        
        public Text Text;
    }
}