<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" />
        <game-editor-form initGameDataName="Test"
                          :rounds="defaultRounds"/>
    </div>
</template>

<script lang="ts">
    import { Component, Prop } from 'vue-property-decorator';
    import AuthenticatedView from './AuthenticatedView';
    import NavBar from '@/components/NavBar.vue';
    import GameEditorForm from '@/components/game-editor-form/GameEditorForm.vue';
    import Round from '@/components/types/Round';

    @Component({
        components: {
            NavBar,
            GameEditorForm
        }
    })
    export default class CreateGameData extends AuthenticatedView {
        private defaultRounds: Round[] = [
            {
                id: 1,
                prompt: '',
                answers: [
                    { id: 1, text: '', value: 0 }
                ],
                active: true
            },
            {
                id: 2,
                prompt: '',
                answers: [
                    { id: 1, text: '', value: 0 }
                ],
                active: false
            }
        ];
    }
</script>

<style scoped>
</style>