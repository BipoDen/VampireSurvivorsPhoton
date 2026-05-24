using _Project.Logic.Multiplayer.Gameplay;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Multiplayer
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Rigidbody2D _rigidbody;

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetInput input))
            {
                Vector2 direction = input.Direction;
                transform.position += (Vector3)(direction * _speed * Runner.DeltaTime);
            }
        }
        
    }
}