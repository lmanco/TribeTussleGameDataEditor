<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" />
        <game-editor-form />
    </div>
</template>

<script lang="ts">
    import { Component, Prop } from 'vue-property-decorator';
    import AuthenticatedView from './AuthenticatedView';
    import NavBar from '@/components/NavBar.vue';
    import GameEditorForm from '@/components/GameEditorForm.vue';

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