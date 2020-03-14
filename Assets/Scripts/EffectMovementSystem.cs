
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace ECS
{
    public class EffectMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = World.Time.DeltaTime;
            var job = Entities.ForEach((ref Translation translation, in EffectMovementComponent component) =>
            {
                translation.Value += component.direction * component.speed * deltaTime;
            }).Schedule(inputDeps);
            
            job.Complete();
            return job;
        }
    }
}