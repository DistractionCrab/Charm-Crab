﻿using System;
using System.Collections.Generic;
using System.Text;
using Vasi;

namespace CharmCrab.Charms {
	class DreamWielder {
		public static readonly float MULT = 1.5f;

		// Cooldown being used a duration tool. Check for !Available to see if it's active.
		private Cooldown cd = new Cooldown(5f);

		public void Update() {
			this.cd.Update();
		}

		public void DreamWielderAction() {
			this.cd.Reset();
		}

		public int Damage(int i) {
			if (CharmData.Equipped(Charm.DreamWielder) && !this.cd.Available) {
				return (int)(i * MULT);
			} else {
				return i;
			}
		}
	}
}