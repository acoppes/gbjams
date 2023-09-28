using Game.Triggers;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM11.Triggers
{
    public class ConnectTunnelTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target1, target2;

        public override string GetObjectName()
        {
            return "ConnectTunnel()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var t1 = target1.Get(world);
            var t2 = target2.Get(world);

            t1.Get<KunaiTunnelComponent>().exitEntity = t2;
            t2.Get<KunaiTunnelComponent>().exitEntity = t1;
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}