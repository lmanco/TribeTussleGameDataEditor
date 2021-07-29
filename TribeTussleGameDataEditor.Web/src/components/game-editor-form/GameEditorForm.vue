<template>
    <div>
        <b-form inline>
            <b-container class="mt-4 ml-4 mr-4 bg-light border rounded-lg" fluid>
                <b-row class="mt-3">
                    <b-col>
                        <b-form-group id="form-group-game-data-name"
                                      label="Name:"
                                      label-size="lg"
                                      :label-cols="isMobile ? 3 : 2"
                                      :class="{ 'pl-3': mqOrdinal < mqOrdinals.tablet }"
                                      label-for="round-form-input-prompt">
                            <b-form-input id="form-input-game-data-name"
                                          v-model="gameDataName"
                                          required
                                          size="lg"
                                          :style="{ 'width': mqOrdinal <= mqOrdinals.tablet ? '100%' : '110%' }"
                                          :placeholder="isMobile ? 'Enter name' : 'Enter game data name'">
                            </b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col cols="1" class="text-right" style="left: 2%;" v-if="mqOrdinal > mqOrdinals.tablet">
                        <p class="font-italic text-muted" style="cursor: default;">{{ isUnsaved ? '(unsaved)' : '' }}</p>
                    </b-col>
                    <b-col cols="5" class="mt-1 text-right" v-if="mqOrdinal > mqOrdinals.tablet">
                        <b-button variant="danger" class="mr-2 pl-4 pr-4" @click="deleteGameData">Delete</b-button>
                        <b-button variant="secondary" class="mr-2 pl-4 pr-4" @click="exportGameData">Export</b-button>
                        <b-button variant="primary" class="pl-4 pr-4" @click="saveGameData">Save</b-button>
                    </b-col>
                </b-row>
                <div :class="{ 'mt-3': isTablet, 'text-center': true }" v-if="mqOrdinal <= mqOrdinals.tablet && isUnsaved">
                    <p class="font-italic text-muted" style="cursor: default;">(unsaved)</p>
                </div>
                <div :class="{ 'mt-3': isTablet, 'text-center': true }" v-if="mqOrdinal <= mqOrdinals.tablet">
                    <b-button variant="danger" class="mr-2 pl-4 pr-4" @click="deleteGameData">Delete</b-button>
                    <b-button variant="secondary" class="mr-2 pl-4 pr-4" @click="exportGameData">Export</b-button>
                    <b-button variant="primary" class="pl-4 pr-4" @click="saveGameData">Save</b-button>
                </div>
                <b-row class="mt-3 pl-2">
                    <b-col cols="4" class="pl-2 pr-1">
                        <round-list :initRounds="rounds" :updateActiveRound="updateActiveRound" ref="round-list" />
                    </b-col>
                    <b-col cols="8" class="pl-2">
                        <round-form :round="activeRound" :roundTitle="roundTitle"
                                    :addAnswerToRound="addAnswerToRound" :updateActiveRoundAnswers="updateActiveRoundAnswers"
                                    :deleteRound="deleteRound" :currentNumRounds="currentNumRounds" @roundChanged="onRoundChanged" />
                    </b-col>
                </b-row>
            </b-container>
        </b-form>
        <div v-if="isDev">
            Media Query: {{ $mq }}
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Inject, Vue } from 'vue-property-decorator';
    import { GameDataService } from '@/services/API/GameDataService';
    import RoundList from './RoundList.vue';
    import Round from '@/components/types/Round';
    import RoundForm from './RoundForm.vue';
    import Answer from '@/components/types/Answer';

    @Component({
        components: {
            RoundList,
            RoundForm
        }
    })
    export default class GameEditorForm extends Vue {
        @Inject() readonly gameDataService!: GameDataService;

        @Prop() readonly initGameDataName!: string;
        @Prop() readonly rounds!: Round[];

        private isDev: boolean = process.env.NODE_ENV === 'development';
        private gameDataName: string = '';
        private activeRound: Round = this.rounds[0];
        private roundTitle: string;
        private currentNumRounds: number = 2;
        private fastMoneyRoundId: number = this.rounds[this.rounds.length - 1].id;
        private isUnsaved: boolean = true;
        private lastSavedName: string = '';

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

        public addAnswerToRound(): void {
            if (this.activeRound.id === this.fastMoneyRoundId || this.activeRound.answers.length < 12) {
                const nextId: number = Math.max(...this.activeRound.answers.map(answer => answer.id)) + 1;
                this.activeRound.answers.push({ id: nextId, text: '', value: 0 });
            }
            else {
                this.$bvModal.msgBoxOk('There may not be more than 12 answers in a main game round.', {
                    title: 'Information',
                    size: 'sm',
                    buttonSize: 'sm',
                    footerClass: 'p-2',
                    hideHeaderClose: false,
                    centered: true
                });
            }
        }

        public updateActiveRoundAnswers(answers: Answer[]): void {
            this.activeRound.answers = answers;
            this.markUnsaved();
        }

        public deleteRound(roundId: number): void {
            (this.$refs['round-list'] as any).deleteRound(roundId);
        }

        public onRoundChanged(): void {
            this.markUnsaved();
        }

        public markUnsaved(): void {
            this.isUnsaved = true;
        }

        public async deleteGameData(): Promise<void> {
            if (await this.$bvModal.msgBoxConfirm(
                'Are you sure you want to delete the current game data? This operation cannot be undone.',
                {
                    title: 'Confirm',
                    size: 'sm',
                    buttonSize: 'sm',
                    okTitle: 'Yes',
                    cancelTitle: 'No',
                    footerClass: 'p-2',
                    hideHeaderClose: false,
                    centered: true
                }
            )) {
                this.$router.push('/');
            }
        }

        public exportGameData(): void {

        }

        public async saveGameData(): Promise<void> {
            const rounds: Round[] = (this.$refs['round-list'] as any).rounds;
            try {
                //if (!this.lastSavedName) {
                //    await this.gameDataService.createGameData({
                //        name: this.gameDataName,
                //        data: {
                //            questions: rounds.map(round => ({
                //                prompt: round.prompt,
                //                scale: round.scale,
                //                answers: round.answers.map(answer => ({
                //                    text: answer.text,
                //                    value: answer.value
                //                }))
                //            })),
                //            fastMoney: null
                //        }
                //    });
                //}
                this.isUnsaved = false;
            }
            catch (apiResponseError) {

            }
        }
    }
</script>

<style scoped>
</style>