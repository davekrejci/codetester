<template>
	<v-container class="pa-0" fill-height fluid>
		<v-row dense class="fill-height">
			<v-col class="col-12 col-md-6">
				<v-row class="fill-height" align="center" justify="center">
					<v-img class="mx-auto" src="../assets/svg/illustrations/login.svg" :max-width="illustrationWidth" alt="" />
				</v-row>
			</v-col>
			<v-col class="">
				<v-row class="fill-height" :align="$vuetify.breakpoint.smAndDown ? 'start' : 'center'" justify="center">
					<div id="loginContainer" :align="$vuetify.breakpoint.smAndDown ? 'center' : 'start'">
						<v-img class="mb-4" src="../assets/svg/logo/text_logo.svg" max-height="100" max-width="150" contain></v-img>
						<h1>Welcome Back!</h1>
						<p>Please sign in to continue.</p>
						<v-form v-model="valid" ref="form" lazy-validation @keyup.native.enter="submit">
							<v-text-field
								label="Email"
								v-model="email"
								required
								solo
								flat
								hide-details="auto"
								single-line
								prepend-inner-icon="mdi-email-outline"
								class="mb-1"
							></v-text-field>
							<v-text-field
								label="Password"
								v-model="password"
								single-line
								required
								solo
								hide-details="auto"
								flat
								class="mb-1"
								prepend-inner-icon="mdi-lock-outline"
								:append-icon="value ? 'mdi-eye-outline' : 'mdi-eye-off-outline'"
								@click:append="() => (value = !value)"
								:type="value ? 'password' : 'text'"
							>
							</v-text-field>
							<a id="forgotPasswordText" class="text--secondary text-caption">Forgot your password?</a>
						</v-form>
						<v-card-actions>
							<v-btn large block rounded text class="mt-2 px-5 primary" @click="submit" :disabled="!valid || loading">Sign In</v-btn>
						</v-card-actions>
						<v-divider class="my-4"></v-divider>
						<p class="text-center text--secondary">Don't have an account yet? <a id="signUpText" class="text-decoration-none" href="#">Sign up</a></p>
					</div>
				</v-row>
			</v-col>
		</v-row>
	</v-container>
</template>

<script>
export default {
	methods: {
		async submit() {
			console.log(`submitting 
			email: ${this.email}
			password: ${this.password}`);
		},
	},
	data: () => ({
		loading: false,
		valid: true,
		email: '',
		password: '',
		responseStatus: null,
		value: String,
	}),
	computed: {
		illustrationWidth() {
			switch (this.$vuetify.breakpoint.name) {
				case 'xs':
					return 200;
				case 'sm':
					return 250;
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

<style scoped>
#loginContainer {
	width: 80%;
	max-width: 500px;
}
#forgotPasswordText:hover {
	color: var(--v-primary-lighten1) !important;
}
#signUpText:hover {
	color: var(--v-primary-lighten1);
}
>>> .v-input__prepend-inner .v-icon {
	margin-right: 5px;
}
</style>
