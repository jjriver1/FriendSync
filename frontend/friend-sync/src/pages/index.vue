<template>
	<HelloWorld />
</template>

<script lang="ts" setup>
import HelloWorld from "@/components/HelloWorld.vue";
import { useAppStore } from "@/stores/app.ts";
import router from "@/router";
import { getPosts } from "@/api.ts";

const store = useAppStore();

onBeforeMount(() => {
	if (!store.isLoggedIn) router.push("/login");
});

onMounted(async () => {
	const response = await getPosts(store.username);
	if (response.status === 200) {
		console.log(response.data);
	} else {
		console.error(
			"Failed to get posts. \n" +
				"Status was: " +
				response.status +
				"\nData was: " +
				response.data,
		);
	}
});
</script>
