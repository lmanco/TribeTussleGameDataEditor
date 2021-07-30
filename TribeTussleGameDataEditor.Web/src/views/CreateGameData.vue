<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" />
        <game-editor-form initGameDataName=""
                          :rounds="defaultRounds" :fastMoneyQuestions="defaultFastMoneyQuestions"/>
    </div>
</template>

<script lang="ts">
    import { Component } from 'vue-property-decorator';
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
        private readonly defaultRounds: Round[] = [
            {
                id: 1,
                prompt: '',
                scale: 1,
                answers: [
                    { id: 1, text: '', value: 0 }
                ],
                active: true
            },
            {
                id: 2,
                prompt: '',
                scale: 1,
                answers: [
                    { id: 1, text: '', value: 0 }
                ],
                active: false
            }
        ];

        private readonly defaultFastMoneyQuestions: Round[] = [
            {
                id: 1,
                prompt: '',
                scale: 1,
                answers: [{ id: 1, text: '', value: 0 }],
                active: true
            },
            {
                id: 2,
                prompt: '',
                scale: 1,
                answers: [{ id: 1, text: '', value: 0 }],
                active: false
            },
            {
                id: 3,
                prompt: '',
                scale: 1,
                answers: [{ id: 1, text: '', value: 0 }],
                active: false
            },
            {
                id: 4,
                prompt: '',
                scale: 1,
                answers: [{ id: 1, text: '', value: 0 }],
                active: false
            },
            {
                id: 5,
                prompt: '',
                scale: 1,
                answers: [{ id: 1, text: '', value: 0 }],
                active: false
            }
        ]
    }
</script>

<style scoped>
</style>