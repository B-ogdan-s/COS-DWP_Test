from mlagents_envs.environment import UnityEnvironment

env = UnityEnvironment()
env.reset()

behavior_name = list(env.behavior_specs.keys())[0]
spec = env.behavior_specs[behavior_name]

print("Behavior:", behavior_name)

print("Observation specs:")
for obs in spec.observation_specs:
    print(" - shape:", obs.shape, " type:", obs.observation_type)

print("Action size:", spec.action_spec.continuous_size)

env.close()