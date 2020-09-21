<template>
	<v-row>
		<v-col cols="12" md="6">
			<v-badge class="font-weight-bold" :content="assignements.length" offset-x="0" offset-y="10">
				<h1>Your Assignements</h1>
			</v-badge>
			<AssignementCard v-for="assignement in assignements" :key="assignement.id" :assignement="assignement" />
		</v-col>
		<v-col cols="6"></v-col>
	</v-row>
</template>

<script>
// @ is an alias to /src
import AssignementCard from '@/components/AssignementCard.vue';

export default {
	name: 'Dashboard',
	components: {
		AssignementCard,
	},
	data: () => ({
		assignements: [
			{
				id: 1,
				className: 'NAVZO - Design Patterns',
				assignementName: 'Model-View-Controller Design Pattern',
				description: 'Implement the MVC design pattern',
			},
			{
				id: 2,
				className: 'EPARP - Multithreaded and parallel programming',
				assignementName: 'Controlling thread behavior and running order',
				description: 'Present your knowledge of controlling thread behaviour in a multithreaded environement',
			},
			{
				id: 3,
				className: 'Wapid - Web API and Machine Data Exchange',
				assignementName: 'REST API',
				description: 'Create a REST API with routes for getting and posting comments.',
			},
		],
	}),
};
</script>
