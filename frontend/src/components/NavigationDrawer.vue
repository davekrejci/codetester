<template>
	<v-navigation-drawer :width="280" app permanent floating mini-variant-width="80" :mini-variant="$vuetify.breakpoint.mdAndDown">
		<v-list class="mt-2">
			<v-list-item class="px-2">
				<v-img src="../assets/svg/logo/symbol_logo.svg" max-height="25" max-width="60" contain></v-img>
				<v-img src="../assets/svg/logo/text_logo.svg" max-height="100" max-width="150" contain></v-img>
			</v-list-item>
		</v-list>
		<v-list nav>
			<v-list-item link>
				<v-badge bordered bottom color="success" dot offset-x="10" offset-y="18">
					<v-list-item-avatar class="mx-0" color="primary">
						<!-- <v-icon dark>mdi-account-circle</v-icon> -->
						<v-img src="https://randomuser.me/api/portraits/men/32.jpg"></v-img>
					</v-list-item-avatar>
				</v-badge>
				<v-list-item-content class="ml-4">
					<v-list-item-title class="font-weight-medium">David Krejci</v-list-item-title>
					<v-list-item-subtitle>Student</v-list-item-subtitle>
				</v-list-item-content>

				<v-list-item-action>
					<v-icon>mdi-menu-down</v-icon>
				</v-list-item-action>
			</v-list-item>
		</v-list>
		<v-list nav dense>
			<v-list-item-group color="primary">
				<v-list-item v-for="item in items" :key="item.title" :to="item.path" link>
					<v-list-item-action>
						<v-icon>{{ item.icon }}</v-icon>
					</v-list-item-action>
					<v-list-item-content>
						<v-list-item-title>{{ item.title }}</v-list-item-title>
					</v-list-item-content>
				</v-list-item>
			</v-list-item-group>
		</v-list>
	</v-navigation-drawer>
</template>

<script>
export default {
	name: 'NavigationDrawer',
	data: () => ({
		items: [
			{ title: 'Dashboard', path: '/', icon: 'mdi-view-dashboard' },
			{ title: 'Settings', path: '/settings', icon: 'mdi-cog' },
		],
	}),
};
</script>
