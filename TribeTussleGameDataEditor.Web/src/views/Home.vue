<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" />
        <b-alert v-if="showActivationSuccessMessage" id="account-activated-alert" variant="success" dismissible show>
            Email confirmed. Your account is now activated!
        </b-alert>
        <b-container class="text-center mt-5">
            <h1>Welcome, {{ authenticatedUser.username }}!</h1>
            <p>
                Create a new game from the
                <span class="font-italic">Add Game Data</span>
                menu above, or select an existing game using the
                <span class="font-italic">Select Game Data...</span> dropdown box.
            </p>
        </b-container>
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
        @Prop() showActivationSuccessMessage!: boolean;
    }
</script>

<style scoped>
</style>
