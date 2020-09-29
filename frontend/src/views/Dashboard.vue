<template>
	<div>
		<navigation-drawer></navigation-drawer>
		<v-main>
			<v-container class="pl-4" fluid>
				<router-view></router-view>
			</v-container>
		</v-main>
	</div>
</template>

<script>
import NavigationDrawer from '@/components/NavigationDrawer.vue';

export default {
	components: {
		NavigationDrawer,
	},
};
</script>
