<template>
    <div class="border mb-3">
        <div :class="{ 'mt-3': true, 'mb-2': true, 'text-center': isMobile }">
            <h5 class="default-cursor ml-3 mr-3">Fast Money Round</h5>
            <div v-if="!isMobile">
                <round-list :initRounds="questions" :updateActiveRound="updateActiveQuestion" :isFastMoneyQuestionList="true"
                            :updateFullList="updateOrderedQuestions" :initSelectedId="activeQuestion.id"/>
                <b-row class="mb-2 mr-1" align-v="center">
                    <b-col :cols="mqOrdinal > mqOrdinals.notebook ? 1 : 2" class="default-cursor ml-3 mr-3"
                           :style="getColStyle()">
                        Prompt:
                    </b-col>
                    <b-col>
                        <b-form-input id="form-input-prompt"
                                      v-model="activeQuestion.prompt"
                                      @input="$parent.markUnsaved()"
                                      required
                                      class="w-100"
                                      placeholder="Enter prompt" />
                    </b-col>
                </b-row>
                <answer-list v-on="$listeners" class="ml-3 mr-3"
                             :answers="activeQuestion.answers" :addAnswer="addAnswerToRound" :updateAnswers="updateActiveRoundAnswers" />
            </div>
            <div v-else>
                <round-list :initRounds="questions" :updateActiveRound="updateActiveQuestion" :isFastMoneyQuestionList="true"
                            :updateFullList="updateOrderedQuestions" :initSelectedId="activeQuestion.id"/>
                <b-form-group id="form-group-prompt"
                              class="mb-2"
                              label-for="form-input-prompt">
                    <b-form-input id="form-input-prompt"
                                  v-model="activeQuestion.prompt"
                                  @input="$parent.markUnsaved()"
                                  required
                                  class="w-100"
                                  placeholder="Enter prompt">
                    </b-form-input>
                </b-form-group>
                <answer-list v-on="$listeners"
                             :answers="activeQuestion.answers" :addAnswer="addAnswerToRound" :updateAnswers="updateActiveRoundAnswers" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import draggable from 'vuedraggable';
    import Round from '@/components/types/Round';
    import RoundList from '@/components/game-editor-form/RoundList.vue';
    import AnswerList from '@/components/game-editor-form/AnswerList.vue';
    import Answer from '@/components/types/Answer';

    @Component({
        components: {
            draggable,
            RoundList,
            AnswerList
        }
    })
    export default class FastMoneyRoundForm extends Vue {
        @Prop() readonly activeQuestion!: Round;
        @Prop() readonly questions!: Round[];
        @Prop() readonly addAnswerToRound!: () => void;
        @Prop() readonly updateActiveQuestion!: (question: Round) => void;
        @Prop() readonly updateOrderedQuestions!: (questions: Round[]) => void;
        @Prop() readonly updateActiveRoundAnswers!: (answers: Answer[]) => void;

        public getColStyle(): object {
            if ((this as any).isNotebook)
                return { 'max-width': '10.5%' };
            return {};
        }
    }
</script>

<style scoped>
    .default-cursor {
        cursor: default;
    }
</style>