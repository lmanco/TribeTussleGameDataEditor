<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" />
        <game-editor-form initGameDataName="Test"
                          :rounds="[{ prompt: '', id: 1 }, { prompt: '', id: 2 }]"/>
    </div>
</template>

<script lang="ts">
    import { Component, Prop } from 'vue-property-decorator';
    import AuthenticatedView from './AuthenticatedView';
    import NavBar from '@/components/NavBar.vue';
    import GameEditorForm from '@/components/game-editor-form/GameEditorForm.vue';

    @Component({
        components: {
            NavBar,
            GameEditorForm
        }
    })
    export default class Home extends AuthenticatedView {
    }
</script>

<style scoped>
</style>