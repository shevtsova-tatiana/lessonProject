using Unity.Entities;
using Unity.Mathematics;

namespace ECS
{
    public struct EffectMovementComponent : IComponentData
    {
        public float3 direction;
        public float speed;
    }
}