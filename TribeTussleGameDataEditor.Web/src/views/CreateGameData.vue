<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" />
        <game-editor-form initGameDataName="Test"
                          :rounds="[{ prompt: 'Foo', id: 1 }, { prompt: 'Bar', id: 2 }, { prompt: 'Baz', id: 3 }, { prompt: 'Last', id: 4 }]"/>
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