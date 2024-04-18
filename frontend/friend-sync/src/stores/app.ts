// Utilities
import { defineStore } from "pinia";

export const useAppStore = defineStore("app", {
	state: () => ({ isLoggedIn: false }),
	actions: {
		login(): void {
			this.isLoggedIn = true;
		},
		logout(): void {
			this.isLoggedIn = false;
		},
	},
});
