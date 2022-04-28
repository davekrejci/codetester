<template>
	<div>
		<navigation-drawer></navigation-drawer>
		<v-main>
			<v-container :class="borderClass" fluid>
				<router-view :key="$route.fullPath"></router-view>
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
	computed: {
		borderClass() {
			if(this.$vuetify.theme.dark) {
				return 'darkTopBorder';
			}
			else {
				return 'lightTopBorder';
			}
		}
	}
};
</script>

<style scoped>
.lightTopBorder {
	border-top: 1px solid #eee;
}
.darkTopBorder {
	border-top: 1px solid #4a4f5c;
}
</style>
