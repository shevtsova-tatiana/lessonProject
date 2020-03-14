using Unity.Entities;
using Unity.Jobs;

namespace ECS
{
    public class LifeSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var deltaTime = World.Time.DeltaTime;
            
            Entities.WithoutBurst().WithStructuralChanges().ForEach((ref Entity entity, ref EffectLifeComponent component) =>
                {
                    component.LifeTime -= deltaTime;
                    if (component.LifeTime <= 0f)
                    {
                        EffectController.EntityManager.DestroyEntity(entity);
                    }
                }).Run();
            return default;
        }
    }
}