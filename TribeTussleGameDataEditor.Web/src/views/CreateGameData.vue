<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" ref="nav-bar" />
        <game-editor-form initGameDataName="" :updateNavBarGameNames="updateNavBarGameNames"
                          :rounds="defaultRounds" :fastMoneyQuestions="defaultFastMoneyQuestions"/>
    </div>
</template>

<script lang="ts">
    import { Component } from 'vue-property-decorator';
    import AuthenticatedView from './AuthenticatedView';
    import NavBar from '@/components/NavBar.vue';
    import GameEditorForm from '@/components/game-editor-form/GameEditorForm.vue';
    import Round from '@/components/types/Round';
    import { defaultRoundList, defaultFastMoneyQuestionList } from '@/views/Constants';

    @Component({
        components: {
            NavBar,
            GameEditorForm
        }
    })
    export default class CreateGameData extends AuthenticatedView {
        private readonly defaultRounds: Round[] = defaultRoundList;
        private readonly defaultFastMoneyQuestions: Round[] = defaultFastMoneyQuestionList;

        public async updateNavBarGameNames(): Promise<void> {
            await (this.$refs['nav-bar'] as any).fetchGameNames();
        }
    }
</script>

<style scoped>
</style>