<template>
    <div id="app">
        <router-view/>
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
</style>
