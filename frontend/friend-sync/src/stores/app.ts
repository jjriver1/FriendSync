// Utilities
import { defineStore } from "pinia";

export const useAppStore = defineStore("app", {
	state: () => ({
		isLoggedIn: false,
		username: "",
		email: "",
		bio: "",
		jwt: "",
	}),
});
