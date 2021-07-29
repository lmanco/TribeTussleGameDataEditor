<template>
    <div class="mb-2">
        <h6 style="cursor: default;" v-if="!isMobile">Answers:</h6>
        <div class="border-top border rounded-lg">
            <div class="m-2">
                <b-button variant="primary" class="w-100" @click="addAnswer">
                    <b-icon-plus></b-icon-plus>{{ isMobile ? '' : 'Add Answer' }}
                </b-button>
            </div>
            <b-row>
                <b-col>
                    <draggable v-model="updatedAnswers" handle=".drag-handle" class="m-2" @change="answerDropped">
                        <transition-group>
                            <ul class="list-group" v-for="(answer, index) in answers" :key="answer.id">
                                <answer-list-item v-on="$listeners" :answer="answer" :index="index" :deleteAnswer="deleteAnswer" />
                            </ul>
                        </transition-group>
                    </draggable>
                </b-col>
            </b-row>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Watch, Vue } from 'vue-property-decorator';
    import draggable from 'vuedraggable';
    import Answer from '@/components/types/Answer';
    import AnswerListItem from './AnswerListItem.vue';

    @Component({
        components: {
            draggable,
            AnswerListItem
        }
    })
    export default class AnswerList extends Vue {
        @Prop() readonly answers!: Answer[];
        @Prop() readonly addAnswer!: () => void;
        @Prop() readonly updateAnswers!: (answers: Answer[]) => void;

        private updatedAnswers: Answer[];

        public constructor() {
            super();
            this.updatedAnswers = this.answers;
        }

        @Watch('answers')
        public onAnswersChanged(newAnswers: Answer[]): void {
            this.updatedAnswers = newAnswers;
        }

        public answerDropped(): void {
            this.updateAnswers(this.updatedAnswers);
        }

        public deleteAnswer(index: number): void {
            if (this.updatedAnswers.length <= 1) {
                this.$bvModal.msgBoxOk('A round must have at least one answer.', {
                    title: 'Information',
                    size: 'sm',
                    buttonSize: 'sm',
                    footerClass: 'p-2',
                    hideHeaderClose: false,
                    centered: true
                });
                return;
            }
            this.updatedAnswers = this.updatedAnswers.filter((_: Answer, curIndex: number) => curIndex !== index);
            this.updateAnswers(this.updatedAnswers);
        }
    }
</script>

<style scoped>
</style>