<template>
	<v-hover v-slot:default="{ hover }">
		<v-card class="my-2 pa-1" :to="'/exam-detail/' + exam.id" flat :class="{ 'on-hover': hover }">
			<v-list-item three-line>
				<v-list-item-content>
					<div class="overline mb-2">{{ exam.className }}</div>
					<v-list-item-title class="headline mb-2">{{ exam.examName }}</v-list-item-title>
				</v-list-item-content>
			</v-list-item>
			<v-card-actions>
				<v-btn text class="primary">Zobrazit detaily</v-btn>
			</v-card-actions>
		</v-card>
	</v-hover>
</template>

<script>
export default {
	name: 'ExamCard',
	props: {
		exam: {
			type: Object,
			required: true,
		},
	},
};
</script>

<style scoped>
.v-card {
	transition: opacity 0.8s ease-in-out;
}

.v-card:hover {
	background-color: var(--v-secondary-base) !important;
	color: white !important;
}
.v-card:hover .v-list-item__title,
.v-card:hover .v-list-item__subtitle,
.v-card:hover div {
	color: #fafafe !important;
}
</style>
