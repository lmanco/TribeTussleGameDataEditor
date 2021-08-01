<template>
    <div id="app">
        <div id="content">
            <router-view :key="$route.fullPath" />
        </div>
        <footer class="text-center p-3" style="background-color: rgba(0, 0, 0, 0.05);">
            &copy; 2021 About Airplane LLC, All Rights Reserved
        </footer>
    </div>
</template>

<script lang="ts">
    import { Component, Vue, Provide } from 'vue-property-decorator';
    import RequesterGameDataService, { GameDataService } from './services/API/GameDataService';
    import RequesterLoginService, { LoginService } from "./services/API/LoginService";
    import { FetchRequester, Requester } from './services/API/Requester';
    import RequesterUserService, { UserService } from './services/API/UserService';

    interface Services {
        requester: Requester;
        loginService: LoginService;
        userService: UserService;
        gameDataService: GameDataService;
    }

    const requester: Requester = new FetchRequester();
    const siteUrlOverride: string | undefined = process.env.NODE_ENV === 'development' ?
        `${window.location.protocol}//${window.location.host}` : undefined;
    const services: Services = {
        requester,
        loginService: new RequesterLoginService(requester, siteUrlOverride),
        userService: new RequesterUserService(requester, siteUrlOverride),
        gameDataService: new RequesterGameDataService(requester)
    };

    @Component
    export default class App extends Vue {
        @Provide() loginService = services.loginService;
        @Provide() userService = services.userService;
        @Provide() gameDataService = services.gameDataService;
    }
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        color: #2c3e50;

       display: flex;
       flex-direction: column;
       min-height: 100vh;
    }

    #nav {
        padding: 30px;
    }

    #nav a {
        font-weight: bold;
        color: #2c3e50;
    }

    #nav a.router-link-exact-active {
        color: #42b983;
    }

    body {
        background-color: #EEF1F4 !important;
    }

    div#content {
        flex-grow: 1;
    }

    footer {
        margin-top: 12px;
        font-size: 0.85rem;
    }
</style>
