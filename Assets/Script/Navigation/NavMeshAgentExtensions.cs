using UnityEngine.AI;

namespace Navigation
{
    public static class NavMeshAgentExtensions
    {
        public static void SetupAgent(this NavMeshAgent agent, NavMeshAgentConfiguration config)
        {
            agent.agentTypeID = config.AgentTypeId;
            agent.baseOffset = config.BaseOffset;
            agent.speed = config.Speed;
            agent.angularSpeed = config.AngularSpeed;
            agent.acceleration = config.Acceleration;
            agent.stoppingDistance = config.StoppingDistance;
            agent.autoBraking = config.AutoBraking;
            agent.radius = config.Radius;
            agent.height = config.Height;
            agent.obstacleAvoidanceType = config.Quality;
            agent.avoidancePriority = config.AgentPriority;
            agent.autoTraverseOffMeshLink = config.AutoTraverseOffMeshLink;
            agent.autoRepath = config.AutoRepath;
            agent.areaMask = config.AreaMask;
        }
        
        public static NavMeshAgentConfiguration ModifyBySettings(this NavMeshAgentConfiguration config, NavMeshBuildSettings settings)
        {
            return new NavMeshAgentConfiguration(settings.agentTypeID, config.BaseOffset, config.Speed, config.AngularSpeed, config.Acceleration,
                config.StoppingDistance, config.AutoBraking, settings.agentRadius, settings.agentHeight, config.Quality, config.AgentPriority,
                config.AutoTraverseOffMeshLink, config.AutoRepath, config.AreaMask);
        }
    }
}