<template>
	<div>
		<navigation-drawer></navigation-drawer>
		<v-main>
			<v-container class="" fluid>
				<router-view></router-view>
			</v-container>
		<bottom-navigation></bottom-navigation>
		</v-main>
	</div>
</template>

<script>
import NavigationDrawer from '@/components/NavigationDrawer.vue';
import BottomNavigation from '@/components/BottomNavigation.vue';


export default {
	components: {
		NavigationDrawer,
		BottomNavigation,
	},
};
</script>
