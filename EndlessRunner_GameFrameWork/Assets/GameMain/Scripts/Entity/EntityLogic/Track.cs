using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner
{
	public class Track : TrackManager
	{

		private TrackData m_TrackData = null;

		protected float m_TimeSinceLastPremium;
		protected float m_TimeSincePowerup;     // The higher it goes, the higher the chance of spawning one

		public float laneOffset = 1.0f;
		protected override void OnShow(object userData)
		{
			base.OnShow(userData);
			m_TrackData = (TrackData)userData;
			if (AllTrack.Count<1)
			{
				CachedTransform.SetLocalPosition(m_TrackData.StartPostion);
			}
			else 
			{
				CachedTransform.SetLocalPosition( new Vector3(0,-2.2f,ProTrack.GetComponent<TrackSegment>().pathParent.GetChild(1).transform.position.z + 4f));
			}
			ProTrack = this;
			AllTrack.Enqueue(this);

			if (m_TrackData.TypeId==34)
			{

				TrackInitFinished = true;
				CurrentGoneTrack = AllTrack.Dequeue();
			}

			SpawnObstacle(this.GetComponent<TrackSegment>()) ;
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
			PowerupSpawnUpdate();
		}

		protected override void OnHide(object userData)
		{
			base.OnHide(userData);
		}

		public void PowerupSpawnUpdate()
		{
			m_TimeSincePowerup += Time.deltaTime;
			m_TimeSinceLastPremium += Time.deltaTime;
		}

		void OnColliderEnter(Collider other)
		{
			Debug.Log(other.name);
		}
		/// <summary>
		/// 生成障碍
		/// </summary>
		/// <param name="segment"></param>
		public void SpawnObstacle(TrackSegment segment)
		{
			if (segment.possibleObstacles.Length != 0)
			{
				for (int i = 0; i < segment.obstaclePositions.Length; ++i)
				{
					segment.possibleObstacles[Random.Range(0, segment.possibleObstacles.Length)].Spawn(segment, segment.obstaclePositions[i]);
				}
			}

			//SpawnCoinAndPowerup(segment);
		}

		/// <summary>
		/// 生成金币和Powerup
		/// </summary>
		/// <param name="segment"></param>
		public void SpawnCoinAndPowerup(TrackSegment segment)
		{
			const float increment = 1.5f;
			float currentWorldPos = 0.0f;
			int currentLane = Random.Range(0, 3);

			float powerupChance = Mathf.Clamp01(Mathf.Floor(m_TimeSincePowerup) * 0.5f * 0.001f);
			float premiumChance = Mathf.Clamp01(Mathf.Floor(m_TimeSinceLastPremium) * 0.5f * 0.0001f);

			while (currentWorldPos < segment.worldLength)
			{
				Vector3 pos;
				Quaternion rot;
				segment.GetPointAtInWorldUnit(currentWorldPos, out pos, out rot);


				bool laneValid = true;
				int testedLane = currentLane;
				while (Physics.CheckSphere(pos + ((testedLane - 1) * laneOffset * (rot * Vector3.right)), 0.4f, 1 << 9))
				{
					testedLane = (testedLane + 1) % 3;
					if (currentLane == testedLane)
					{
						// Couldn't find a valid lane.
						laneValid = false;
						break;
					}
				}

				currentLane = testedLane;

				if (laneValid)
				{
					pos = pos + ((currentLane - 1) * laneOffset * (rot * Vector3.right));


					GameObject toUse;
					if (Random.value < powerupChance)
					{
						int picked = Random.Range(0, 10);

						//if the powerup can't be spawned, we don't reset the time since powerup to continue to have a high chance of picking one next track segment
						if (true)
						{
							// Spawn a powerup instead.
							m_TimeSincePowerup = 0.0f;
							powerupChance = 0.0f;

							toUse = Instantiate(segment.possibleObstacles[1].gameObject, pos, rot) as GameObject;
							toUse.transform.SetParent(segment.transform, true);
						}
					}
					else if (Random.value < premiumChance)
					{
						m_TimeSinceLastPremium = 0.0f;
						premiumChance = 0.0f;

						toUse = Instantiate(segment.possibleObstacles[2].gameObject, pos, rot);
						toUse.transform.SetParent(segment.transform, true);
					}
					else
					{
						toUse = Coin.coinPool.Get(pos, rot);
						toUse.transform.SetParent(segment.collectibleTransform, true);
					}


				}

				currentWorldPos += increment;
			}

		}
	}
	
}