<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" />
    </div>
</template>

<script lang="ts">
    import { Component, Prop } from 'vue-property-decorator';
    import AuthenticatedView from './AuthenticatedView';
    import NavBar from '@/components/NavBar.vue';

    @Component({
        components: {
            NavBar
        }
    })
    export default class Home extends AuthenticatedView {
    }
</script>

<style scoped>
</style>