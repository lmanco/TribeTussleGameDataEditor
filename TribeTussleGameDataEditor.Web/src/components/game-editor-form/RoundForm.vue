<template>
    <div class="border">
        <div :class="{ 'm-3': true, 'text-center': isMobile }">
            <h5>{{ roundTitle }}</h5>
            <div v-if="mqOrdinal >= mqOrdinals.laptop">
                <b-row class="mb-2">
                    <b-col :cols="isFastMoneyRound? 12 : 9">
                        <b-form-group id="form-group-prompt"
                                      label="Prompt:"
                                      :label-cols="isDesktop ? 1 : 2"
                                      class="ml-1"
                                      label-for="form-input-prompt">
                            <b-form-input id="form-input-prompt"
                                          v-model="round.prompt"
                                          required
                                          class="w-100"
                                          placeholder="Enter prompt">
                            </b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col v-if="!isFastMoneyRound">
                        <b-form-group id="form-group-scale"
                                      label="Scale:"
                                      label-cols="3"
                                      label-for="form-input-prompt">
                            <b-form-select v-model="selectedScale" class="w-100" :options="scaleOptions"></b-form-select>
                        </b-form-group>
                    </b-col>
                </b-row>
            </div>
            <div v-else>
                <b-form-group id="form-group-prompt"
                              :label="isMobile ? '' : 'Prompt:'"
                              :label-cols="isMobile ? 0 : 3"
                              :class="{ 'ml-1': isTablet, 'mb-2': true }"
                              label-for="form-input-prompt">
                    <b-form-input id="form-input-prompt"
                                  v-model="round.prompt"
                                  required
                                  class="w-100"
                                  placeholder="Enter prompt">
                    </b-form-input>
                </b-form-group>
                <b-form-group v-if="!isFastMoneyRound" id="form-group-scale"
                              :label="isMobile ? '' : 'Scale:'"
                              :label-cols="isMobile ? 0 : 3"
                              :class="{ 'ml-1': isTablet, 'mb-2': true }"
                              label-for="form-input-prompt">
                    <b-form-select v-model="selectedScale" class="w-100" :options="isMobile ? scaleOptionsMobile : scaleOptions"></b-form-select>
                </b-form-group>
            </div>
            <b-row>
                <b-col :class="{ 'text-right': !isMobile, 'text-center': isMobile }">
                    <b-button variant="danger" v-if="!isFastMoneyRound" @click="deleteCurrentRound">Delete Round</b-button>
                </b-col>
            </b-row>
        </div>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from 'vue-property-decorator';
    import draggable from 'vuedraggable';
    import Round from '@/components/types/Round';

    interface ScaleOption {
        value: number,
        text: string
    }

    @Component({
        components: {
            draggable
        }
    })
    export default class RoundList extends Vue {
        @Prop() readonly round!: Round;
        @Prop() readonly roundTitle!: string;
        @Prop() readonly currentNumRounds!: number;
        @Prop() readonly deleteRound!: (roundId: number) => void;

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
</style>