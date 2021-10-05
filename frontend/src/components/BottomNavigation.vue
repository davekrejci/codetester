<template>
  <v-bottom-navigation color="primary" v-if="$vuetify.breakpoint.smAndDown" app v-model="value">
    <v-btn  v-for="item in items" :key="item.title" :to="item.path" :value="item.title">
      <span>{{ item.title }}</span>

      <v-icon >{{ item.icon }}</v-icon>
    </v-btn>
    <v-btn>
				<v-badge bordered bottom color="success" dot offset-x="10" offset-y="18">
					<v-list-item-avatar class="mx-0" color="primary">
						<v-img src="https://randomuser.me/api/portraits/men/32.jpg"></v-img>
					</v-list-item-avatar>
				</v-badge>
				</v-btn>
  </v-bottom-navigation>
</template>

<script>
export default {
	name: 'BottomNavigation',
	data: () => ({
		items: [
			{ title: 'Úkoly', path: '/assignements', icon: 'mdi-view-dashboard' },
			{ title: 'Nastavení', path: '/settings', icon: 'mdi-cog' },
		],
	}),
};
</script>

<style>

</style>