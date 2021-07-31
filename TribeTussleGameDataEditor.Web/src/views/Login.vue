<template>
    <div>
        <div class="home page-header bg-dark text-light">
            <b-container class="text-center">
                <b-row align-h="center">
                    <h1>Tribe Tussle Game Data Editor</h1>
                </b-row>
                <b-row align-h="center">
                    <p>A generic game data editor for Tribe Tussle.</p>
                </b-row>
            </b-container>
        </div>
        <login-form :initMessages="initMessages" :initMessagesTitle="initMessagesTitle" />
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import LoginForm from '@/components/LoginForm.vue';
    import RegisterForm from '@/components/RegisterForm.vue';

    @Component({
        components: {
            LoginForm,
            RegisterForm
        }
    })
    export default class Login extends Vue {
        @Prop() readonly initMessages!: string[];
        @Prop() readonly initMessagesTitle!: string;
    }
</script>

<style scoped>
    h1 {
        margin-top: 0.5rem;
    }
</style>