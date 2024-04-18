<template>
	<v-container class="fill-height">
		<v-row justify="center">
			<v-col sm="12" md="8" align-self="center">
				<h1 class="text-h3 text-md-h2 text-lg-h1">FriendSync</h1>
				<br />
				<p class="text-h6 text-lg-h5">
					Connecting through blogs now, syncing friends later - Welcome to FriendSync!
				</p>
			</v-col>
			<v-col sm="12" md="4" align-self="center">
				<CreateNewAccountCard />
			</v-col>
		</v-row>
	</v-container>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import CreateNewAccountCard from "@/components/CreateNewAccountCard.vue";

export default defineComponent({
	name: "CreateAccount",
	components: {
		CreateNewAccountCard,
	},
});
</script>

<style scoped lang="sass">
</style>
