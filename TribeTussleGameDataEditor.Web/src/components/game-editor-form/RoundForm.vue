<template>
    <div class="border">
        <div :class="{ 'm-3': true, 'text-center': isMobile }">
            <h5 class="default-cursor">{{ roundTitle }}</h5>
            <div v-if="!isMobile">
                <b-row class="mb-2" align-v="center" v-if="!isFastMoneyRound">
                    <b-col :cols="mqOrdinal > mqOrdinals.notebook ? 1 : 2" class="default-cursor"
                           :style="getColStyle()">
                        Scale:
                    </b-col>
                    <b-col>
                        <b-form-select v-model="selectedScale" :options="scaleOptions"></b-form-select>
                    </b-col>
                </b-row>
                <b-row class="mb-2" align-v="center">
                    <b-col :cols="mqOrdinal > mqOrdinals.notebook ? 1 : 2" class="default-cursor"
                           :style="getColStyle()">
                        Prompt:
                    </b-col>
                    <b-col>
                        <b-form-input id="form-input-prompt"
                                      v-model="round.prompt"
                                      required
                                      class="w-100"
                                      placeholder="Enter prompt" />
                    </b-col>
                </b-row>
                <answer-list :answers="round.answers" :addAnswer="addAnswerToRound" :updateAnswers="updateActiveRoundAnswers" />
            </div>
            <div v-else>
                <b-form-group v-if="!isFastMoneyRound" id="form-group-scale"
                              class="mb-2"
                              label-for="form-input-prompt">
                    <b-form-select v-model="selectedScale" class="w-100" :options="scaleOptionsMobile"></b-form-select>
                </b-form-group>
                <b-form-group id="form-group-prompt"
                              class="mb-2"
                              label-for="form-input-prompt">
                    <b-form-input id="form-input-prompt"
                                  v-model="round.prompt"
                                  required
                                  class="w-100"
                                  placeholder="Enter prompt">
                    </b-form-input>
                </b-form-group>
                <answer-list :answers="round.answers" :addAnswer="addAnswerToRound" :updateAnswers="updateActiveRoundAnswers" />
            </div>
            <div :class="{ 'text-right': !isMobile, 'text-center': isMobile }">
                <b-button variant="danger" v-if="!isFastMoneyRound" @click="deleteCurrentRound">Delete Round</b-button>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import draggable from 'vuedraggable';
    import Round from '@/components/types/Round';
    import AnswerList from '@/components/game-editor-form/AnswerList.vue';

    interface ScaleOption {
        value: number,
        text: string
    }

    @Component({
        components: {
            draggable,
            AnswerList
        }
    })
    export default class RoundList extends Vue {
        @Prop() readonly round!: Round;
        @Prop() readonly roundTitle!: string;
        @Prop() readonly currentNumRounds!: number;
        @Prop() readonly deleteRound!: (roundId: number) => void;
        @Prop() readonly addAnswerToRound!: () => void;
        @Prop() readonly updateActiveRoundAnswers!: () => void;

        private readonly scaleOptions: ScaleOption[] = [
            { value: 1, text: 'Single' },
            { value: 2, text: 'Double' },
            { value: 3, text: 'Triple' }
        ];
        private readonly scaleOptionsMobile: ScaleOption[] = [
            { value: 0, text: 'Select Scale' },
            ...this.scaleOptions
        ];
        private selectedScale: number = 0;

        public mounted(): void {
            window.addEventListener("resize", this.checkScale);
            if (!(this as any).isMobile)
                this.selectedScale = 1;
        }

        public async deleteCurrentRound(): Promise<void> {
            if (await this.showDeletionConfirmationModal())
                this.deleteRound(this.round.id);
        }

        private async showDeletionConfirmationModal(): Promise<boolean> {
            if (this.currentNumRounds <= 2) {
                await this.$bvModal.msgBoxOk('There must be at least one main game round per game.', {
                    title: 'Information',
                    size: 'sm',
                    buttonSize: 'sm',
                    footerClass: 'p-2',
                    hideHeaderClose: false,
                    centered: true
                });
                return false;
            }
            else {
                return await this.$bvModal.msgBoxConfirm('Are you sure you want to delete this round?', {
                    title: 'Confirm',
                    size: 'sm',
                    buttonSize: 'sm',
                    okTitle: 'Yes',
                    cancelTitle: 'No',
                    footerClass: 'p-2',
                    hideHeaderClose: false,
                    centered: true
                });
            }
        }

        public getColStyle(): object {
            if ((this as any).isNotebook)
                return { 'max-width': '10.5%' };
            return {};
        }

        private checkScale(): void {
            if (!(this as any).isMobile && this.selectedScale === 0)
                this.selectedScale = 1;
        }

        get isFastMoneyRound() {
            return this.roundTitle === 'Fast Money Round';
        }
    }
</script>

<style scoped>
    .default-cursor {
        cursor: default;
    }
</style>