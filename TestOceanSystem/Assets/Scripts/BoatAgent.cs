using NWH.DWP2.ShipController;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class BoatAgent : Agent
{
	[SerializeField] private AdvancedShipController ship;

	Vector3 startPos;
	Quaternion startRot;

	private void Awake()
	{
		startPos = transform.position;
		startRot = transform.rotation;
	}

	public override void Initialize()
	{
		ship.input.autoSetInput = false;
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		float throttle = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
		float steering = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

		ship.input.Throttle = throttle;
		ship.input.Steering = steering;

		float forwardSpeed = Vector3.Dot(
		ship.transform.forward,
		ship.vehicleRigidbody.linearVelocity
	);

		AddReward(forwardSpeed * 0.01f);
		AddReward(-Mathf.Abs(steering) * 0.001f);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var a = actionsOut.ContinuousActions;
		a[0] = Input.GetAxis("Vertical");
		a[1] = Input.GetAxis("Horizontal");
	}

	public override void OnEpisodeBegin()
	{
		ship.vehicleRigidbody.linearVelocity = Vector3.zero;
		ship.vehicleRigidbody.angularVelocity = Vector3.zero;

		ship.transform.position = startPos;
		ship.transform.rotation = startRot;
	}
}
