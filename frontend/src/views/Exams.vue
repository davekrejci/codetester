<template>
	<v-row>
		<v-col cols="12" md="6">
			<v-badge class="font-weight-bold" :content="exams.length" offset-x="0" offset-y="10">
				<h1>Vaše testy</h1>
			</v-badge>
			<ExamCard v-for="exam in exams" :key="exam.id" :exam="exam" />
		</v-col>
		<v-col cols="6"></v-col>
	</v-row>
</template>

<script>
// @ is an alias to /src
import ExamCard from '@/components/ExamCard.vue';

export default {
	name: 'Exams',
	components: {
		ExamCard,
	},
	data: () => ({
		exams: [
			{
				id: 1,
				className: 'NAVZO - Návrhové vzory',
				examName: 'Semestrální test #1',
			},
			{
				id: 2,
				className: 'EPARP - Multivláknové a paralelní programování',
				examName: 'Průběžný test #4',
			},
			{
				id: 3,
				className: 'Wapid - Webové API a strojová výměna dat',
				examName: 'REST API',
			}
		],
	}),
};
</script>
