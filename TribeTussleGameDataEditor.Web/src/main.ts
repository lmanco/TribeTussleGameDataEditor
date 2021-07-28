import Vue from 'vue';
import App from './App.vue';
import router from './router';
import { BootstrapVue, BootstrapVueIcons } from 'bootstrap-vue';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import VueMq from 'vue-mq';

Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);

Vue.use(VueMq, {
    breakpoints: {
        mobile: 450,
        tablet: 900,
        laptop: 1250,
        desktop: Infinity
    }
});

Vue.mixin({
    data: function () {
        return {
            get mqOrdinals() {
                return {
                    mobile: 0,
                    tablet: 1,
                    laptop: 2,
                    desktop: 3
                }
            }
        }
    },
    computed: {
        isMobile: function () {
            return this.mqOrdinal === (this.mqOrdinals as any).mobile;
        },
        isTablet: function () {
            return this.mqOrdinal === (this.mqOrdinals as any).tablet;
        },
        isDesktop: function () {
            return this.mqOrdinal === (this.mqOrdinals as any).desktop;
        },
        mqOrdinal: function () {
            return (this.mqOrdinals as any)[this.$mq as string];
        }
    }
})


Vue.config.productionTip = false;

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');
