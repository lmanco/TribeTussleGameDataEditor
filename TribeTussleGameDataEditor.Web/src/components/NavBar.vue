<template>
    <div>
        <b-navbar toggleable="lg" type="light" variant="light">
            <b-navbar-brand href="#">Tribe Tussle Game Data Editor ({{ $mq }})</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">
                    <b-nav-form>
                        <b-input-group>
                            <b-input-group-prepend is-text>
                                <b-icon icon="search"></b-icon>
                            </b-input-group-prepend>
                            <b-form-input list="games-list" placeholder="Select Game Data..."></b-form-input>
                            <b-form-datalist id="games-list" :options="gameNames"></b-form-datalist>
                        </b-input-group>
                    </b-nav-form>
                    <b-nav-item-dropdown>
                        <template #button-content>
                            <em>
                                <b-icon icon="plus"></b-icon>
                                Add Game Data
                            </em>
                        </template>
                        <b-dropdown-item href="#" @click="createNewGame">Create New Game Data</b-dropdown-item>
                        <b-dropdown-item href="#">Import Game Data</b-dropdown-item>
                    </b-nav-item-dropdown>
                    <b-nav-item-dropdown right>
                        <!-- Using 'button-content' slot -->
                        <template #button-content>
                            <em>{{ username }}</em>
                        </template>
                        <b-dropdown-item href="#" @click="logOut">Log Out</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
        <b-alert v-if="errors.length" id="errors-alert" variant="danger" dismissible show>
            <div v-for="error in errors">
                {{ error }}
            </div>
        </b-alert>
    </div>
</template>

<script lang="ts">
    import { Component, Inject, Prop, Vue } from 'vue-property-decorator';
    import { GameDataService } from '@/services/API/GameDataService';
    import { LoginService } from '@/services/API/LoginService';
    import { FormState } from './enums';

    @Component
    export default class NavBar extends Vue {
        @Inject() readonly loginService!: LoginService;
        @Inject() readonly gameDataService!: GameDataService;

        @Prop() username!: string;

        private gameNames: string[] = [];
        private errors: string[] = [];
        private state: FormState = FormState.Ready;

        public async mounted(): Promise<void> {
            try {
                this.gameNames = await this.gameDataService.getGameNames();
            }
            catch (apiResponseError) {
                this.errors = ['Failed to fetch existing games. Please try reloading the page.'];
            }
        }

        public createNewGame(): void {
            this.$router.push('/create-game-data')
        }

        public async logOut(): Promise<void> {
            if (this.state !== FormState.Ready)
                return;
            await this.loginService.logout();
            this.$router.push('/login');
        }
    }
</script>

<style scoped>
</style>