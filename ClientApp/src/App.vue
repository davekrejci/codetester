<template>
	<v-app>
		<router-view></router-view>
	</v-app>
</template>

<script>
export default {
	created() {
		// start ticker for calculating current time app wide
		this.$store.dispatch("startTicker");
	}
};
</script>

<style>
@import './App.css';
</style>
