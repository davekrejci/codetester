<template>
	<v-app>
		<navigation-drawer></navigation-drawer>
		<v-main>
			<v-container class="pl-4" fluid>
				<router-view></router-view>
			</v-container>
		</v-main>
	</v-app>
</template>

<script>
import NavigationDrawer from '@/components/NavigationDrawer.vue';

export default {
	components: {
		NavigationDrawer,
	},
};
</script>

<style>
@import './App.css';
</style>
