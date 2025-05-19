using UnityEngine;

namespace CharmCrab.Charms {
	class SteadyBody {
		public readonly float ProcRate = 0.5f;
		public readonly float FAIL_TIME = 0.2f;
		public readonly float ACTIVE_TIME = 0.1f;

		private float time = 0;
		private bool succeed = true;

		public void Update() {
			this.time -= Time.deltaTime;
		}

		/*
		private void DoCheck() {
			if (Random.Range(0f, 1f) < ProcRate) {
				this.succeed = true;
			}
		}
		*/

		public int TakeDamage(ref int hazard, int dmg) {
			if (PlayerData.instance.GetBool("equippedCharm_14")) {
				if (succeed) {
					if (this.time <= 0) {
						this.succeed = false;
						return dmg;
					} else {
						return 0;
					}
				} else {
					if (this.time <= 0) {
						this.succeed = (Random.Range(0f, 1f) < ProcRate);
						if (this.succeed) {
							this.time = ACTIVE_TIME;
							return 0;
						} else {
							this.time = FAIL_TIME;
							return dmg;
						}
					} else {
						return dmg;
					}
				}
			} else {
				return dmg;
			}
		}
	}
}
