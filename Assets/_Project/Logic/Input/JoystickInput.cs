using UnityEngine;

namespace _Project.Logic.Input
{
    public class JoystickInput : IInput
    {
        public Vector2 Move()
        {
            float horizontal = SimpleInput.GetAxis("Horizontal");
            float vertical = SimpleInput.GetAxis("Vertical");

            return new Vector2(horizontal, vertical);
        }
    }
}