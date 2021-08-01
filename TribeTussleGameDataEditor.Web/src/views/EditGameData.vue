<template>
    <div v-if="authenticated">
        <nav-bar :username="authenticatedUser.username" ref="nav-bar" />
        <game-editor-form v-if="dataLoaded" :initGameDataName="gameDataName" :updateNavBarGameNames="updateNavBarGameNames"
                          :rounds="loadedRounds" :fastMoneyQuestions="loadedFastMoneyQuestions"/>
        <div v-else-if="loadingSpinner" id="loading-wrapper">
            <div class="loading spinner-border text-primary row align-items-center" role="status">
                <span class="sr-only">Loading...</span>
            </div>
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
    import { Component, Inject } from 'vue-property-decorator';
    import AuthenticatedView from './AuthenticatedView';
    import NavBar from '@/components/NavBar.vue';
    import GameEditorForm from '@/components/game-editor-form/GameEditorForm.vue';
    import GameDataService from '@/services/API/GameDataService';
    import GameData, { Question, MainGameQuestion } from '@/services/API/types/GameData';
    import Round from '@/components/types/Round';
    import { defaultRoundList, defaultFastMoneyQuestionList } from '@/views/Constants';

    @Component({
        components: {
            NavBar,
            GameEditorForm
        }
    })
    export default class EditGameData extends AuthenticatedView {
        @Inject() readonly gameDataService!: GameDataService;

        private gameDataName: string = '';
        private dataLoaded: boolean = false;
        private loadingSpinner: boolean = false;
        private loadedRounds: Round[] = defaultRoundList;
        private loadedFastMoneyQuestions: Round[] = defaultFastMoneyQuestionList;
        private errorMessages: string[] = [];

        public async mounted(): Promise<void> {
            const timerId: number = setTimeout(() => this.loadingSpinner = true, 1000);
            this.gameDataName = this.$route.params.name;
            try {
                const gameData: GameData = await this.gameDataService.getGameData(this.gameDataName);
                this.loadedRounds = this.extractRoundsFromResponseQuestions(gameData.data.questions);
                this.loadedRounds.push({
                    id: this.loadedRounds.length + 1,
                    prompt: '',
                    scale: 1,
                    answers: [
                        { id: 1, text: '', value: 0 }
                    ],
                    active: false
                });
                this.loadedFastMoneyQuestions = this.extractRoundsFromResponseQuestions(gameData.data.fastMoney);
            }
            catch (apiResponseError) {
                this.errorMessages = apiResponseError.detailMessages;
                (this.$refs['error-messages-modal'] as any).show();
            }
            clearTimeout(timerId);
            this.loadingSpinner = false;
            this.dataLoaded = true;
        }

        public async updateNavBarGameNames(): Promise<void> {
            await (this.$refs['nav-bar'] as any).fetchGameNames();
        }

        private extractRoundsFromResponseQuestions(questions: Question[]): Round[] {
            return questions.map((question, index) => ({
                id: index + 1,
                prompt: question.prompt,
                scale: (question as MainGameQuestion).scale ? (question as MainGameQuestion).scale : 1,
                answers: question.answers.map((answer, answerIndex) => ({
                    id: answerIndex + 1,
                    text: answer.text,
                    value: answer.value
                })),
                active: index === 0
            }));
        }
    }
</script>

<style scoped>
    .error {
        color: red;
    }

    #loading-wrapper {
        position: fixed;
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        top: 0;
    }

    .loading {
        display: flex;
        margin: 0 auto;
    }
</style>