<template>
	<div v-for="post in posts" :key="post.createdAt">
		<h2>
			{{ post.authorUsername }}
		</h2>
		<p>
			{{ post.content }}
		</p>
	</div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { useAppStore } from "@/stores/app.ts";
import router from "@/router";
import { getPosts } from "@/api.ts";

interface Post {
	authorUsername: string;
	content: string;
	createdAt: string;
}

const store = useAppStore();

export default defineComponent({
	name: "DashBoard",
	data: function () {
		return {
			posts: [] as Post[],
		};
	},
	beforeMount: () => {
		if (!store.isLoggedIn) {
			router.push("/login");
		}
	},
	mounted: async function (){
		const response = await getPosts(store.username);
		if (response.status === 200) {
			console.log(response.data);

			for (const post of response.data) {;
				this.posts.push({
					authorUsername: post.authorUsername,
					content: post.content,
					createdAt: post.createdAt,
				});
			}

			console.log(this.posts);
		} else {
			console.error(
				"Failed to get posts. \n" +
					"Status was: " +
					response.status +
					"\nData was: " +
					response.data,
			);
		}
	},
});
</script>
<style scoped>
div {
	margin: 10px;
	padding: 10px;
	border: 1px solid black;
	display: flex;
	flex-direction: column;
}
</style>
