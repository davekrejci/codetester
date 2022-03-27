<template>
	<div>
		<mobile-navigation></mobile-navigation>
		<navigation-drawer></navigation-drawer>
		<v-main>
			<v-container class="" fluid>
				<router-view :key="$route.fullPath"></router-view>
			</v-container>
		</v-main>
	</div>
</template>

<script>
import NavigationDrawer from '@/components/NavigationDrawer.vue';
import MobileNavigation from '@/components/MobileNavigation.vue';


export default {
	components: {
		NavigationDrawer,
		MobileNavigation,
	},
};
</script>
