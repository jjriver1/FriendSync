<template>
	<!-- way too hard to center stuff -->
	<!-- convert create account card into component to simplify this page? -->
	<CreateAccountCard />
</template>

<script lang="ts">
import { defineComponent } from "vue";
import CreateAccountCard from "@/components/CreateAccountCard.vue";

export default defineComponent({
	name: "CreateAccount",
	components: {
		CreateAccountCard,
	},
});
</script>

<style scoped lang="sass"></style>
