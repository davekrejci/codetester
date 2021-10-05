<template>
	<v-row>
		<v-col cols="12" md="6">
			<v-badge class="font-weight-bold" :content="assignements.length" offset-x="0" offset-y="10">
				<h1>Vaše úkoly</h1>
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
	name: 'Assignements',
	components: {
		AssignementCard,
	},
	data: () => ({
		assignements: [
			{
				id: 1,
				className: 'NAVZO - Návrhové vzory',
				assignementName: 'Vzor Model-View-Controller',
				description: 'Implementujte návrhový vzor MVC',
			},
			{
				id: 2,
				className: 'EPARP - Multivláknové a paralelní programování',
				assignementName: 'Ovládání chování vlákna a běhové pořadí',
				description: 'Dokažte vaše znalosti ovládání chování vlákna ve vícevláknovém prostředí',
			},
			{
				id: 3,
				className: 'Wapid - Webové API a strojová výměna dat',
				assignementName: 'REST API',
				description: 'Vytvořte REST API s endpointy pro zobrazení a postování komentů.',
			},
			{
				id: 4,
				className: 'Wapid - Webové API a strojová výměna dat',
				assignementName: 'REST API',
				description: 'Vytvořte REST API s endpointy pro zobrazení a postování komentů.',
			},
			{
				id: 5,
				className: 'Wapid - Webové API a strojová výměna dat',
				assignementName: 'REST API',
				description: 'Vytvořte REST API s endpointy pro zobrazení a postování komentů.',
			},
			{
				id: 6,
				className: 'Wapid - Webové API a strojová výměna dat',
				assignementName: 'REST API',
				description: 'Vytvořte REST API s endpointy pro zobrazení a postování komentů.',
			},
			{
				id: 7,
				className: 'Wapid - Webové API a strojová výměna dat',
				assignementName: 'REST API',
				description: 'Vytvořte REST API s endpointy pro zobrazení a postování komentů.',
			},
		],
	}),
};
</script>
