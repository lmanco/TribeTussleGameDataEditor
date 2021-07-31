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
                        <round-list :initRounds="rounds" :updateActiveRound="updateActiveRound" ref="round-list"
                                    @numRoundsChanged="onNumRoundsChanged" />
                    </b-col>
                    <b-col cols="8" class="pl-2">
                        <round-form v-if="activeRound.id !== fastMoneyRoundId" :round="activeRound" :roundTitle="roundTitle"
                                    :addAnswerToRound="addAnswerToRound" :updateActiveRoundAnswers="updateActiveRoundAnswers"
                                    :deleteRound="deleteRound" :currentNumRounds="currentNumRounds" @roundChanged="onRoundChanged" />
                        <fast-money-round-form v-else :activeQuestion="activeFastMoneyQuestion" :questions="orderedFastMoneyQuestions"
                                               :updateOrderedQuestions="updateOrderedFastMoneyQuestions"
                                               :updateActiveQuestion="updateActiveFastMoneyQuestion" :addAnswerToRound="addAnswerToFastMoneyQuestion"
                                               :updateActiveRoundAnswers="updateActiveFastMoneyQuestionAnswers" @roundChanged="onRoundChanged" />
                    </b-col>
                </b-row>
            </b-container>
        </b-form>
        <div v-if="isDev">
            Media Query: {{ $mq }}
        </div>
        <b-modal id="error-messages-modal" ref="error-messages-modal" title="Error" ok-only>
            <b-container>
                <div v-for="errorMessage in errorMessages" class="error">
                    {{ errorMessage }}
                </div>
            </b-container>
        </b-modal>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Inject, Vue } from 'vue-property-decorator';
    import { GameDataService } from '@/services/API/GameDataService';
    import RoundList from './RoundList.vue';
    import Round from '@/components/types/Round';
    import RoundForm from './RoundForm.vue';
    import FastMoneyRoundForm from './FastMoneyRoundForm.vue';
    import Answer from '@/components/types/Answer';

    @Component({
        components: {
            RoundList,
            RoundForm,
            FastMoneyRoundForm
        }
    })
    export default class GameEditorForm extends Vue {
        @Inject() readonly gameDataService!: GameDataService;

        @Prop() readonly initGameDataName!: string;
        @Prop() readonly rounds!: Round[];
        @Prop() readonly fastMoneyQuestions!: Round[];

        private isDev: boolean = process.env.NODE_ENV === 'development';
        private gameDataName: string = '';
        private activeRound: Round = this.rounds[0];
        private activeFastMoneyQuestion: Round = this.fastMoneyQuestions[0];
        private orderedFastMoneyQuestions: Round[] = this.fastMoneyQuestions;
        private roundTitle: string = 'Round 1';
        private currentNumRounds: number = 2;
        private fastMoneyRoundId: number = this.rounds[this.rounds.length - 1].id;
        private isUnsaved: boolean = true;
        private lastSavedName: string = '';
        private errorMessages: string[] = [];

        public mounted(): void {
            if (this.initGameDataName)
                this.gameDataName = this.initGameDataName;
        }

        public updateActiveRound(round: Round, title: string): void {
            this.activeRound = round;
            this.roundTitle = title;
        }

        public updateActiveFastMoneyQuestion(question: Round): void {
            this.activeFastMoneyQuestion = question;
        }

        public updateOrderedFastMoneyQuestions(questions: Round[]): void {
            this.orderedFastMoneyQuestions = questions;
        }

        public addAnswerToRound(): void {
            if (this.activeRound.answers.length < 12) {
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

        public addAnswerToFastMoneyQuestion(): void {
            const nextId: number = Math.max(...this.activeFastMoneyQuestion.answers.map(answer => answer.id)) + 1;
            this.activeFastMoneyQuestion.answers.push({ id: nextId, text: '', value: 0 });
        }

        public updateActiveRoundAnswers(answers: Answer[]): void {
            this.activeRound.answers = answers;
            this.markUnsaved();
        }

        public updateActiveFastMoneyQuestionAnswers(answers: Answer[]): void {
            this.activeFastMoneyQuestion.answers = answers;
            this.markUnsaved();
        }

        public deleteRound(roundId: number): void {
            (this.$refs['round-list'] as any).deleteRound(roundId);
        }

        public onNumRoundsChanged(newNumRounds: number): void {
            this.currentNumRounds = newNumRounds;
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
                if (!this.lastSavedName) {
                    await this.gameDataService.createGameData({
                        name: this.gameDataName,
                        data: {
                            questions: rounds.slice(0, -1).map(round => ({
                                prompt: round.prompt,
                                scale: round.scale,
                                answers: round.answers.map(answer => ({
                                    text: answer.text,
                                    value: answer.value
                                }))
                            })),
                            fastMoney: this.orderedFastMoneyQuestions.map(question => ({
                                prompt: question.prompt,
                                answers: question.answers.map(answer => ({
                                    text: answer.text,
                                    value: answer.value
                                }))
                            }))
                        }
                    });
                }
                this.isUnsaved = false;
            }
            catch (apiResponseError) {
                this.errorMessages = apiResponseError.detailMessages
                    .reduce((messages: string[], message: string) => [...messages, ...message.split('\n')], []);
                (this.$refs['error-messages-modal'] as any).show();
                //this.$bvModal.msgBoxOk(apiResponseError.detailMessages.join('\n'), {
                //    title: 'Error',
                //    size: 'sm',
                //    buttonSize: 'sm',
                //    footerClass: 'p-2',
                //    hideHeaderClose: false,
                //    centered: true
                //});
            }
        }
    }
</script>

<style scoped>
    .error {
        color: red;
    }
</style>