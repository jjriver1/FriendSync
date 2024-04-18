<template>
	<v-card title="Login" width="400" class="text-center pa-4" color="secondary">
		<v-sheet class="mx-auto" width="300" color="secondary">
			<v-form @submit.prevent>
				<v-text-field v-model="email" type="text" label="Email"></v-text-field>
				<v-text-field
					v-model="password"
					type="password"
					label="Password"
				></v-text-field>
				<!--<div>Forgot Password?</div>-->
				<v-btn class="mt-2" block @click="login">Log In</v-btn>
				<v-divider class="ma-4 border-opacity-50"></v-divider>
				<v-btn class="mt-2" block @click="openCreateAccount">
					Create New Account
				</v-btn>
			</v-form>
		</v-sheet>
	</v-card>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import router from "@/router";
import { login } from "@/api.ts";
import { useAppStore } from "@/stores/app.ts";
import { jwtDecode } from "jwt-decode";

const store = useAppStore();

interface MyToken {
	email: string;
	unique_name: string;
}

export default defineComponent({
	name: "LoginCard",
	data: function () {
		return {
			email: "",
			password: "",
		};
	},
	methods: {
		async login() {
			const response = await login(this.email, this.password);
			if (response.status === 200) {
				const decodedJWT = jwtDecode<MyToken>(response.data);
				store.isLoggedIn = true;
				store.jwt = response.data;
				store.email = decodedJWT["email"];
				store.username = decodedJWT["unique_name"];
				router.push("/");
			} else {
				console.error(
					"Failed to login. \n" +
						"Status was: " +
						response.status +
						"\nData was: " +
						response.data,
				);
			}
		},
		openCreateAccount() {
			router.push("/create-account");
		},
	},
});
</script>

<style scoped></style>
