<template>
	<v-container fill-height fluid>
		<v-app-bar color="white" flat app>

  <v-img
    class="mx-2"
    src="../assets/svg/logo/full_logo.svg"
    max-height="150"
    max-width="150"
    contain
  ></v-img>
</v-app-bar>
		<v-row>
			<v-col>
				<v-img class="mx-auto" src="../assets/svg/illustrations/404.svg" :max-width="illustrationWidth" alt="" />
				<h2 class="mx-2 mt-6 text-center">Ups! Tahle stránka neexistuje!</h2>
				<p class="mx-2 text-subtitle text-center">Zkontrolujte, že máte správnou URL adresu, vraťte se na předchozí stránku, nebo na domovskou stránku</p>
				<div class="text-center">
					<v-btn large rounded text class="px-5 primary" href="/">Vrátit se domů</v-btn>
				</div>
			</v-col>
		</v-row>
	</v-container>
</template>

<script>
export default {
	computed: {
		illustrationWidth() {
			switch (this.$vuetify.breakpoint.name) {
				case 'xs':
					return 300;
				case 'sm':
					return 350;
				case 'md':
					return 500;
				case 'lg':
					return 600;
				case 'xl':
					return 800;
				default:
					return 200;
			}
		},
	},
};
</script>

<style>
	.v-image{
		display: bloc;
	}
</style>
