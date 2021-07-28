<template>
    <div>
        <b-form inline>
            <b-container class="mt-4 ml-4 mr-4 bg-light border rounded-lg" fluid>
                <b-row class="mt-3">
                    <b-col>
                        <b-form-group id="form-group-game-data-name"
                                      label="Name:"
                                      label-size="lg"
                                      :label-cols="isMobile ? 3 : (isTablet ? 2 : 1)"
                                      :class="{ 'pl-3': mqOrdinal < mqOrdinals.tablet }"
                                      label-for="round-form-input-prompt">
                            <b-form-input id="form-input-game-data-name"
                                          v-model="gameDataName"
                                          required
                                          size="lg"
                                          class="w-100"
                                          placeholder="Enter game data name">
                            </b-form-input>
                        </b-form-group>
                    </b-col>
                </b-row>
                <b-row class="mt-3 mb-3 pl-2">
                    <b-col cols="4" class="pl-2 pr-1">
                        <round-list :initRounds="rounds" :updateActiveRound="updateActiveRound" />
                    </b-col>
                    <b-col cols="8" class="pl-2">
                        <round-form :round="activeRound ? activeRound : rounds[0]" :roundTitle="roundTitle" />
                    </b-col>
                </b-row>
                <b-row class="mb-2">
                    <b-col class="text-right">
                        <b-button variant="danger" class="mr-2 pl-4 pr-4">Delete</b-button>
                        <b-button variant="secondary" class="mr-2 pl-4 pr-4">Export</b-button>
                        <b-button variant="primary" class="pl-4 pr-4">Save</b-button>
                    </b-col>
                </b-row>
            </b-container>
        </b-form>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import RoundList from './RoundList.vue';
    import Round from '@/components/types/Round';
    import RoundForm from './RoundForm.vue';

    @Component({
        components: {
            RoundList,
            RoundForm
        }
    })
    export default class GameEditorForm extends Vue {
        @Prop() readonly initGameDataName!: string;
        @Prop() readonly rounds!: Round[];

        private gameDataName: string = '';
        private activeRound!: Round;
        private roundTitle: string;

        public constructor() {
            super();
            this.activeRound = this.rounds[0];
            this.roundTitle = 'Round 1';
        }

        public mounted(): void {
            if (this.initGameDataName)
                this.gameDataName = this.initGameDataName;
        }

        public updateActiveRound(round: Round, title: string): void {
            this.activeRound = round;
            this.roundTitle = title;
        }
    }
</script>

<style scoped>
</style>